using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using SneckoDownfall.SneckoDownfallCode.Actions;

namespace SneckoDownfall.SneckoDownfallCode.Powers;

// Reference: STS1 UnendingSupplyPower; STS2 CreativeAiPower/CallOfTheVoidPower for start-turn generated cards.
public class UnendingSupplyPower : SneckoDownfallPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner.Player)
            return;

        var cards = SneckoActions.GenerateRandomOffclassCards(player, Amount).ToList();

        for (int num = 0; num < Amount; num++)
        {
            if (!cards[num].Keywords.Contains(CardKeyword.Ethereal))
            {
                CardCmd.ApplyKeyword(cards[num], CardKeyword.Ethereal);
            }
            if (!cards[num].Keywords.Contains(CardKeyword.Exhaust))
            {
                CardCmd.ApplyKeyword(cards[num], CardKeyword.Exhaust);  
            }
        }

        Flash();
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, player);
    }
}
