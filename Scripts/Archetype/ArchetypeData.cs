using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts.Archetype;

public partial class ArchetypeData : GodotObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Health { get; set; }
    public Array<string> SubArchetypes { get; set; }
    public Array<ArchetypeSkill> Skills { get; set; }
    public bool IsSubArchetype { get; set; }

    public bool HasSubArchetypes()
    {
        return SubArchetypes.Count > 0;
    }
}