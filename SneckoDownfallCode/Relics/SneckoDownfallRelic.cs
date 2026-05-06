using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using SneckoDownfall.SneckoDownfallCode.Character;
using SneckoDownfall.SneckoDownfallCode.Extensions;
using Godot;

namespace SneckoDownfall.SneckoDownfallCode.Relics;

[Pool(typeof(SneckoDownfallRelicPool))]
public abstract class SneckoDownfallRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}