using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

// Reference: STS1 MudshieldPower; STS2 RagePower/BlunderGuardBlockPower for triggered block gain.
public class MudshieldPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardMuddled(PlayerChoiceContext ctx, CardModel card)
    {
        if(card.Owner == Owner.Player)
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }

}
