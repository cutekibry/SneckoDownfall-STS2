using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

public class Tsunami : SneckoDownfallCard
{
    public Tsunami() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<TsunamiPower>(4, 1, false);
        WithTip(StaticHoverTip.Block);
        WithKeyword(SneckoDownfallKeyword.Overflow);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<TsunamiPower>(ctx, Owner.Creature, DynamicVars["TsunamiPower"].BaseValue, Owner.Creature, this);
    }
}
