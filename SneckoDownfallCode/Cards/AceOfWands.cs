using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Powers;
using SneckoDownfall.SneckoDownfallCode.Utils;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

public class AceOfWands : SneckoDownfallCard
{
    public AceOfWands() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<AceOfWandsPower>(4);
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
        WithGift(GiftSelector.IsDebuff);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, DynamicVars["CalculatedHits"].IntValue).Execute(ctx);
    }
}