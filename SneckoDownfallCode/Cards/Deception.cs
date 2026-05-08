using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using SneckoDownfall.SneckoDownfallCode.Enchantments;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 Deception; STS2 Shockwave plus Begone/CardPileCmd for generated-card creation.
public class Deception : SneckoDownfallCard
{
    public Deception() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(11, 4);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(typeof(Shockwave));
        WithTip(typeof(Fake));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        var card = CombatState!.CreateCard<Shockwave>(Owner);
        CardCmd.Enchant<Fake>(card, 1m)!.SetCharacter(ModelDb.Character<Ironclad>());
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}
