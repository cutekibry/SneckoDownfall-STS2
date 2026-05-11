using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 SerpentineSleuth; STS2 StormPower for power-card trigger.
public class SerpentineSleuth : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Type == CardType.Power && c.Rarity == CardRarity.Rare;

    public SerpentineSleuth() : base(4, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<SubroutinePower>(1, false);
        WithKeyword(CardKeyword.Ethereal);
        WithTip(StaticHoverTip.Energy);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<SubroutinePower>(ctx, Owner.Creature, DynamicVars["SubroutinePower"].BaseValue, Owner.Creature, this);
    }
}
