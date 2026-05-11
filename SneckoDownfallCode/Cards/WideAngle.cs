using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 WideAngle; local IronFang for all-enemy attacks.
public class WideAngle : SneckoDownfallCard
{
    public WideAngle() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(18, 4);
        WithKeyword(CardKeyword.Retain);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
    }
}
