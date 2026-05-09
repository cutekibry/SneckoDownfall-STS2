using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Cards;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

// Reference: STS1 FountainPower; STS2 hooks through SneckoActions.Muddle.
public class FountainPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Owner != Owner.Player || play.Card is not SneckoDownfallCard sneckoCard || !sneckoCard.IsOverflowed)
            return;

        Flash();
        var enemy = Owner.Player.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        if (enemy != null)
        {
            await PowerCmd.Apply<VenomPower>(ctx, enemy, Amount, Owner, null);
        }
    }
}
