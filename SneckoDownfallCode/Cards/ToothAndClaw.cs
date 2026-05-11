using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using BaseLib.Utils;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 ToothAndClaw; local GiftFilter handling via SneckoSoul.
public class ToothAndClaw : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Uncommon;

    public ToothAndClaw() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(4, 2);
        WithCalculatedVar("Colors", 0, CountUniquePoolsInHand);
        WithTips(card => [HoverTipFactory.FromCard<Shiv>(card.IsUpgraded)]);
    }

    private static decimal CountUniquePoolsInHand(CardModel card, MegaCrit.Sts2.Core.Entities.Creatures.Creature? _)
    {
        return PileType.Hand.GetPile(card.Owner).Cards
            .Where(c => c != card)
            .Select(c => c.Pool.ColorId())
            .Distinct()
            .Count();
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);

        for (var i = 0; i < GetCalculatedValue("Colors", play); i++)
        {
            var card = CombatState!.CreateCard<Shiv>(Owner);
            if (IsUpgraded)
                CardCmd.Upgrade(card);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }
}
