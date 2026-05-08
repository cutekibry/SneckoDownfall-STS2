using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using SneckoDownfall.SneckoDownfallCode.Enchantments;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Lacerate; local CripplingPoison token and VenomPower.
public class Lacerate : SneckoDownfallCard
{
    public Lacerate() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithPower<VenomPower>(4, true);
        WithKeyword(CardKeyword.Exhaust);
        WithTips(card => [HoverTipFactory.FromCard<CripplingPoison>(card.IsUpgraded)]);
        WithTip(typeof(Fake));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
            await PowerCmd.Apply<VenomPower>(ctx, enemy, DynamicVars["VenomPower"].BaseValue, Owner.Creature, this);

        var card = CombatState.CreateCard<CripplingPoison>(Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(card);
        CardCmd.Enchant<Fake>(card, 1m)!.SetCharacter(ModelDb.Character<Silent>());
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}
