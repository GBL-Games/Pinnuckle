using Godot;
using Pinnuckle.Scripts.Archetype;

namespace Pinnuckle.Scripts.Player;

public partial class PlayerData : GodotObject
{
    public ArchetypeData PlayerArchetype { get; set; }
    public float Health { get; set; }
    public float Block { get; set; }
}