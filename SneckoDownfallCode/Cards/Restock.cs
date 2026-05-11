using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Actions;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Restock; local FlashInThePan for discarding hand and PureSnecko for draw-then-Muddle.
public class Restock : SneckoDownfallCard
{
    public Restock() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(6);
        WithKeyword(CardKeyword.Exhaust);
        WithKeyword(SneckoDownfall.Character.SneckoDownfallKeyword.Muddle);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CardCmd.Discard(ctx, PileType.Hand.GetPile(Owner).Cards);
        await CardPileCmd.Draw(ctx, DynamicVars.Cards.IntValue, Owner);
        await SneckoActions.Muddle(ctx, PileType.Hand.GetPile(Owner).Cards.Where(c => c.CanBeMuddled()));
    }
}
