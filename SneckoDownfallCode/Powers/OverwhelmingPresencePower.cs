using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

// Reference: STS1 OverwhelmingPresencePower.
public class OverwhelmingPresencePower : SneckoDownfallPower
{
    private class Data
    {
        public int offclassCardPlayed;
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => Math.Max(0, Amount - GetInternalData<Data>().offclassCardPlayed);
    protected override object InitInternalData()
    {
        return new Data();
    }
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        SetOffclassCardPlayed(CombatManager.Instance.History.Entries.OfType<CardPlayStartedEntry>().Count(e => e.CardPlay.Card.IsOffclass() && e.CardPlay.Card.Owner.Creature == Owner && e.HappenedThisTurn(CombatState)));
        return Task.CompletedTask;
    }

    public override Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side == Owner.Side)
            SetOffclassCardPlayed(0);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.IsOffclass() && play.Card.Owner.Creature == Owner && GetInternalData<Data>().offclassCardPlayed < Amount) {
            Flash();
            SetOffclassCardPlayed(GetInternalData<Data>().offclassCardPlayed + 1);
            await CardPileCmd.Draw(ctx, Owner.Player!);
        }
    }
    private void SetOffclassCardPlayed(int value)
    {
        GetInternalData<Data>().offclassCardPlayed = value;
        InvokeDisplayAmountChanged();
    }
}
