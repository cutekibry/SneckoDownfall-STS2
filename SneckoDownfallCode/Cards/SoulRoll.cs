using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;


[Pool(typeof(TokenCardPool))]
public class SoulRoll : SneckoDownfallCard
{
    public SoulRoll() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithBlock(3, 3);
        WithMuddle(1);
        WithKeywords(CardKeyword.Retain, CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await SneckoActions.MuddleHand(ctx, this);
    }
}