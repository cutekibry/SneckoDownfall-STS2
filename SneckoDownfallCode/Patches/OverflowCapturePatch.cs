using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Cards;

namespace SneckoDownfall.SneckoDownfallCode.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
public static class OverflowCapturePatch
{
    [HarmonyPrefix]
    public static void Prefix(CardModel __instance)
    {
        if (__instance is SneckoDownfallCard sneckoCard)
        {
            Log.Info($"Card is a SneckoDownfallCard, capturing overflow for play");
            sneckoCard.CacheIsOverflowed();
        }
    }

    [HarmonyPostfix]
    public static void Postfix(CardModel __instance, ref Task __result)
    {
        if (__instance is SneckoDownfallCard)
        {
            __result = ClearOverflowCaptureAfterPlay(__instance, __result);
        }
    }

    private static async Task ClearOverflowCaptureAfterPlay(CardModel card, Task original)
    {
        try
        {
            await original;
        }
        finally
        {
            if (card is SneckoDownfallCard sneckoCard)
            {
                sneckoCard.ClearIsOverflowedCache();
            }
        }
    }
}
