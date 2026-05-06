using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Powers;

namespace SneckoDownfall.SneckoDownfallCode.Cards;



public class CobraCoil : SneckoDownfallCard
{
    public override Func<CardModel, bool>? GiftFilter => c => c.Rarity == CardRarity.Rare && c.Type == CardType.Attack;
    public CobraCoil() : base(4, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(20, 4);
        WithPower<ConstrictedPower>(10, 2, false);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        await PowerCmd.Apply<ConstrictedPower>(ctx, play.Target!, DynamicVars["ConstrictedPower"].IntValue, Owner.Creature, this);
    }
}