using Godot;

[GlobalClass]
public partial class Card : Sprite2D
{
	public CardData CardInfo;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Frame = CardInfo.SpriteIndex;
	}
}
