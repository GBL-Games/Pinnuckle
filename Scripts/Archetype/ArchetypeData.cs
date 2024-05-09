using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts.Archetype;

public partial class ArchetypeData : GodotObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Health { get; set; }
    public float Block { get; set; }
    public float DodgeChance { get; set; }
    public Array<string> SubArchetypes { get; set; }
    public Array<ArchetypeSkill> Skills { get; set; }
    public bool IsSubArchetype { get; set; }

    public bool HasSubArchetypes()
    {
        return SubArchetypes.Count > 0;
    }
}