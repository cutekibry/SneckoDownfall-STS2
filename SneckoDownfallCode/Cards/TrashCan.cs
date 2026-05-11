using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

public class TrashCan : SneckoDownfallCard
{
    public TrashCan() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<TrashCanPower>(1, false);
        WithTip(CardKeyword.Exhaust);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<TrashCanPower>(ctx, Owner.Creature, DynamicVars["TrashCanPower"].BaseValue, Owner.Creature, this);
    }
}
