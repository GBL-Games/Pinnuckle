using System;
using System.Linq;
using Godot;
using Godot.Collections;
using Array = System.Array;

namespace Pinnuckle.Scripts
{
    public partial class MatchManager : Control
    {
        [Export] public string TrumpSuit;
        [Export] public int TotalPlayerDmg = 0;
        [Export] public int TotalOpponentDmg = 0;

        private Random _random;
        private SignalBus _signalBus;
        private string[] _suits = ["Spades", "Clubs", "Diamonds", "Hearts"];

        private CardData _selectedPlayerCard;
        private CardData _selectedOpponentCard;
        private int _selectedCardIndex;

        private bool _isPlayersTurn;
        private string _whoWentFirst;
        private int _currentTrick = 1;

        private Array<CardData> _currentPlayerHand = [];
        private Array<CardData> _currentOpponentHand = [];

        public override void _Ready()
        {
            _random = new Random();

            GD.Print("Match Started!");

            _ConnectSignals();
            _SetTrumpSuit();
        }

        private void _ConnectSignals()
        {
            _signalBus = GetNode<SignalBus>("/root/SignalBus");

            _signalBus.MeldListUpdate += _CalculateMeldsDamage;
            _signalBus.CardHandUpdated += _SetLocalHands;
            _signalBus.CardSelected += _PlayerCardSelected;

            _signalBus.EmitSignal("ShuffleCards");
            _signalBus.EmitSignal("DealCards", 12, "player");
            _signalBus.EmitSignal("DealCards", 12, "opponent");
        }

        #region Round Setup

        private void _SetTrumpSuit()
        {
            TrumpSuit = _suits[_random.Next(0, 3)];
            GetNode<RichTextLabel>("Match UI/UI Right/TrumpSuit").Text = "Trump Suit: " + TrumpSuit;

            _ChooseFirstMove();

            if (!_isPlayersTurn)
            {
                _OpponentsTurn();
            }
        }

        private void _SetLocalHands(string handOwner, Array<CardData> hand)
        {
            if (handOwner == "player")
            {
                _currentPlayerHand.AddRange(hand);
            }
            else
            {
                _currentOpponentHand.AddRange(hand);
            }
        }

        private void _ChooseFirstMove()
        {
            _isPlayersTurn = _random.Next(0, 20) >= 10;
            _currentTrick = 1;

            if (_isPlayersTurn)
            {
                _whoWentFirst = "player";
            }
            else
            {
                _whoWentFirst = "opponent";
            }
        }

        #endregion

        #region UI Updating

        private void _CalculateMeldsDamage(string listOwner, Array<MeldData> meldList)
        {
            int totalMeldsDmg = meldList.Aggregate(0, (acc, cur) => acc + cur.Value);

            if (listOwner == "player")
            {
                TotalPlayerDmg = totalMeldsDmg;
            }
            else
            {
                TotalOpponentDmg = totalMeldsDmg;
            }

            _UpdateDamageText(listOwner);
        }

        private void _UpdateDamageText(string listOwner)
        {
            int totalDmg = listOwner == "player" ? TotalPlayerDmg : TotalOpponentDmg;
            NodePath path = listOwner == "player"
                ? "Match UI/UI Right/PlayerMeldsList/Hand Damage"
                : "Match UI/OpponentMeldsList/Hand Damage";
            GetNode<RichTextLabel>(path).Text =
                "Total Damage: " + totalDmg;
        }

        #endregion

        #region Player Card Playing

        private void _PlayerCardSelected(CardData cardData)
        {
            _selectedPlayerCard = cardData;
        }

        private void _on_play_card(InputEvent @event)
        {
            if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
            {
                _RunCurrentTrick();
                // if (_selectedPlayerCard != null)
                // {
                //     TotalPlayerDmg += _selectedPlayerCard.Value;
                //     _UpdateDamageText("player");
                //     _signalBus.EmitSignal("CardPlayed", _selectedPlayerCard.Id);
                //     _selectedCardIndex = -1;
                //
                //     if (GetNode<HBoxContainer>("Hands/Opponent").GetChildCount() > 0)
                //     {
                //         _OpponentsTurn();
                //     }
                // }
            }
        }

        #endregion

        #region Turn Management

        private void _RunCurrentTrick()
        {
            GD.Print("Current Trick: " + _currentTrick);
            GD.Print("Selected Player Card: " + _selectedPlayerCard.Title());

            _OpponentsTurn();

            GD.Print("Selected Opponent Card: " + _selectedOpponentCard.Title());
        }

        #endregion

        #region Opponent AI

        private void _OpponentsTurn()
        {
            _selectedOpponentCard = _FindBestOpponentCard();
        }

        private CardData _FindBestOpponentCard()
        {
            CardData bestCard = null;

            foreach (CardData cardData in _currentOpponentHand)
            {
                if (cardData.Value > _selectedPlayerCard.Value && (bestCard == null || cardData.Value < bestCard.Value))
                {
                    bestCard = cardData;
                }
                else if (cardData.Value == _selectedPlayerCard.Value && bestCard == null)
                {
                    bestCard = cardData;
                }
            }

            return bestCard;
        }

        #endregion
    }
}