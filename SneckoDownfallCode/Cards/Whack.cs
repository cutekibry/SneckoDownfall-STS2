using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using SneckoDownfall.SneckoDownfallCode.Enchantments;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Whack; local Cower/HoleUp for generated-card creation because STS2 has no Wallop.
public class Whack : SneckoDownfallCard
{
    public Whack() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9, 3);
        WithKeyword(CardKeyword.Exhaust);
        WithTips(card => [HoverTipFactory.FromCard<Fisticuffs>(card.IsUpgraded)]);
        WithTip(typeof(Fake));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        var card = CombatState!.CreateCard<Fisticuffs>(Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(card);
        var fake = CardCmd.Enchant<Fake>(card, 1m)!;
        fake.SetCharacter(ModelDb.Character<Necrobinder>());
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}
