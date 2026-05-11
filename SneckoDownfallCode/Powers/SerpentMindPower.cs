using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

// Reference: STS1 SerpentMindPower; STS2 DemonFormPower for end-turn Strength gain.
public class SerpentMindPower : SneckoDownfallPower
{
    private class Data
    {
        public int differentColorsPlayed;
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => GetInternalData<Data>().differentColorsPlayed;
    protected override object InitInternalData()
    {
        return new Data();
    }
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        UpdateDifferentColorsPlayed();
        return Task.CompletedTask;
    }
    public override Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        UpdateDifferentColorsPlayed();
        return Task.CompletedTask;
    }
    public override Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side == Owner.Side)
            SetDifferentColorsPlayed(0);
        return Task.CompletedTask;
    }

    public override async Task BeforeTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
            return;

        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount * GetInternalData<Data>().differentColorsPlayed, Owner, null);
    }
    private bool UpdateDifferentColorsPlayed()
    {
        return SetDifferentColorsPlayed(CombatManager.Instance.History.CardPlaysStarted
            .Where(e => e.HappenedThisTurn(CombatState) && e.CardPlay.Card.Owner.Creature == Owner)
            .Select(e => e.CardPlay.Card.Pool.ColorId())
            .Distinct()
            .Count()
        );
    }
    private bool SetDifferentColorsPlayed(int value)
    {
        if (GetInternalData<Data>().differentColorsPlayed == value)
            return false;

        GetInternalData<Data>().differentColorsPlayed = value;
        InvokeDisplayAmountChanged();
        return true;
    }
}
