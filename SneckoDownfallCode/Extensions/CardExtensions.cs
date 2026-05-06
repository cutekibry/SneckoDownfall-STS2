
using MegaCrit.Sts2.Core.Models;

namespace SneckoDownfall.SneckoDownfallCode.Extensions;

public static class CardExtensions
{
    public static bool CanBeMuddled<TCard>(this TCard card) where TCard : CardModel
    {
        return !card.EnergyCost.CostsX && card.EnergyCost.Canonical >= 0;
    }
}