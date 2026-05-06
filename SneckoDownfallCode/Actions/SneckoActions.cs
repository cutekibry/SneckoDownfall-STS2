using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using SneckoDownfall.SneckoDownfallCode.CardSelectorPref;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Actions;

public static class SneckoActions
{
    public static Task Muddle(CardModel card)
    {
        if (card.EnergyCost.Canonical < 0 || card.EnergyCost.CostsX)
            return Task.CompletedTask;
        int cost = card.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
        card.EnergyCost.SetThisTurn(cost);
        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        return Task.CompletedTask;
    }
    public static async Task MuddleHand(PlayerChoiceContext ctx, CardModel card, int amount)
    {
        var cards = await CardSelectCmd.FromHand(ctx, card.Owner, new CardSelectorPrefs(SneckoDownfallCardSelectorPrefs.MuddleSelectionPrompt, amount), c => c.CanBeMuddled(), card);
        await Muddle(cards);
    }
    public static async Task MuddleHand(PlayerChoiceContext ctx, CardModel card)
    {
        await MuddleHand(ctx, card, card.DynamicVars["Muddle"].IntValue);
    }

    public static Task Muddle(IEnumerable<CardModel> cards)
    {
        return Task.WhenAll(cards.Select(Muddle));
    }
}