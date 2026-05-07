using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 LilGuardian; STS2 Stomp for in-hand BeforeCardPlayed hooks.
public class LilGuardian : SneckoDownfallCard
{
    public LilGuardian() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(7, 2);
        WithEnergy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card != this && play.Card.Owner == Owner && play.Card.EnergyCost.GetResolved() >= DynamicVars.Energy.IntValue && PileType.Hand.GetPile(Owner).Cards.Contains(this))
            await CardCmd.AutoPlay(ctx, this, Owner.Creature);
    }
}
