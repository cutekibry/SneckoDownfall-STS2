using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 FlashInThePan; STS2 Predator for DrawCardsNextTurnPower and Hermit Roulette for discarding hand.
public class FlashInThePan : SneckoDownfallCard
{
    public FlashInThePan() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(13, 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        var handCards = PileType.Hand.GetPile(Owner).Cards.ToList();
        await CardCmd.Discard(ctx, handCards);
        if (handCards.Count > 0)
            await PowerCmd.Apply<DrawCardsNextTurnPower>(ctx, Owner.Creature, handCards.Count, Owner.Creature, this);
    }
}
