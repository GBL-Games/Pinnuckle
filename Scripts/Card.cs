using System;
using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts
{
    [GlobalClass]
    public partial class Card : TextureButton
    {
        public CardData CardInfo;
        public string CardOwner;

        private int _clickRadius = 10;
        private bool _selected;
        private bool _hovering;
        private int _targetY = -10;

        private int _atlasX = 350;
        private int _atlasY = 108;

        public override void _Ready()
        {
            MouseFilter = MouseFilterEnum.Stop;
            MouseDefaultCursorShape = CardOwner == "player" ? CursorShape.PointingHand : CursorShape.Arrow;

            TextureNormal = _GetCardAtlas();
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
            if (@event is InputEventMouseButton mbe && mbe.ButtonIndex == MouseButton.Left && mbe.Pressed &&
                CardOwner == "player")
            {
                GD.Print(CardInfo.Title());

                _selected = !_selected;
            }
        }

        private AtlasTexture _GetCardAtlas()
        {
            AtlasTexture atlas = new AtlasTexture();
            Resource img = ResourceLoader.Load("res://Assets/pixelCards/CuteCardsPixel_outline.png");
            atlas.Atlas = (Texture2D)img;

            if (CardOwner == "player")
            {
                _SetAtlasRegion();
            }

            Vector2 regionSize = new Vector2(25, 36);
            Vector2 regionPosition = new Vector2(_atlasX, _atlasY);
            atlas.Region = new Rect2(regionPosition, regionSize);

            return atlas;
        }

        private void _SetAtlasRegion()
        {
            _atlasX = CardInfo.Rank switch
            {
                "Ace" => 0,
                "9" => 200,
                "10" => 225,
                "Jack" => 250,
                "Queen" => 275,
                "King" => 300,
                _ => 350
            };

            _atlasY = CardInfo.Suit switch
            {
                "Clubs" => 0,
                "Diamonds" => 36,
                "Spades" => 72,
                "Hearts" => 108,
                _ => 108
            };
        }
    }
}