using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 RoundaboutSwing; STS2 PhotonCut/Relax for put-on-deck and next-turn draw.
public class RoundaboutSwing : SneckoDownfallCard
{
    public RoundaboutSwing() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(8, 3);
        WithCards(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        var selected = await CardSelectCmd.FromHand(ctx, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), c => c != this, this);
        await CardPileCmd.Add(selected, PileType.Draw, CardPilePosition.Top);
        await PowerCmd.Apply<DrawCardsNextTurnPower>(ctx, Owner.Creature, DynamicVars.Cards.BaseValue, Owner.Creature, this);
    }
}
