using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Deception; STS2 Shockwave plus Begone/CardPileCmd for generated-card creation.
public class Deception : SneckoDownfallCard
{
    public Deception() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(11, 4);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(typeof(Shockwave));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CardPileCmd.AddGeneratedCardToCombat(CombatState!.CreateCard<Shockwave>(Owner), PileType.Hand, Owner);
    }
}
