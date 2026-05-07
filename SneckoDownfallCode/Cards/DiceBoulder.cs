using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 DiceBoulder; STS2 Claw/Rampage for self-modifying values and UpMySleeve for AddThisCombat.
public class DiceBoulder : SneckoDownfallCard
{
    public DiceBoulder() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(7, 1);
        WithVar("Increase", 4, 1);
        WithEnergy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        DynamicVars.Block.BaseValue += DynamicVars["Increase"].IntValue;
        EnergyCost.AddThisCombat(DynamicVars.Energy.IntValue);
    }
}
