using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Enchantments;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

public class TyphoonFang : SneckoDownfallCard
{
    protected override bool HasOverflow => true;

    public TyphoonFang() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(12, 4);
        WithKeyword(SneckoDownfallKeyword.Overflow);
        WithTip(typeof(Fake));
        WithTips(c => [HoverTipFactory.FromCard<MinionDiveBomb>(c.IsUpgraded)]);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        if (IsOverflowed)
        {
            if (IsUpgraded)
                await PowerCmd.Apply<TyphoonPlusPower>(ctx, Owner.Creature, 1, Owner.Creature, this);
            else
                await PowerCmd.Apply<TyphoonPower>(ctx, Owner.Creature, 1, Owner.Creature, this);
        }
    }
}
