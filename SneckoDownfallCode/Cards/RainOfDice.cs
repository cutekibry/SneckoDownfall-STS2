using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 RainOfDice; local SnekBite/Cower for MuddleHand and generated self-copy.
public class RainOfDice : SneckoDownfallCard
{
    public RainOfDice() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6, 2);
        WithMuddle(1);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        await SneckoActions.MuddleHand(ctx, this);
        var card = CombatState!.CreateCard<RainOfDice>(Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}
