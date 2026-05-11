using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

// Reference: STS1 SnakeEyesPower; STS2 BurstPower for ModifyCardPlayCount.
public class SnakeEyesPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != Owner || !card.IsOffclass())
            return playCount;
        return playCount + 1;
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        if (card.Owner.Creature == Owner && card.IsOffclass())
        {
            Flash();
            await PowerCmd.Decrement(this);
        }
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
            await PowerCmd.Remove(this);
    }
}
