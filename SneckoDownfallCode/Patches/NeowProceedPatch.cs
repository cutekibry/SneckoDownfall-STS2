using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using SneckoDownfall.SneckoDownfallCode.Relics;

namespace SneckoDownfall.SneckoDownfallCode.Patches;

[HarmonyPatch(typeof(NEventRoom), nameof(NEventRoom.Proceed))]
public static class NeowProceedPatch
{
    private static bool Prefix(ref Task __result)
    {
        if (RunManager.Instance.DebugOnlyGetState()?.CurrentRoom is not EventRoom { LocalMutableEvent: Neow neow })
        {
            return true;
        }

        var sneckoSoul = neow.Owner?.Relics.OfType<SneckoSoul>().FirstOrDefault();
        if (sneckoSoul == null || sneckoSoul.CharacterIds.Count > 0)
        {
            return true;
        }

        __result = ProceedAfterSneckoSoulInitialized(sneckoSoul);
        return false;
    }

    private static async Task ProceedAfterSneckoSoulInitialized(SneckoSoul sneckoSoul)
    {
        await sneckoSoul.Initialize();
        // NMapScreen.Instance.SetTravelEnabled(enabled: true);
        // NMapScreen.Instance.Open();
    }
}
