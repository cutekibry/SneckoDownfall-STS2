using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Cower; local HoleUp plus CardPileCmd.AddGeneratedCardToCombat for generated-card creation.
public class Cower : SneckoDownfallCard
{
    public Cower() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(14, 4);
        WithKeyword(CardKeyword.Exhaust);
        WithTips(card => [HoverTipFactory.FromCard<HoleUp>(card.IsUpgraded)]);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        var card = CombatState!.CreateCard<HoleUp>(Owner);
        if (IsUpgraded && card.IsUpgradable)
            CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}
