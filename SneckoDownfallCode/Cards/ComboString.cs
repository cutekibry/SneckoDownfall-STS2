using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using SneckoDownfall.Character;
using SneckoDownfall.SneckoDownfallCode.Character;
using SneckoDownfall.SneckoDownfallCode.Extensions;

namespace SneckoDownfall.SneckoDownfallCode.Cards;

public class ComboString : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Uncommon;  
    public ComboString() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(7, 2);
        WithCalculatedVar("Hits", 0, GetHits);
        WithKeyword(SneckoDownfallKeyword.Offclass);
    }
    private static decimal GetHits(CardModel card, Creature? _)
    {
        return CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().Count(e => e.CardPlay.Card.IsOffclass() && e.Actor == card.Owner.Creature && e.HappenedThisTurn(card.CombatState));
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, (int)GetCalculatedValue("Hits", play)).Execute(ctx);
    }
}