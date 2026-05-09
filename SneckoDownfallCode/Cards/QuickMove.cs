using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 QuickMove.
public class QuickMove : SneckoDownfallCard
{
    protected override bool HasOverflow => true;

    public QuickMove() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(7, 3);
        WithPower<VulnerablePower>(1, true);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        if (IsOverflowed)
        {
            foreach (var enemy in CombatState!.HittableEnemies)
                await CommonActions.Apply<VulnerablePower>(ctx, enemy, this);
        }
    }
}
