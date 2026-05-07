using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 DangerNoodle; local SnekBite for MuddleHand and CobraCoil for GiftFilter.
public class DangerNoodle : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => !c.EnergyCost.CostsX && c.EnergyCost.Canonical >= 3;

    public DangerNoodle() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(14, 4);
        WithMuddle(1);
        WithEnergy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        await SneckoActions.MuddleHand(ctx, this);
    }
}
