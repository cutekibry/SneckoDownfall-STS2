using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Mudshield; local CheapStock/BlunderGuard for applying custom powers.
public class Mudshield : SneckoDownfallCard
{
    public Mudshield() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<MudshieldPower>(2, 1, false);
        WithTip(StaticHoverTip.Block);
        WithKeyword(SneckoDownfallKeyword.Muddle);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<MudshieldPower>(ctx, Owner.Creature, DynamicVars["MudshieldPower"].BaseValue, Owner.Creature, this);
    }
}
