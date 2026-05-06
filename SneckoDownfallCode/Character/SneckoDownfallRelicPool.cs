using BaseLib.Abstracts;
using SneckoDownfall.SneckoDownfallCode.Extensions;
using Godot;

namespace SneckoDownfall.SneckoDownfallCode.Character;

public class SneckoDownfallRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => SneckoDownfall.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}