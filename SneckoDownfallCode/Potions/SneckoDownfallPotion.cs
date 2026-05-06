using BaseLib.Abstracts;
using BaseLib.Utils;
using SneckoDownfall.SneckoDownfallCode.Character;

namespace SneckoDownfall.SneckoDownfallCode.Potions;

[Pool(typeof(SneckoDownfallPotionPool))]
public abstract class SneckoDownfallPotion : CustomPotionModel;