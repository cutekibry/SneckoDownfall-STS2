using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Utils;

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
        var cards = GiftSelector.GetGiftCards(Owner, c => c.CanBeGeneratedInCombat).ToList()
            .StableShuffle(Owner.RunState.Rng.CombatCardSelection)
            .Take(DynamicVars.Cards.IntValue)
            .Select(c => CombatState!.CreateCard(c, Owner))
            .ToList();
        var selected = await CardSelectCmd.FromChooseACardScreen(ctx, cards, Owner, canSkip: true);
        if (selected == null)
            return;
        selected.EnergyCost.SetThisTurn(0);
        await CardPileCmd.AddGeneratedCardToCombat(selected, PileType.Hand, Owner);
    }
}
