using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;


public class CheapStock : SneckoDownfallCard
{
    public CheapStock() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<CheapStockPower>(1, 1, false);
        WithKeyword(SneckoDownfallKeyword.Muddle);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<CheapStockPower>(ctx, Owner.Creature, DynamicVars["CheapStockPower"].BaseValue, Owner.Creature, this);
    }
}