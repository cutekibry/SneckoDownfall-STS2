using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;


public class BlunderGuard : SneckoDownfallCard
{
    public BlunderGuard() : base(0, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<BlunderGuardBlockPower>(6, 2, false);
        WithPower<BlunderGuardStrengthPower>(2, 1, false);
        WithEnergy(3);
        WithTip(typeof(StrengthPower));
        WithTip(StaticHoverTip.Block);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<BlunderGuardBlockPower>(ctx, Owner.Creature, DynamicVars["BlunderGuardBlockPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<BlunderGuardStrengthPower>(ctx, Owner.Creature, DynamicVars["BlunderGuardStrengthPower"].BaseValue, Owner.Creature, this);
    }
}