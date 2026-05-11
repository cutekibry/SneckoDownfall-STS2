using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

// Reference: STS1 SaveForLater/CoalescencePower; STS2 WellLaidPlansPower for choosing cards before hand flush.
public class SaveForLaterPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeFlushLate(PlayerChoiceContext ctx, Player player)
    {
        var combatState = player.Creature.CombatState;
        if (player != Owner.Player || combatState == null || !Hook.ShouldFlush(combatState, player))
            return;

        var selected = await CardSelectCmd.FromHand(
            ctx,
            player,
            new CardSelectorPrefs(SelectionScreenPrompt, 0, Amount),
            c => !c.ShouldRetainThisTurn,
            this);

        foreach (var card in selected)
            card.GiveSingleTurnRetain();
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext ctx, MegaCrit.Sts2.Core.Combat.CombatSide side)
    {
        if (side == Owner.Side)
            await PowerCmd.Remove(this);
    }
}
