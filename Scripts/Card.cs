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
        private bool _dragging;

        private bool _mouseIn;
        private bool _mouseOut;

        private int _atlasX = 350;
        private int _atlasY = 108;
        private int _zIndex;
        private Vector2 _originalPosition;

        public override void _Ready()
        {
            _signalBus = GetNode<SignalBus>("/root/SignalBus");

            MouseFilter = MouseFilterEnum.Stop;
            MouseDefaultCursorShape = CardOwner == "player" ? CursorShape.PointingHand : CursorShape.Arrow;

            _zIndex = ZIndex;
            _originalPosition = GlobalPosition;

            TextureNormal = _GetCardAtlas();

            base._Ready();
        }

        public override void _GuiInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
            {
                _dragging = true;
            }

            if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false })
            {
                if (_dragging)
                {
                    _dragging = false;
                    _ResetTween();
                    _tween.TweenProperty(this, "global_position", _originalPosition, .5f)
                        .SetTrans(Tween.TransitionType.Elastic);
                    _tween.Finished += () =>
                    {
                        _tween.Kill();
                        ZIndex = _zIndex;
                    };
                }
            }

            if (@event is InputEventMouseMotion mouseMotion && _dragging)
            {
                GlobalPosition = new Vector2(GetGlobalMousePosition().X - 12.5f, GetGlobalMousePosition().Y - 18);
            }
        }

        private void _ResetTween()
        {
            if (_tween != null)
            {
                _tween.Kill();
            }

            _tween = CreateTween();
        }

        private void _on_mouse_entered()
        {
            if (_dragging) return;
            if (CardOwner != "player") return;

            _ResetTween();
            _tween.TweenProperty(this, "position", new Vector2(Position.X, Position.Y - 10), .1f)
                .SetTrans(Tween.TransitionType.Sine);
            _tween.TweenProperty(this, "scale", new Vector2(1.25f, 1.25f), .1f)
                .SetTrans(Tween.TransitionType.Back);
            ZIndex = 12;
        }

        private void _on_mouse_exited()
        {
            if (CardOwner != "player") return;

            _ResetTween();
            _tween.TweenProperty(this, "position", new Vector2(Position.X, 0), .025f)
                .SetTrans(Tween.TransitionType.Sine);
            _tween.TweenProperty(this, "scale", new Vector2(1, 1), .025f).SetTrans(Tween.TransitionType.Sine);
            ZIndex = _zIndex;
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