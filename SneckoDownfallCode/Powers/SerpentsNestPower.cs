using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

// Reference: STS1 SerpentsNestPower; STS2 StormPower for power-card trigger timing.
public class SerpentsNestPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private bool IsFirstTrigger = true;

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Owner.Creature != Owner || play.Card.Type != CardType.Power)
            return;
        
        if (IsFirstTrigger)
        {
            // Skip the first trigger from the card application itself.
            IsFirstTrigger = false;
            return;
        }

        Flash();
        await CreatureCmd.Damage(ctx, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
    }
}
