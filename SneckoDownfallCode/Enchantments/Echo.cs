using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;

namespace SneckoDownfall.SneckoDownfallCode.Enchantments;

// TODO: Change UnendingSupplyPower to use this
public class Echo : SneckoDownfallEnchantment
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
    ];
    protected override void OnEnchant()
    {
        Log.Info($"Enchanting {Card.Id.Entry} with Echo");
        Log.Info($"Card.Keywords: {string.Join(", ", Card.Keywords)}");
        if (!Card.Keywords.Contains(CardKeyword.Exhaust))
            Card.AddKeyword(CardKeyword.Exhaust);
        if (!Card.Keywords.Contains(CardKeyword.Ethereal))
            Card.AddKeyword(CardKeyword.Ethereal);
    }
}
