using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Serpentscale; STS2 StoneArmor/PlatingPower for Plated Armor.
public class Serpentscale : SneckoDownfallCard
{
    protected override bool HasOverflow => true;

    public Serpentscale() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9, 3);
        WithPower<PlatingPower>(3, 1, true);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        await PowerCmd.Apply<PlatingPower>(ctx, Owner.Creature, IsOverflowed ? DynamicVars["PlatingPower"].BaseValue : 1, Owner.Creature, this);
    }
}
