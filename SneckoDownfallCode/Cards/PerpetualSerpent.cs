using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 PerpetualSerpent.
public class PerpetualSerpent : SneckoDownfallCard
{
    protected override bool HasOverflow => true;

    public PerpetualSerpent() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(20, 5);
        WithEnergy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        if (IsOverflowed)
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }
}
