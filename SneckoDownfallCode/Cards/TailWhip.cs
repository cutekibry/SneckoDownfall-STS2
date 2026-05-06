using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using SneckoDownfall.Character;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

public class TailWhip : SneckoDownfallCard
{
    protected override bool HasOverflow => true;
    public TailWhip() : base(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(10, 1);
        WithPower<WeakPower>(1, 1);
        WithPower<VulnerablePower>(1, 1);
        WithKeyword(SneckoDownfallKeyword.Overflow);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        if(IsOverflowed)
        {
            await CommonActions.Apply<WeakPower>(ctx, play.Target!, this);
            await CommonActions.Apply<VulnerablePower>(ctx, play.Target!, this);
        }
    }
}