using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

public class TrashCanPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeTurnEndVeryEarly(PlayerChoiceContext ctx, CombatSide side)
    {
        if (side != Owner.Side)
            return;

        var selected = await CardSelectCmd.FromHand(ctx, Owner.Player!, new CardSelectorPrefs(SelectionScreenPrompt, 0, Amount), c => true, this);
        foreach (var card in selected)
            await CardCmd.Exhaust(ctx, card);
    }
}
