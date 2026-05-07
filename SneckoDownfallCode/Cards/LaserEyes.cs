using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 LaserEyes/SnekBeam; STS2 Sunder/Adrenaline for attack plus energy gain.
public class LaserEyes : SneckoDownfallCard
{
    public LaserEyes() : base(3, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(15, 5);
        WithEnergy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }
}
