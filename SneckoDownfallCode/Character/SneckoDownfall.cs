using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using SneckoDownfall.SneckoDownfallCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using SneckoDownfall.SneckoDownfallCode.Cards;
using SneckoDownfall.SneckoDownfallCode.Relics;

namespace SneckoDownfall.SneckoDownfallCode.Character;

public class SneckoDownfall : PlaceholderCharacterModel
{
    public const string CharacterId = "SneckoDownfall";
    
    public static readonly Color Color = new("ccf2ff");

    public override Color NameColor => Color;
    public override Color MapDrawingColor => Color;

    public override Color RemoteTargetingLineColor => Color;

    public override Color RemoteTargetingLineOutline => Color;
    public override Color EnergyLabelOutlineColor => new("0c3a36");
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 85;

    public override int BaseOrbSlotCount => 3;

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeSnecko>(),
        ModelDb.Card<StrikeSnecko>(),
        ModelDb.Card<StrikeSnecko>(),
        ModelDb.Card<StrikeSnecko>(),
        ModelDb.Card<DefendSnecko>(),
        ModelDb.Card<DefendSnecko>(),
        ModelDb.Card<DefendSnecko>(),
        ModelDb.Card<DefendSnecko>(),
        ModelDb.Card<TailWhip>(),
        ModelDb.Card<SnekBite>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<SneckoSoul>()
    ];
    
    public override CardPoolModel CardPool => ModelDb.CardPool<SneckoDownfallCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<SneckoDownfallRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<SneckoDownfallPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets. 
        These are just some of the simplest assets, given some placeholders to differentiate your character with. 
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomCharacterSelectBg => "res://SneckoDownfall/scenes/screens/char_select/char_select_bg_snecko_downfall.tscn";
    public override string CustomEnergyCounterPath => "res://SneckoDownfall/scenes/combat/energy_counters/snecko_downfall_energy_counter.tscn";
    public override string CustomVisualPath => "res://SneckoDownfall/scenes/creature_visuals/snecko_downfall.tscn";

    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        var idle = new AnimState("Idle", isLooping: true);
        var attack = new AnimState("Attack_2");
        var cast = new AnimState("Attack");
        var hit = new AnimState("Hit");

        idle.AddBranch("Attack", attack);
        idle.AddBranch("Cast", cast);
        idle.AddBranch("Hit", hit);
        idle.AddBranch("Dead", hit);
        idle.AddBranch("Relaxed", idle);

        attack.NextState = idle;
        hit.NextState = idle;

        var animator = new CreatureAnimator(idle, controller);
        animator.AddAnyState("Idle", idle);
        animator.AddAnyState("Revive", idle);
        animator.AddAnyState("Attack", attack);
        animator.AddAnyState("Cast", cast);
        animator.AddAnyState("Hit", hit);
        animator.AddAnyState("Dead", hit);
        return animator;
    }

    public override string CustomIconTexturePath => "character_icon_snecko_downfall.png".CharacterUiPath();
    public override string CustomIconOutlineTexturePath => "character_icon_snecko_downfall_outline.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_snecko_downfall.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_snecko_downfall_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_snecko_downfall.png".CharacterUiPath();
}
