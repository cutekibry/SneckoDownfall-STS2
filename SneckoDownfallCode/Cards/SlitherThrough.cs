using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 SlitherThrough; STS2 Stomp/Pinpoint for temporary cost reduction.
public class SlitherThrough : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Uncommon;

    public SlitherThrough() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(14, 4);
        WithEnergy(1);
        WithKeyword(SneckoDownfall.Character.SneckoDownfallKeyword.Offclass);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        foreach (var card in PileType.Hand.GetPile(Owner).Cards)
            if (card.IsOffclass() && card.CanBeMuddled())
                card.EnergyCost.AddThisTurn(-DynamicVars.Energy.IntValue, reduceOnly: true);
    }
}
