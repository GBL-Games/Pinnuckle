using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts.Archetype;

public enum DurationType
{
    Burst,
    Overtime
}

public enum EnhanceType
{
    Attack,
    Defense,
    Both
}

public enum ImpairType
{
    Corrode,
    Poison,
    Stun,
    Weaken
}

public partial class Effect : GodotObject
{
}

public partial class ArchetypeSkill : GodotObject
{
    public string Id { get; set; }
    public int Amount { get; set; }
    public string SkillName { get; set; }
    public string SkillDescription { get; set; }
    public int SkillAttack { get; set; }
    public int SkillDefense { get; set; }
    public int SkillCooldown { get; set; }
    public int SkillDuration { get; set; }
    public Array<string> SkillKeywords { get; set; }

    private Vector2 _skillIconPos;

    public Vector2 SkillIconPos
    {
        get => this._skillIconPos;

        set => this._skillIconPos = new Vector2(value.X, value.Y);
    }
}