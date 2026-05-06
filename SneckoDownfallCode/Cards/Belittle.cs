using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SneckoDownfall.SneckoDownfallCode.Extensions;
using SneckoDownfall.SneckoDownfallCode.Utils;

namespace SneckoDownfall.SneckoDownfallCode.Cards;


public class Belittle : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => GiftSelector.IsDebuff(c) && c.Rarity == CardRarity.Uncommon;
    public Belittle() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithCalculatedVar("HpLoss", 0, 9, GetDebuffCount, 0, 3);
    }
    private static decimal GetDebuffCount(CardModel _, Creature? target)
    {
        return target?.Powers.Count(p => p.IsActualDebuff()) ?? 1;
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CreatureCmd.Damage(ctx, play.Target!, DynamicVars["HpLoss"].IntValue, ValueProp.Unblockable | ValueProp.Unpowered, this);
    }
}