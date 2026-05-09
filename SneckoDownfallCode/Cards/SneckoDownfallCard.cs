using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using SneckoDownfall.SneckoDownfallCode.Character;
using SneckoDownfall.SneckoDownfallCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SneckoDownfall.Character;
using MegaCrit.Sts2.Core.Models;
using HarmonyLib;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

[Pool(typeof(SneckoDownfallCardPool))]
public abstract class SneckoDownfallCard :
    ConstructedCardModel
{
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected virtual bool HasOverflow => false;
    private bool? _cachedIsOverflowed = null;
    protected override bool ShouldGlowGoldInternal => IsOverflowed;
    public bool IsOverflowed => _cachedIsOverflowed ?? (HasOverflow && PileType.Hand.GetPile(Owner).Cards.Count > 5);

    public SneckoDownfallCard(int cost, CardType type, CardRarity rarity, TargetType target) : base(cost, type, rarity, target)
    {
        if (HasOverflow)
            WithKeyword(SneckoDownfallKeyword.Overflow);
        if (GiftFilter != null)
            WithKeyword(SneckoDownfallKeyword.Gift);
    }

    public void CacheIsOverflowed()
    {
        _cachedIsOverflowed = HasOverflow && PileType.Hand.GetPile(Owner).Cards.Count > 5;
    }
    public void ClearIsOverflowedCache()
    {
        _cachedIsOverflowed = null;
    }

    public virtual Func<CardModel, bool>? GiftFilter => null;

    protected ConstructedCardModel WithMuddle(int baseVal, int upgrade = 0)
    {
        WithKeyword(SneckoDownfallKeyword.Muddle);
        WithVar(new DynamicVar("Muddle", baseVal).WithUpgrade(upgrade));
        return this;
    }

    protected ConstructedCardModel WithPower<T>(int baseVal, int upgrade, bool hasTooltip) where T : PowerModel
    {
        WithVar(new DynamicVar(typeof(T).Name, baseVal).WithUpgrade(upgrade));
        if (hasTooltip)
            WithTip(typeof(T));
        return this;
    }
    protected ConstructedCardModel WithPower<T>(int baseVal, bool hasTooltip) where T : PowerModel
    {
        return WithPower<T>(baseVal, 0, hasTooltip);
    }

    protected decimal GetCalculatedValue(string varName, CardPlay play)
    {
        return ((CalculatedVar)DynamicVars[varName]).Calculate(play.Target);
    }


    [HarmonyPatch(typeof(CardModel), nameof(OnPlayWrapper))]
    public static class OverflowCapturePatch
    {
        [HarmonyPrefix]
        public static void Prefix(CardModel __instance)
        {
            if (__instance is SneckoDownfallCard sneckoCard)
                sneckoCard.CacheIsOverflowed();
        }

        [HarmonyPostfix]
        public static void Postfix(CardModel __instance, ref Task __result)
        {
            if (__instance is SneckoDownfallCard)
                __result = ClearOverflowCaptureAfterPlay(__instance, __result);
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
}