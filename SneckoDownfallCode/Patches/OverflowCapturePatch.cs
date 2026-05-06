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
}