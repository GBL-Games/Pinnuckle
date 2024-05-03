using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts.Managers;

public enum EffectTypes
{
    Poison,
    Corrosion
}

public partial class StatusEffect : GodotObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public EffectTypes EffectType { get; set; }
}

public partial class ConsumableItem : GodotObject
{
    public string Name { get; set; }
    public StatusEffect Effect { get; set; }
    public int Amount { get; set; }
    public int Duration { get; set; }
}

public partial class Relic : GodotObject
{
    public string Name { get; set; }
}

public partial class Skill : GodotObject
{
    public string Name { get; set; }
    public string Type { get; set; }
    public ArchTypes ArchType { get; set; }
    public int Amount { get; set; }
}

public enum ArchTypes
{
    Knight,
    Priest,
    Mage,
    Rogue,
    Alchemist
}

public partial class PlayerArchType : GodotObject
{
    public ArchTypes ArchType { get; set; }
    public string Name { get; set; }
    public int BaseHp { get; set; }
    public int BaseDefense { get; set; }
    public Array<Skill> BaseSkills { get; set; }
}

public partial class Player : Node
{
    public PlayerArchType PlayerBaseClass;

    public int PlayerHp;
    public int PlayerAttack;
    public int PlayerDefense;
    public Array<Skill> PlayerSkills;
    public Array<ConsumableItem> PlayerConsumables;
    public Array<Relic> PlayerRelics;
    public Array<StatusEffect> StatusEffects;

    public override void _Ready()
    {
        _initializePlayer();
        base._Ready();
    }

    private void _initializePlayer()
    {
        PlayerHp = PlayerBaseClass.BaseHp;
        PlayerAttack = 0;
        PlayerDefense = PlayerBaseClass.BaseDefense;
        PlayerSkills = PlayerBaseClass.BaseSkills;
        PlayerConsumables = [];
        PlayerRelics = [];
        StatusEffects = [];
    }
}