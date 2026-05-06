using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Powers;


public class BlunderGuardStrengthPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3)];

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Owner.Creature == Owner && play.Card.EnergyCost.GetResolved() >= DynamicVars.Energy.IntValue)
        {
            Flash();
            await PowerCmd.Apply<StrengthPower>(ctx, Owner, Amount, Owner, null);
        }
    }
}