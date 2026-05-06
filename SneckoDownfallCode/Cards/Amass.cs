using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace SneckoDownfall.SneckoDownfallCode.Cards;


public class Amass : SneckoDownfallCard
{
    public Amass() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVar("Ratio", 1, 2);
        WithCalculatedBlock(9, GetBonusBlock);
    }
    private static decimal GetBonusBlock(CardModel card, Creature? _)
    {
        return card.DynamicVars["Ratio"].IntValue * PileType.Hand.GetPile(card.Owner).Cards.Where(c => c != card).Sum(c => Math.Max(0m, c.EnergyCost.Canonical));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.CalculatedBlock, play);
    }
}