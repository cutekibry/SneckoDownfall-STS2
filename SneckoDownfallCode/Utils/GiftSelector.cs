
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using SneckoDownfall.SneckoDownfallCode.Character;
using SneckoDownfall.SneckoDownfallCode.Relics;

namespace SneckoDownfall.SneckoDownfallCode.Utils;

public static class GiftSelector
{
    public static IEnumerable<CardModel> GetGiftCards(Player player, Func<CardModel, bool> filter)
    {
        var sneckoSoul = player.GetRelic<SneckoSoul>();

        var pools =
            (sneckoSoul == null)
            ?
            player.UnlockState.CharacterCardPools.Where(p => p is not SneckoDownfallCardPool)
            :
            sneckoSoul.CharacterIds.Select(ModelDb.GetById<CharacterModel>).Select(c => c.CardPool);

        var cards = pools.SelectMany(c => c.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint));

        Log.Info($"GiftSelector: Found {cards.Count()} candidate cards for player {player} with filter {filter}");
        return cards.Where(filter);
    }

    public static async Task OfferGiftReward(Player player, Func<CardModel, bool> filter, bool upgraded = false)
    {
        var candidateCards = GetGiftCards(player, filter);
        var isRarityAllTheSame = candidateCards.Select(c => c.Rarity).Distinct().Count() <= 1;

        var options = new CardCreationOptions(candidateCards, CardCreationSource.Other, isRarityAllTheSame ? CardRarityOddsType.Uniform : CardRarityOddsType.RegularEncounter);
        Reward reward = upgraded
            ? new UpgradedCardReward(options, 3, player)
            : new CardReward(options, 3, player);

        await RewardsCmd.OfferCustom(player, [reward]);
    }

    private sealed class UpgradedCardReward : CardReward
    {
        public UpgradedCardReward(CardCreationOptions options, int cardCount, Player player)
            : base(options.WithFlags(CardCreationFlags.NoUpgradeRoll), cardCount, player)
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
