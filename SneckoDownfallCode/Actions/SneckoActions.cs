using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace SneckoDownfall.SneckoDownfallCode.Actions;

public static class SneckoActions
{
    public static Task Muddle(CardModel card)
    {
        if (card.EnergyCost.Canonical < 0)
            return Task.CompletedTask;
        int cost = card.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
        card.EnergyCost.SetThisTurn(cost);
        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        return Task.CompletedTask;
    }

    public static Task Muddle(IEnumerable<CardModel> cards)
    {
        return Task.WhenAll(cards.Select(Muddle));
    }
}