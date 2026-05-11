using HarmonyLib;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using SneckoDownfall.Character;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

[Pool(typeof(TokenCardPool))]
public class MockCharacterChoiceCard : SneckoDownfallCard
{
    private CardPoolModel? _mockedPool;

    public override CardPoolModel Pool => _mockedPool ?? base.Pool;

    public MockCharacterChoiceCard() : base(0, CardType.Status, CardRarity.Token, TargetType.None)
    {
        WithKeyword(SneckoDownfallKeyword.Gift);
        WithVar(new StringVar("Characters"));
    }

    public void Mock(CardModel representativeCard)
    {
        _mockedPool = representativeCard.Pool;
    }
}
