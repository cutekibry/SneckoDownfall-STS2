using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Crippling Poison behavior supplied by user; local HoleUp for TokenCardPool pattern.
[Pool(typeof(TokenCardPool))]
public class CripplingPoison : SneckoDownfallCard
{
    public CripplingPoison() : base(2, CardType.Skill, CardRarity.Token, TargetType.AllEnemies)
    {
        WithPower<PoisonPower>(4, 3, true);
        WithPower<WeakPower>(2, true);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await PowerCmd.Apply<PoisonPower>(ctx, enemy, DynamicVars["PoisonPower"].BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<WeakPower>(ctx, enemy, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this);
        }
    }
}
