using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Mesmerize; local SnekBite for MuddleHand and STS2 Shockwave for all-enemy debuffs.
public class Mesmerize : SneckoDownfallCard
{
    public Mesmerize() : base(3, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithPower<StrengthPower>(2, 1, true);
        WithMuddle(1);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
            await PowerCmd.Apply<StrengthPower>(ctx, enemy, -DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        await SneckoActions.MuddleHand(ctx, this);
    }
}
