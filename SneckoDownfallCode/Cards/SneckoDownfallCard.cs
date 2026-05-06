using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using SneckoDownfall.SneckoDownfallCode.Character;
using SneckoDownfall.SneckoDownfallCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SneckoDownfall.Character;
using MegaCrit.Sts2.Core.Models;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

[Pool(typeof(SneckoDownfallCardPool))]
public abstract class SneckoDownfallCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ConstructedCardModel(cost, type, rarity, target)
{
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected virtual bool HasOverflow => false;
    private bool? _cachedIsOverflowed = null;
    protected override bool ShouldGlowGoldInternal => HasOverflow && IsOverflowed;
    public bool IsOverflowed => _cachedIsOverflowed ?? PileType.Hand.GetPile(Owner).Cards.Count >= 5;

    public void CacheIsOverflowed()
    {
        _cachedIsOverflowed = PileType.Hand.GetPile(Owner).Cards.Count >= 5;
    }
    public void ClearIsOverflowedCache()
    {
        _cachedIsOverflowed = null;
    }

    public Func<CardModel, bool>? GiftFilter { get; protected set; } = null;

    protected ConstructedCardModel WithMuddle(int baseVal, int upgrade = 0)
    {
        WithKeyword(SneckoDownfallKeyword.Muddle);
        WithVar(new DynamicVar("Muddle", baseVal).WithUpgrade(upgrade));
        return this;
    }

    protected ConstructedCardModel WithGift(Func<CardModel, bool> filter)
    {
        WithKeyword(SneckoDownfallKeyword.Gift);
        GiftFilter = filter;
        return this;
    }
}