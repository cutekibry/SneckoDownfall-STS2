
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using SneckoDownfall.SneckoDownfallCode.Relics;

namespace SneckoDownfall.SneckoDownfallCode.Utils;

public static class GiftSelector
{
    public static IEnumerable<CardModel> GetGiftCards(Player player, Func<CardModel, bool> filter)
    {
        var pools = player.GetRelic<SneckoSoul>()!.CharacterIds.Select(ModelDb.GetById<CharacterModel>).Select(c => c.CardPool);
        var cards = pools.SelectMany(c => c.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint));
        return cards.Where(filter);
    }

    public static async Task GetGiftReward(Player player, Func<CardModel, bool> filter)
    {
        var candidateCards = GetGiftCards(player, filter);
        var options = new CardCreationOptions(candidateCards, CardCreationSource.Other, CardRarityOddsType.RegularEncounter);
        var options2 = (from r in CardFactory.CreateForReward(player, 3, options)
                                    select r.Card).ToList();
        var chosenCard = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), options2, player, canSkip: true);
        if (chosenCard != null)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(chosenCard, PileType.Deck));
    }

    public static bool IsDebuff(CardModel card)
    {
        return card.HoverTips.Any(t => t.IsDebuff) || card.DynamicVars.ContainsKey("StrengthLoss") || card.DynamicVars.ContainsKey("DexterityLoss");
    }
}