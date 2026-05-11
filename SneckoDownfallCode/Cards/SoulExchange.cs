using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Actions;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 SoulExchange; local PureSnecko/Restock for Muddle whole hand.
public class SoulExchange : SneckoDownfallCard
{
    public SoulExchange() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Retain, CardKeyword.Exhaust);
        WithKeyword(SneckoDownfall.Character.SneckoDownfallKeyword.Muddle);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await SneckoActions.Muddle(ctx, PileType.Hand.GetPile(Owner).Cards.Where(c => c.CanBeMuddled()));
    }
}
