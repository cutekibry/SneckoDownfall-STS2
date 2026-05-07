using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 MarkedCard; local SnekBite/SoulRoll for MuddleHand and STS2 Anointed for upgrade Retain.
public class MarkedCard : SneckoDownfallCard
{
    public MarkedCard() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithMuddle(1);
        WithKeyword(CardKeyword.Exhaust);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithTip(StaticHoverTip.Energy);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await SneckoActions.MuddleHand(ctx, this, cheaperOnly: true);
    }
}
