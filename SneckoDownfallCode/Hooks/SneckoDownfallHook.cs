

using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Cards;
using SneckoDownfall.SneckoDownfallCode.Powers;
using SneckoDownfall.SneckoDownfallCode.Relics;

namespace SneckoDownfall.SneckoDownfallCode.Hooks;

public static class SneckoDownfallHook
{
    public static async Task TriggerAfterCardMuddled(PlayerChoiceContext ctx, CardModel card)
    {
        if (card is SneckoDownfallCard sneckoCard)
            await sneckoCard.AfterMuddled(ctx);

        var powers = card.Owner.Creature.Powers.Where(p => p is SneckoDownfallPower);
        foreach (var power in powers)
            await ((SneckoDownfallPower)power).AfterCardMuddled(ctx, card);

        var relics = card.Owner.Relics.Where(r => r is SneckoDownfallRelic);
        foreach (var relic in relics)
            await ((SneckoDownfallRelic)relic).AfterCardMuddled(ctx, card);
    }
}