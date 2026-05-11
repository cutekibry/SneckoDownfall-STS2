using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Enchantments;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 UnendingSupply/UnendingSupplyPower; STS2 CreativeAiPower for start-turn generated cards.
public class UnendingSupply : SneckoDownfallCard
{
    public UnendingSupply() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(SneckoDownfallKeyword.Offclass);
        WithTip(typeof(Echo));
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithPower<UnendingSupplyPower>(1, false);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<UnendingSupplyPower>(ctx, Owner.Creature, DynamicVars["UnendingSupplyPower"].BaseValue, Owner.Creature, this);
    }
}
