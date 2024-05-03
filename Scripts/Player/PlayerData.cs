using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts.Player;

public partial class ClassSkill : GodotObject
{
    public string Id { get; set; }
    public string SkillName { get; set; }

    public enum SkillType
    {
        attack,
        defense,
        enhance,
        impair
    }

    public int SkillCooldown { get; set; }
    public int SkillDuration { get; set; }
}

public partial class ClassData : GodotObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Array<ClassSkill> Skills { get; set; }
}

public partial class PlayerData : GodotObject
{
    public ClassData PlayerClass { get; set; }
    public int PlayerHp { get; set; }
}