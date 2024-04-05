using System.Text.Json.Serialization;
using Godot;

public partial class CardObject : GodotObject
{
  public string Id { get; set; }
  public string Rank { get; set; }
  public string Suit { get; set; }
  public int SpriteIndex { get; set; }
  public int Value { get; set; }

  public string Title() => $"{Rank} of {Suit}";
}