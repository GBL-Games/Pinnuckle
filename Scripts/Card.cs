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
        public int CardIndex;

        private SignalBus _signalBus;
        private Tween _tween;
        private bool _selected;
        private int _atlasX = 350;
        private int _atlasY = 108;
        private int _zIndex;
        private float _originalY;


        public override void _Ready()
        {
            _signalBus = GetNode<SignalBus>("/root/SignalBus");

            MouseFilter = MouseFilterEnum.Stop;
            MouseDefaultCursorShape = CardOwner == "player" ? CursorShape.PointingHand : CursorShape.Arrow;

            _zIndex = ZIndex;
            _originalY = Position.Y;

            TextureNormal = _GetCardAtlas();
        }

        private void _on_mouse_entered()
        {
            if (CardOwner == "player")
            {
                if (_tween != null)
                    _tween.Kill();

                _tween = CreateTween();
                _tween.TweenProperty(this, "position", new Vector2(Position.X, Position.Y - 10), .1f)
                    .SetTrans(Tween.TransitionType.Sine);
                _tween.TweenProperty(this, "scale", new Vector2(1.25f, 1.25f), .1f)
                    .SetTrans(Tween.TransitionType.Back);
                ZIndex = 12;
            }
        }

        private void _on_mouse_exited()
        {
            if (CardOwner == "player")
            {
                if (_tween != null)
                    _tween.Kill();

                _tween = CreateTween();
                _tween.TweenProperty(this, "position", new Vector2(Position.X, 0), .025f)
                    .SetTrans(Tween.TransitionType.Sine);
                _tween.TweenProperty(this, "scale", new Vector2(1, 1), .025f).SetTrans(Tween.TransitionType.Sine);
                ZIndex = _zIndex;
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