using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.Character;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 RiskySword; local DiceBoulder for combat self-scaling values.
public class RiskySword : SneckoDownfallCard
{
    public RiskySword() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(8, 2);
        WithVar("Increase", 8, 2);
        WithKeyword(SneckoDownfallKeyword.Muddle);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
    }

    public override Task AfterMuddled(PlayerChoiceContext choiceContext)
    {
        DynamicVars.Damage.BaseValue += DynamicVars["Increase"].IntValue;
        return Task.CompletedTask;
    }
}
