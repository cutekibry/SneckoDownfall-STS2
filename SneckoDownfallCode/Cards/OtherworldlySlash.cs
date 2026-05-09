using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

// Reference: STS1 OtherworldlySlash; SneckoSoul handles Gift rewards when this enters the deck.
public class OtherworldlySlash : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Common;
    protected override bool ShouldGlowGoldInternal => HasPlayedOffclassCardThisTurn();

    public OtherworldlySlash() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(7, 2);
        WithKeyword(SneckoDownfallKeyword.Offclass);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, HasPlayedOffclassCardThisTurn() ? 2 : 1).Execute(ctx);
    }

    private bool HasPlayedOffclassCardThisTurn()
    {
        return CombatManager.Instance.History.Entries
            .OfType<CardPlayFinishedEntry>()
            .Any(e => e.Actor == Owner.Creature && e.HappenedThisTurn(CombatState) && e.CardPlay.Card.IsOffclass());
    }
}
