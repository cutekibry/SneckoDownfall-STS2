using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.Character;

namespace SneckoDownfall.SneckoDownfallCode.Cards;


public class BeyondArmor : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Common;
    public BeyondArmor() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(5, 3);
        WithCards(2);
        WithKeyword(SneckoDownfallKeyword.Offclass);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play); 
        var cards = PileType.Hand.GetPile(Owner).Cards.Where(c => c.Pool != Owner.Character.CardPool).ToList().StableShuffle(Owner.RunState.Rng.Shuffle);
        await CardPileCmd.Add(cards, PileType.Hand);
    }
}