using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 GildedShield; STS2 ParticleWall for returning the played card to hand.
public class GildedShield : SneckoDownfallCard
{
    public GildedShield() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(8, 3);
        WithMuddle(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await SneckoActions.Muddle(this);
    }

    protected override PileType GetResultPileType()
    {
        var resultPileType = base.GetResultPileType();
        return resultPileType == PileType.Discard ? PileType.Hand : resultPileType;
    }
}
