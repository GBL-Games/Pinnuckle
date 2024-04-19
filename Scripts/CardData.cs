using System.Runtime.CompilerServices;
using Godot;

public partial class CardData : GodotObject
{
    public string Id { get; set; }
    public string Rank { get; set; }
    public string Suit { get; set; }

    public int Value { get; set; }

    public string Title() => $"{Rank} of {Suit}";
}