using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 LatchOn; local Behold and ViperEssence for generated-card creation.
public class LatchOn : SneckoDownfallCard
{
    public LatchOn() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(7, 3);
        WithTip(typeof(ViperEssence));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        await CardPileCmd.AddGeneratedCardToCombat(CombatState!.CreateCard<ViperEssence>(Owner), PileType.Hand, Owner);
    }
}
