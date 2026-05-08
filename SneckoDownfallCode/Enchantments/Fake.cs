using HarmonyLib;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace SneckoDownfall.SneckoDownfallCode.Enchantments;

public class Fake : SneckoDownfallEnchantment
{
    public override bool HasExtraCardText => true;

    public override bool ShowAmount => false;

    public CardPoolModel? OriginalPool { get; private set; } = null;
    public CardPoolModel? NewPool { get; private set; } = null;


    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Character"), new StringVar("PoolId"), new StringVar("OldPoolId")];

    public void SetCharacter(CharacterModel character) {
        OriginalPool = Card.Pool;
        ((StringVar) DynamicVars["Character"]).StringValue = character.Title.GetFormattedText();
        NewPool = character.CardPool;
    }
    [HarmonyPatch(typeof(CardModel), "Pool", MethodType.Getter)]
    private static class PoolPatch
    {
        private static void Postfix(CardModel __instance, ref CardPoolModel? __result)
        {
            if (__instance.Enchantment is not Fake { NewPool: not null } fake)
                return;

            __result = fake.NewPool;
        }
    }

    [HarmonyPatch(typeof(CardFactory), nameof(CardFactory.GetDefaultTransformationOptions))]
    private static class GetDefaultTransformationOptionsPatch
    {
        private static bool Prefix(CardModel original, bool isInCombat, ref IEnumerable<CardModel> __result)
        {
            if (original.Enchantment is not Fake { NewPool: not null } fake)
                return true;

            IEnumerable<CardModel> cards = fake.NewPool.GetUnlockedCards(
                original.Owner.UnlockState,
                original.RunState!.CardMultiplayerConstraint
            );

            __result = (IEnumerable<CardModel>)AccessTools.Method(typeof(CardFactory), "GetFilteredTransformationOptions")!
                .Invoke(null, [original, cards, isInCombat])!;
            return false;
        }
    }

    [HarmonyPatch(typeof(CardModel), "PortraitPath", MethodType.Getter)]
    private static class PortraitPathPatch
    {
        private static void Postfix(CardModel __instance, ref string __result)
        {
            if (TryGetOriginalPortraitPath(__instance, beta: false, png: false, out string path))
                __result = path;
        }
    }

    [HarmonyPatch(typeof(CardModel), "BetaPortraitPath", MethodType.Getter)]
    private static class BetaPortraitPathPatch
    {
        private static void Postfix(CardModel __instance, ref string __result)
        {
            if (TryGetOriginalPortraitPath(__instance, beta: true, png: false, out string path))
                __result = path;
        }
    }

    [HarmonyPatch(typeof(CardModel), "PortraitPngPath", MethodType.Getter)]
    private static class PortraitPngPathPatch
    {
        private static void Postfix(CardModel __instance, ref string __result)
        {
            if (TryGetOriginalPortraitPath(__instance, beta: false, png: true, out string path))
                __result = path;
        }
    }

    [HarmonyPatch(typeof(CardModel), "BetaPortraitPngPath", MethodType.Getter)]
    private static class BetaPortraitPngPathPatch
    {
        private static void Postfix(CardModel __instance, ref string __result)
        {
            if (TryGetOriginalPortraitPath(__instance, beta: true, png: true, out string path))
                __result = path;
        }
    }

    private static bool TryGetOriginalPortraitPath(CardModel card, bool beta, bool png, out string path)
    {
        path = string.Empty;

        if (card.Enchantment is not Fake { OriginalPool: not null } fake)
            return false;

        string poolTitle = fake.OriginalPool.Title.ToLowerInvariant();
        string cardId = card.Id.Entry.ToLowerInvariant();
        string betaSegment = beta ? "/beta" : "";
        string extension = png ? "png" : "tres";
        string root = png ? "packed/card_portraits" : "atlases/card_atlas.sprites";

        path = ImageHelper.GetImagePath($"{root}/{poolTitle}{betaSegment}/{cardId}.{extension}");
        return true;
    }
}
