using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 SerpentIdol; STS2 Toolbox/Splash for choose-one generated card.
public class SerpentIdol : SneckoDownfallCard
{
    public SerpentIdol() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(3);
        WithKeyword(CardKeyword.Exhaust);
        WithKeyword(SneckoDownfall.Character.SneckoDownfallKeyword.Offclass);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        var cards = SneckoActions.GenerateRandomOffclassCards(Owner, DynamicVars.Cards.IntValue).ToList();
        var selected = await CardSelectCmd.FromChooseACardScreen(ctx, cards, Owner, canSkip: true);
        if (selected == null)
            return;
        selected.EnergyCost.SetThisTurn(0);
        await CardPileCmd.AddGeneratedCardToCombat(selected, PileType.Hand, Owner);
    }
}
