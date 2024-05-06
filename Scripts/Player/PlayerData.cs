using Godot;
using Pinnuckle.Scripts.Archetype;

namespace Pinnuckle.Scripts.Player;

public partial class PlayerData : GodotObject
{
    public ArchetypeData PlayerArchetype { get; set; }
    public int Health { get; set; }
}