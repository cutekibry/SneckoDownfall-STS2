using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Actions;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Powers;


public class CheapStockPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner.Player)
            return;
        
        var candidateCards = PileType.Hand.GetPile(Owner.Player).Cards.Where(c => c.CanBeMuddled());
        var sortedCandidateCards = candidateCards.ToList().StableShuffle(Owner.Player.RunState.Rng.Shuffle).OrderBy(c => c.EnergyCost.GetResolved());

        var cardsToMuddle = sortedCandidateCards.Take(Amount);
        await SneckoActions.Muddle(cardsToMuddle);
    }
}