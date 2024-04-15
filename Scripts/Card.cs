using Godot;

namespace Pinnuckle.Scripts
{
    [GlobalClass]
    public partial class Card : Control
    {
        public CardData CardInfo;
        public string CardOwner;

        private int _clickRadius = 10;
        private bool _selected;
        private bool _hovering;
        private int _targetY = -10;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            MouseFilter = MouseFilterEnum.Stop;

            if (CardOwner == "player")
            {
                GD.Print("Card Created");
            }

            GetNode<Sprite2D>("Card Sprite").Frame = CardOwner == "player" ? CardInfo.SpriteIndex : 59;
        }

        private void OnMouseEntered()
        {
            if (CardOwner == "player")
            {
                GetViewport().SetInputAsHandled();
                _hovering = true;
            }
        }

        private void OnMouseLeave()
        {
            if (CardOwner == "player")
            {
                GetViewport().SetInputAsHandled();
                _hovering = false;
            }
        }

        public override void _GuiInput(InputEvent @event)
        {
            GD.PrintT("Event: ", @event.AsText());

            if (@event is InputEventMouseButton mbe && mbe.ButtonIndex == MouseButton.Left && mbe.Pressed &&
                CardOwner == "player")
            {
                // GetViewport().SetInputAsHandled();
                GD.Print(CardInfo.Title());

                _selected = !_selected;
            }
        }
    }
}