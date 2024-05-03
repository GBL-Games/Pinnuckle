using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts.Match;

public abstract partial class CardCombination : GodotObject
{
    public string Rank { get; set; }
    public string Suit { get; set; }
}

public partial class MeldData : GodotObject
{
    private string Id { get; set; }
    public string Name { get; set; }
    public string MeldDisplay { get; set; }
    public int Value { get; set; }
    public Array<CardCombination> CardCombinations { get; set; }
}