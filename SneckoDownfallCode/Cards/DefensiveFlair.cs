using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 DefensiveFlair; local Amass for WithCalculatedBlock and CobraCoil for GiftFilter.
public class DefensiveFlair : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Uncommon;

    public DefensiveFlair() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCalculatedBlock(8, 2, GetBonusBlock, ValueProp.Move, 1, 1);
        WithKeyword(SneckoDownfallKeyword.Offclass);
    }
    
    private static decimal GetBonusBlock(CardModel card, Creature? _)
    {
        return PileType.Hand.GetPile(card.Owner).Cards.Count(c => c.IsOffclass());
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.CalculatedBlock, play);
    }
}
