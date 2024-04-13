using Godot;

namespace Pinnuckle.Scripts
{
    [GlobalClass]
    public partial class Card : Sprite2D
    {
        public CardData CardInfo;
        public string CardOwner;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Frame = CardOwner == "player" ? CardInfo.SpriteIndex : 59;
        }
    }
}