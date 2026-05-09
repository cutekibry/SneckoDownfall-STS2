using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 OverwhelmingPresence.
public class OverwhelmingPresence : SneckoDownfallCard
{
    public OverwhelmingPresence() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<OverwhelmingPresencePower>(1, false);
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
        WithKeyword(SneckoDownfallKeyword.Offclass);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<OverwhelmingPresencePower>(ctx, Owner.Creature, DynamicVars["OverwhelmingPresencePower"].BaseValue, Owner.Creature, this);
    }
}
