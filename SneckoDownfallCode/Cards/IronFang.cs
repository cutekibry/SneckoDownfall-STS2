using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 IronFang.
public class IronFang : SneckoDownfallCard
{
    public IronFang() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithDamage(6, 3);
        WithPower<WeakPower>(1, true);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);

        foreach (var enemy in CombatState!.HittableEnemies)
            await PowerCmd.Apply<WeakPower>(ctx, enemy, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this);
    }
}
