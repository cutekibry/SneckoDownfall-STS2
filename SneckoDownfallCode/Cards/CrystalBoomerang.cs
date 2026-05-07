using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 CrystalBoomerang; STS2 Hologram/Dredge for discard-pile selection into hand.
public class CrystalBoomerang : SneckoDownfallCard
{
    public CrystalBoomerang() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(5, 3);
        WithKeyword(SneckoDownfallKeyword.Offclass);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        var selected = (await CardSelectCmd.FromSimpleGrid(ctx, PileType.Discard.GetPile(Owner).Cards, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1))).FirstOrDefault();
        if (selected == null)
            return;

        bool offclass = selected.IsOffclass();
        await CardPileCmd.Add(selected, PileType.Hand);
        if (offclass)
            await CommonActions.CardBlock(this, play);
    }
}
