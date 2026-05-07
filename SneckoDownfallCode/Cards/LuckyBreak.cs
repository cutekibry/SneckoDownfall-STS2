using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 LuckyBreak; local Blunderbus for high-cost hand counting.
public class LuckyBreak : SneckoDownfallCard
{
    protected override bool ShouldGlowGoldInternal => GetDrawCount(this, Owner.Creature) > 0;

    public LuckyBreak() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(8, 3);
        WithEnergy(2);
        WithCalculatedVar("CardsToDraw", 0, GetDrawCount);
    }

    private static decimal GetDrawCount(CardModel card, Creature? _)
    {
        var hand = PileType.Hand.GetPile(card.Owner).Cards;
        return hand.Count(c => c != card && c.EnergyCost.GetResolved() >= card.DynamicVars.Energy.IntValue);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);;
        await CardPileCmd.Draw(ctx, GetCalculatedValue("CardsToDraw", play), Owner);
    }
}
