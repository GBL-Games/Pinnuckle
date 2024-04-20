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

            _signalBus.CardPlayed += _CardPlayed;
            _signalBus.CardSelected += _CardSelected;

            MouseFilter = MouseFilterEnum.Stop;
            MouseDefaultCursorShape = CardOwner == "player" ? CursorShape.PointingHand : CursorShape.Arrow;

            _zIndex = ZIndex;
            _originalPosition = GlobalPosition;

            TextureNormal = _GetCardAtlas();

            base._Ready();
        }

        #region Card Selection & Play

        private void _CardSelected(CardData cardData)
        {
            if (cardData.Id == CardInfo.Id || CardOwner != "player") return;

            _FocusCard(_zIndex, false);
            _selected = false;
        }

        // TODO: PIN-17 - https://linear.app/pinnuckle/issue/PIN-17/destroy-card-instance
        private void _CardPlayed(string cardOwner, string id)
        {
            if (id != CardInfo.Id && CardOwner != cardOwner) return;
            Visible = false;
        }

        #endregion

        #region Card Input Events

        public override void _GuiInput(InputEvent @event)
        {
            if (CardOwner != "player") return;

            if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
            {
                if (_selected)
                {
                    _FocusCard(_zIndex, false);
                }
                else
                {
                    _signalBus.EmitSignal("CardSelected", CardInfo);
                    ZIndex = 12;
                }

                _selected = !_selected;
            }
        }

        private void _FocusCard(int zIndex, bool focus)
        {
            Vector2 pos = new Vector2(Position.X, focus ? Position.Y - 10 : 0);
            Vector2 scale = new Vector2(focus ? 1.25f : 1, focus ? 1.25f : 1);
            _ResetTween();
            _tween.TweenProperty(this, "position", pos, .01f)
                .SetTrans(Tween.TransitionType.Sine).SetDelay(.05f);
            _tween.TweenProperty(this, "scale", scale, .1f)
                .SetTrans(Tween.TransitionType.Back);
            ZIndex = focus ? zIndex : _zIndex;
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
            if (CardOwner != "player") return;

            if (!_selected)
            {
                _FocusCard(13, true);
            }
        }

        private void _on_mouse_exited()
        {
            if (CardOwner != "player") return;

            if (!_selected)
            {
                _FocusCard(_zIndex, false);
            }
        }

        #endregion

        #region Card Texture

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

        #endregion
    }
}