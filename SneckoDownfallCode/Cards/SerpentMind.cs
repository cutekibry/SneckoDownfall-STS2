using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 SerpentMind; STS2 DemonFormPower for recurring Strength gain.
public class SerpentMind : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Rare;

    public SerpentMind() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<SerpentMindPower>(1, false);
        WithTip(typeof(StrengthPower));
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<SerpentMindPower>(ctx, Owner.Creature, DynamicVars["SerpentMindPower"].BaseValue, Owner.Creature, this);
    }
}
