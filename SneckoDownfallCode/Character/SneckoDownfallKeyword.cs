using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace SneckoDownfall.Character;

public static class SneckoDownfallKeyword
{
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Overflow;

    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Muddle;

    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Offclass;

    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Gift;
}