using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 ViperEssence; local HoleUp/SoulRoll for TokenCardPool pattern.
[Pool(typeof(TokenCardPool))]
public class ViperEssence : SneckoDownfallCard
{
    public ViperEssence() : base(0, CardType.Power, CardRarity.Token, TargetType.Self)
    {
        WithPower<StrengthPower>(1, 1, true);
        WithKeyword(CardKeyword.Ethereal);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await PowerCmd.Apply<StrengthPower>(ctx, Owner.Creature, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
    }
}
