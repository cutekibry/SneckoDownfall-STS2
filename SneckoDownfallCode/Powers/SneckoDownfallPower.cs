using BaseLib.Abstracts;
using BaseLib.Extensions;
using SneckoDownfall.SneckoDownfallCode.Extensions;
using Godot;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

public abstract class SneckoDownfallPower : CustomPowerModel
{
    //Loads from SneckoDownfall/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}