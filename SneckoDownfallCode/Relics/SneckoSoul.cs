using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using SneckoDownfall.SneckoDownfallCode.Cards;
using SneckoDownfall.SneckoDownfallCode.Utils;

namespace SneckoDownfall.SneckoDownfallCode.Relics;



public class SneckoSoul : SneckoDownfallRelic
{
    private const int CharacterCount = 3;

    public override RelicRarity Rarity => RelicRarity.Starter;

    private List<ModelId> _characterIds = [];

    public List<ModelId> CharacterIds
    {
        get
        {
            return _characterIds;
        }
        set
        {
            AssertMutable();
            _characterIds.Clear();
            _characterIds.AddRange(value);
            ((StringVar)DynamicVars["Characters"]).StringValue = string.Join(", ", Characters.Select(c => $"[gold]{c.Title.GetFormattedText()}[/gold]"));
        }
    }

    [SavedProperty]
    public string SavedCharacterIds
    {
        get
        {
            return string.Join("|", _characterIds.Select(id => id.ToString()));
        }
        set
        {
            CharacterIds = string.IsNullOrEmpty(value)
                ? []
                : value.Split('|').Select(ModelId.Deserialize).ToList();
        }
    }

    private IEnumerable<CharacterModel> Characters
    {
        get
        {
            return _characterIds.Select(ModelDb.GetById<CharacterModel>);
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Characters")];

    private static CardModel GetRepresentativeCard(CharacterModel character)
    {
        var nonStrikeDefendCards = character.StartingDeck.Where(c => !c.Tags.Contains(CardTag.Strike) && !c.Tags.Contains(CardTag.Defend)).ToList();
        if (nonStrikeDefendCards.Count == 0)
            return character.StartingDeck.First();
        else
            return nonStrikeDefendCards.First();
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        _characterIds = [];
    }

    public async Task Initialize()
    {
        var characters = Owner.UnlockState.Characters.ToList();
        characters.Remove(Owner.Character);

        var selectedCharacters = new List<ModelId>();

        for (int i = 0; i < CharacterCount; i++)
        {
            var firstCharacter = Owner.RunState.Rng.Shuffle.NextItem(characters)!;
            var secondCharacter = Owner.RunState.Rng.Shuffle.NextItem(characters.Where(c => c.Id != firstCharacter.Id))!;

            var firstCard = Owner.RunState.CreateCard<MockCharacterChoiceCard>(Owner);
            var secondCard = Owner.RunState.CreateCard<MockCharacterChoiceSecondCard>(Owner);
            firstCard.Mock(GetRepresentativeCard(firstCharacter));
            secondCard.Mock(GetRepresentativeCard(secondCharacter));

            ((StringVar)firstCard.DynamicVars["Characters"]).StringValue = firstCharacter.Title.GetFormattedText();
            ((StringVar)secondCard.DynamicVars["Characters"]).StringValue = secondCharacter.Title.GetFormattedText();

            var card = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), [firstCard, secondCard], Owner, canSkip: false);

            if (card == firstCard)
            {
                selectedCharacters.Add(firstCharacter.Id);
                characters.Remove(firstCharacter);
            }
            else
            {
                selectedCharacters.Add(secondCharacter.Id);
                characters.Remove(secondCharacter);
            }
        }

        CharacterIds = selectedCharacters;
    }
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? source)
    {
        if (Owner.Creature.IsDead || card.Owner != Owner)
        {
            return;
        }

        CardPile? pile = card.Pile;
        if (pile != null && pile.Type == PileType.Deck && card is SneckoDownfallCard sneckoCard && sneckoCard.GiftFilter != null)
        {
            if (sneckoCard is GlitteringGambit)
            {
                await PlayerCmd.GainGold(sneckoCard.DynamicVars["Gold"].BaseValue, Owner);
                await GiftSelector.OfferGiftReward(Owner, sneckoCard.GiftFilter, true);
            }
            else
            {
                await GiftSelector.OfferGiftReward(Owner, sneckoCard.GiftFilter);
            }
        }
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext ctx, ICombatState combatState)
    {
        if (player == Owner && combatState.RoundNumber == 1)
            await CardPileCmd.AddGeneratedCardToCombat(Owner.Creature.CombatState!.CreateCard<SoulRoll>(Owner), PileType.Hand, Owner);
    }
}
