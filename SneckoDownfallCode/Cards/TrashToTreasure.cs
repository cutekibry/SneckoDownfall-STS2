using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 TrashToTreasure; STS2 Recycle behavior represented as select/exhaust/gain Energy.
public class TrashToTreasure : SneckoDownfallCard
{
    public TrashToTreasure() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithBlock(9);
        WithTip(CardKeyword.Exhaust);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        var selected = (await CardSelectCmd.FromHand(ctx, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), c => c != this, this)).FirstOrDefault();
        if (selected == null)
            return;

        var energy =
            selected.EnergyCost.CostsX
            ?
            Owner.PlayerCombatState!.Energy
            :
            Math.Max(0, selected.EnergyCost.GetResolved());
        await CardCmd.Exhaust(ctx, selected);
        if (energy > 0)
            await PlayerCmd.GainEnergy(energy, Owner);
    }

}
