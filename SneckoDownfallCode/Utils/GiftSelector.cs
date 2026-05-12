
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using SneckoDownfall.SneckoDownfallCode.Character;
using SneckoDownfall.SneckoDownfallCode.Relics;

namespace SneckoDownfall.SneckoDownfallCode.Utils;

public static class GiftSelector
{
    private const int RewardOptionCount = 3;

    public static IEnumerable<CardModel> GetGiftCards(Player player, Func<CardModel, bool> filter)
    {
        var sneckoSoul = player.GetRelic<SneckoSoul>();

        var pools =
            (sneckoSoul == null)
            ?
            player.UnlockState.CharacterCardPools.Where(p => p is not SneckoDownfallCardPool)
            :
            sneckoSoul.CharacterIds.Select(ModelDb.GetById<CharacterModel>).Select(c => c.CardPool);

        var cards = pools.SelectMany(c => c.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)).Where(c => c.Rarity != CardRarity.Basic && c.Rarity != CardRarity.Ancient);

        Log.Info($"GiftSelector: Found {cards.Count()} candidate cards for player {player} with filter {filter}");
        return cards.Where(filter);
    }

    public static async Task OfferGiftReward(Player player, Func<CardModel, bool> filter, bool upgraded = false)
    {
        var candidateCards = GetGiftCards(player, filter).ToList();
        var options = new CardCreationOptions(candidateCards, CardCreationSource.Other, GetCardCreationOptionsRarityOdds(candidateCards))
            .WithFlags(CardCreationFlags.IsCardReward);
        var rerollOptions = new CardCreationOptions(candidateCards, CardCreationSource.Other, CardRarityOddsType.Uniform)
            .WithFlags(CardCreationFlags.IsCardReward);

        if (upgraded)
        {
            options.WithFlags(CardCreationFlags.NoUpgradeRoll);
            rerollOptions.WithFlags(CardCreationFlags.NoUpgradeRoll);
        }

        var cards = CreateGiftRewardCards(player, options, RewardOptionCount);
        if (upgraded)
        {
            CardCmd.Upgrade(cards, CardPreviewStyle.None);
        }

        Reward reward = upgraded
            ? new UpgradedCardReward(cards, CardCreationSource.Other, player, rerollOptions)
            : new CardReward(cards, CardCreationSource.Other, player, rerollOptions);

        await RewardsCmd.OfferCustom(player, [reward]);
    }

    private static CardRarityOddsType GetCardCreationOptionsRarityOdds(IReadOnlyCollection<CardModel> cards)
    {
        return cards.Select(c => c.Rarity).Distinct().Skip(1).Any()
            ? CardRarityOddsType.RegularEncounter
            : CardRarityOddsType.Uniform;
    }

    private static List<CardModel> CreateGiftRewardCards(Player player, CardCreationOptions options, int cardCount)
    {
        options = Hook.ModifyCardRewardCreationOptions(player.RunState, player, options);
        var possibleCards = FilterForPlayerCount(player, options.GetPossibleCards(player)).ToList();
        var generatedCards = new List<CardModel>();
        var blacklist = new HashSet<ModelId>();

        for (int i = 0; i < cardCount; i++)
        {
            var remainingCards = possibleCards.Where(c => !blacklist.Contains(c.Id)).ToList();
            if (remainingCards.Count == 0)
            {
                throw new InvalidOperationException($"Tried to create a Gift card reward, but there were no cards left. Card pool: {string.Join(",", possibleCards)}, blacklist: {string.Join(",", blacklist)}");
            }

            var rarity = RollAvailableRegularEncounterRarity(player, remainingCards);
            var cardsOfRarity = remainingCards.Where(c => c.Rarity == rarity).ToList();
            var canonicalCard = player.PlayerRng.Rewards.NextItem(cardsOfRarity)
                ?? throw new InvalidOperationException($"Tried to create a Gift card reward, but no card matched rarity {rarity}. Card pool: {string.Join(",", remainingCards)}");
            var card = player.RunState.CreateCard(canonicalCard, player);

            blacklist.Add(canonicalCard.Id);
            generatedCards.Add(card);

            if (!options.Flags.HasFlag(CardCreationFlags.NoUpgradeRoll))
            {
                RollForUpgrade(player, card);
            }
        }

        return generatedCards;
    }

    private static IEnumerable<CardModel> FilterForPlayerCount(Player player, IEnumerable<CardModel> cards)
    {
        if (player.RunState.Players.Count > 1)
        {
            return cards.Where(c => c.MultiplayerConstraint != CardMultiplayerConstraint.SingleplayerOnly);
        }

        return cards.Where(c => c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly);
    }

    private static CardRarity RollAvailableRegularEncounterRarity(Player player, IReadOnlyCollection<CardModel> cards)
    {
        var availableRarities = cards.Select(c => c.Rarity).ToHashSet();
        var weights = new List<(CardRarity Rarity, float Weight)>
        {
            (CardRarity.Common, GetRegularEncounterRarityWeight(CardRarity.Common)),
            (CardRarity.Uncommon, GetRegularEncounterRarityWeight(CardRarity.Uncommon)),
            (CardRarity.Rare, GetRegularEncounterRarityWeight(CardRarity.Rare)),
        }
            .Where(w => availableRarities.Contains(w.Rarity) && w.Weight > 0f)
            .ToList();

        if (weights.Count == 0)
        {
            throw new InvalidOperationException($"Tried to create a Gift card reward, but no regular reward rarity was available. Card pool: {string.Join(",", cards)}");
        }

        var totalWeight = weights.Sum(w => w.Weight);
        var roll = player.PlayerRng.Rewards.NextFloat(totalWeight);
        foreach (var (rarity, weight) in weights)
        {
            roll -= weight;
            if (roll <= 0f)
            {
                return rarity;
            }
        }

        return weights[^1].Rarity;
    }

    private static float GetRegularEncounterRarityWeight(CardRarity rarity)
    {
        return rarity switch
        {
            // Mirrors CardRarityOdds.RollWithBaseOdds(RegularEncounter): rare first,
            // then uncommon threshold, with the remainder becoming common.
            CardRarity.Common => 1f - CardRarityOdds.regularUncommonOdds,
            CardRarity.Uncommon => CardRarityOdds.regularUncommonOdds - CardRarityOdds.RegularRareOdds,
            CardRarity.Rare => CardRarityOdds.RegularRareOdds,
            _ => 0f,
        };
    }

    private static void RollForUpgrade(Player player, CardModel card)
    {
        var roll = player.PlayerRng.Rewards.NextFloat();
        if (!card.IsUpgradable)
        {
            return;
        }

        decimal upgradeOdds = 0m;
        if (card.Rarity != CardRarity.Rare)
        {
            var upgradedCardOddScaling = AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.125m, 0.25m);
            upgradeOdds += player.RunState.CurrentActIndex * upgradedCardOddScaling;
        }

        upgradeOdds = Hook.ModifyCardRewardUpgradeOdds(player.RunState, player, card, upgradeOdds);
        if ((decimal)roll <= upgradeOdds)
        {
            CardCmd.Upgrade(card);
        }
    }

    private sealed class UpgradedCardReward : CardReward
    {
        public UpgradedCardReward(IEnumerable<CardModel> cardsToOffer, CardCreationSource source, Player player, CardCreationOptions rerollOptions)
            : base(cardsToOffer, source, player, rerollOptions)
        {
            AfterGenerated += UpgradeGeneratedCards;
        }

        private void UpgradeGeneratedCards()
        {
            CardCmd.Upgrade(Cards, CardPreviewStyle.None);
        }
    }

    public static bool IsDebuff(CardModel card)
    {
        return card.HoverTips.Any(t => t.IsDebuff) || card.DynamicVars.ContainsKey("StrengthLoss") || card.DynamicVars.ContainsKey("DexterityLoss");
    }
}
