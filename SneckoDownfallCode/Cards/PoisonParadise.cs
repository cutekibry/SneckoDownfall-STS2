using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 PoisonParadise.
public class PoisonParadise : SneckoDownfallCard
{
    public PoisonParadise() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<FountainPower>(4, 2, false);
        WithTip(typeof(VenomPower));
        WithKeyword(SneckoDownfallKeyword.Overflow);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<FountainPower>(ctx, Owner.Creature, DynamicVars["FountainPower"].BaseValue, Owner.Creature, this);
    }
}
