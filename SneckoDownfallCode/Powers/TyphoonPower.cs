using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using SneckoDownfall.SneckoDownfallCode.Cards;
using MegaCrit.Sts2.Core.Models.Cards;
using SneckoDownfall.SneckoDownfallCode.Enchantments;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

public class TyphoonPower : SneckoDownfallPower
{
    protected virtual bool IsUpgraded => false;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private bool IsFirstTrigger = true;

    public override async Task AfterCardPlayedLate(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Owner != Owner.Player || play.Card is not SneckoDownfallCard { IsOverflowed: true })
            return;

        if (IsFirstTrigger)
        {   
            // Skip the first time triggered by the card itself.
            IsFirstTrigger = false;
            return;
        }

        Flash();
        for (var i = 0; i < Amount; i++)
        {
            var target = Owner.Player.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (target == null)
                continue;

            var diveBomb = CombatState.CreateCard<MinionDiveBomb>(Owner.Player);
            if(IsUpgraded)
                CardCmd.Upgrade(diveBomb);
            var fake = CardCmd.Enchant<Fake>(diveBomb, 1m)!;
            fake.SetCharacter(ModelDb.Character<Regent>());
            await CommonActions.CardAttack(diveBomb, target).Execute(ctx);
        }
    }
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
            await PowerCmd.Remove(this);
    }
}
