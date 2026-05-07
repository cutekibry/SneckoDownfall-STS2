using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 DragonsHoard; STS2 Prowess for Strength/Dexterity and local AceOfWands for Ethereal upgrade removal.
public class DragonsHoard : SneckoDownfallCard
{
    public DragonsHoard() : base(3, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<StrengthPower>(3, false);
        WithPower<DexterityPower>(3, false);
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
        WithTip(typeof(StrengthPower));
        WithTip(typeof(DexterityPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<StrengthPower>(ctx, Owner.Creature, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(ctx, Owner.Creature, DynamicVars["DexterityPower"].BaseValue, Owner.Creature, this);
    }
}
