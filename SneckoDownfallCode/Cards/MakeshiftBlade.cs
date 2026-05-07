using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Extensions;
using SneckoDownfall.SneckoDownfallCode.Utils;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 MakeshiftBlade; local Belittle for debuff counting and GiftSelector.IsDebuff.
public class MakeshiftBlade : SneckoDownfallCard
{
    protected override bool ShouldGlowGoldInternal => CombatState?.HittableEnemies.Any(e => GetDebuffCount(this, e) >= DynamicVars["Requirement"].BaseValue) ?? false;
    public override Func<CardModel, bool>? GiftFilter => c => GiftSelector.IsDebuff(c) && c.Rarity != CardRarity.Rare;

    public MakeshiftBlade() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9, 4);
        WithCards(3);
        WithVar("Requirement", 3);
        WithCalculatedVar("Debuffs", 0, GetDebuffCount);
    }

    private static decimal GetDebuffCount(CardModel _, Creature? target)
    {
        return target?.Powers.Count(p => p.IsActualDebuff()) ?? 0;
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        if (GetCalculatedValue("Debuffs", play) >= DynamicVars["Requirement"].BaseValue)
            await CardPileCmd.Draw(ctx, DynamicVars.Cards.BaseValue, Owner);
    }
}
