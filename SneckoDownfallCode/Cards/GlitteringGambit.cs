using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 GlitteringGambit; SneckoSoul handles Gift rewards when this enters the deck.
public class GlitteringGambit : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Rare;

    public GlitteringGambit() : base(-2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar(new GoldVar(150));
        WithKeyword(CardKeyword.Unplayable);
        WithKeyword(CardKeyword.Eternal);
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Add);
    }

    protected override Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        return Task.CompletedTask;
    }

    public override async Task BeforeCardRemoved(CardModel card)
    {
        if (card == this)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<CurseOfTheBell>(Owner), PileType.Deck));
    }
}
