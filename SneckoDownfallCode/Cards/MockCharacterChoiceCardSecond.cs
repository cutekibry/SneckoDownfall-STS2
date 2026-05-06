using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

[Pool(typeof(TokenCardPool))]
public class MockCharacterChoiceCardSecond : SneckoDownfallCard
{
    // public string CopiedPortraitPath = "";
    // public string CopiedBetaPortraitPath = "";
    // public override string PortraitPath => CopiedPortraitPath;
    // public override string BetaPortraitPath => CopiedBetaPortraitPath;

    public MockCharacterChoiceCardSecond() : base(0, CardType.Status, CardRarity.Token, TargetType.None)
    {
        WithVar(new StringVar("Characters"));
    }
}