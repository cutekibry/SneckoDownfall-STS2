using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Nope; STS2 ports class-color lookup through unlocked characters' card pools.
public class Nope : SneckoDownfallCard
{
    public Nope() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(5, 3);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        var selected = (await CardSelectCmd.FromHand(ctx, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), c => c != this, this)).FirstOrDefault();
        if (selected == null)
            return;

        if (Owner.UnlockState.Characters.Any(c => c.CardPool.Id.Entry == selected.Pool.Id.Entry))
            await CardCmd.TransformToRandom(selected, Owner.RunState.Rng.CombatCardSelection);
        else
            await CardCmd.Exhaust(ctx, selected);
    }
}
