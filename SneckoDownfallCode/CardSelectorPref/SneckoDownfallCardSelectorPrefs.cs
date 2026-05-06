using MegaCrit.Sts2.Core.Localization;

namespace SneckoDownfall.SneckoDownfallCode.CardSelectorPref;

public readonly struct SneckoDownfallCardSelectorPrefs
{
    public static LocString MuddleSelectionPrompt => new("card_selection", "TO_MUDDLE"); public LocString Prompt { get; }
}