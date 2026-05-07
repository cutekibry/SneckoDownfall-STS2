using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 InertBlade; effects depend on energy actually spent for this play.
public class InertBlade : SneckoDownfallCard
{
    public InertBlade() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(10, 3);
        WithCards(3, 1);
        WithPower<StrengthPower>(3, 1);
        WithEnergy(1, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);

        var energySpent = EnergyCost.GetResolved();
        if (energySpent >= 1)
            await CardPileCmd.Draw(ctx, DynamicVars.Cards.BaseValue, Owner);
        if (energySpent >= 2)
            await PowerCmd.Apply<StrengthPower>(ctx, Owner.Creature, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        if (energySpent >= 3)
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}
