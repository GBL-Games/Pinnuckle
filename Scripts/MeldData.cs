using Godot;
using Godot.Collections;

public partial class CardCombination : GodotObject
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