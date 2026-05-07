using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 DiceCrush; local Behold/TailWhip for Overflow and STS2 ShrugItOff for CardPileCmd.Draw.
public class DiceCrush : SneckoDownfallCard
{
    protected override bool HasOverflow => true;

    public DiceCrush() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(18, 4);
        WithCards(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        if (IsOverflowed)
            await CardPileCmd.Draw(ctx, DynamicVars.Cards.BaseValue, Owner);
    }
}
