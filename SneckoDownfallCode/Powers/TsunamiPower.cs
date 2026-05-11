using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using SneckoDownfall.SneckoDownfallCode.Cards;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

public class TsunamiPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Owner != Owner.Player || play.Card is not SneckoDownfallCard sneckoCard || !sneckoCard.IsOverflowed)
            return;

        Flash();
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }
}
