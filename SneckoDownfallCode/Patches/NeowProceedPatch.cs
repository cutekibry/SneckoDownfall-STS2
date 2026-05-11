using HarmonyLib;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Models.Events;
using SneckoDownfall.SneckoDownfallCode.Relics;

namespace SneckoDownfall.SneckoDownfallCode.Patches;

[HarmonyPatch]
public static class NeowProceedPatch
{
    private static readonly AccessTools.FieldRef<NEventRoom, EventModel> EventField =
        AccessTools.FieldRefAccess<NEventRoom, EventModel>("_event");

    private static readonly Dictionary<SneckoSoul, Task> InitializationTasks = [];

    [HarmonyPatch(typeof(NEventRoom), "SetOptions")]
    [HarmonyPostfix]
    private static void StartInitializationAfterNeowButtonsAreReady(EventModel eventModel)
    {
        if (TryGetUninitializedSneckoSoul(eventModel, out var sneckoSoul))
        {
            TaskHelper.RunSafely(GetOrStartInitializationTask(sneckoSoul));
        }
    }

    [HarmonyPatch(typeof(NEventRoom), nameof(NEventRoom.OptionButtonClicked))]
    [HarmonyPrefix]
    private static bool WaitForInitializationBeforeNeowOption(NEventRoom __instance, EventOption option, int index)
    {
        if (!TryGetUninitializedSneckoSoul(option, out var sneckoSoul))
        {
            return true;
        }

        var initializationTask = GetOrStartInitializationTask(sneckoSoul);
        if (initializationTask.IsCompletedSuccessfully)
        {
            return true;
        }

        TaskHelper.RunSafely(ClickAfterSneckoSoulInitialized(__instance, option, index, initializationTask));
        return false;
    }

    private static async Task ClickAfterSneckoSoulInitialized(NEventRoom eventRoom, EventOption option, int index, Task initializationTask)
    {
        await initializationTask;
        eventRoom.OptionButtonClicked(option, index);
    }

    private static bool TryGetUninitializedSneckoSoul(EventOption option, out SneckoSoul sneckoSoul)
    {
        var eventRoom = NEventRoom.Instance;
        if (eventRoom == null || EventField(eventRoom) is not Neow neow || !neow.CurrentOptions.Contains(option))
        {
            sneckoSoul = null!;
            return false;
        }

        return TryGetUninitializedSneckoSoul(neow, out sneckoSoul);
    }

    private static bool TryGetUninitializedSneckoSoul(EventModel eventModel, out SneckoSoul sneckoSoul)
    {
        sneckoSoul = null!;
        if (eventModel is not Neow neow || neow.IsFinished)
        {
            return false;
        }

        sneckoSoul = neow.Owner?.Relics.OfType<SneckoSoul>().FirstOrDefault()!;
        return sneckoSoul != null && sneckoSoul.CharacterIds.Count == 0;
    }

    private static Task GetOrStartInitializationTask(SneckoSoul sneckoSoul)
    {
        if (!InitializationTasks.TryGetValue(sneckoSoul, out var task))
        {
            task = InitializeAndForget(sneckoSoul);
            InitializationTasks[sneckoSoul] = task;
        }

        return task;
    }

    private static async Task InitializeAndForget(SneckoSoul sneckoSoul)
    {
        try
        {
            await sneckoSoul.Initialize();
        }
        finally
        {
            InitializationTasks.Remove(sneckoSoul);
        }
    }
}
