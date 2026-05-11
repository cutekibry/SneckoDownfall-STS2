using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 SerpentBottle; local QuickBite/SnekBite for draw and selected Muddle.
public class SerpentBottle : SneckoDownfallCard
{
    public SerpentBottle() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(2);
        WithMuddle(1, 1);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CardPileCmd.Draw(ctx, DynamicVars.Cards.IntValue, Owner);
        await SneckoActions.MuddleHand(ctx, this);
    }
}
