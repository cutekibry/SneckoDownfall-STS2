using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 DiceBlock; local Behold/TailWhip for HasOverflow and IsOverflowed.
public class DiceBlock : SneckoDownfallCard
{
    protected override bool HasOverflow => true;

    public DiceBlock() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(5, 2);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        if (IsOverflowed)
            await CommonActions.CardBlock(this, play);
    }
}
