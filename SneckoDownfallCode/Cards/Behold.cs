using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;

namespace SneckoDownfall.SneckoDownfallCode.Cards;


public class Behold : SneckoDownfallCard
{
    protected override bool HasOverflow => true;
    public Behold() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6, 3);
        WithVar("Shivs", 2);
        WithTip(typeof(Shiv));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        if (IsOverflowed)
        {
            for (int i = 0; i < DynamicVars["Shivs"].IntValue; i++)
            {
                await Shiv.CreateInHand(Owner, CombatState!);
                await Cmd.CustomScaledWait(0.1f, 0.2f);
            }
        }
    }
}