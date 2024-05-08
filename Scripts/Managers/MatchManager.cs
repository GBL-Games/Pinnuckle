using System;
using System.Linq;
using Godot;
using Godot.Collections;
using Pinnuckle.addons.ScenicRoute;
using Pinnuckle.Scripts.Autoload;
using Pinnuckle.Scripts.Match;

namespace Pinnuckle.Scripts.Managers
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
        private string _ledSuit;
        private bool _canPlayCard;

        private Array<CardData> _currentPlayerHand = [];
        private Array<CardData> _currentOpponentHand = [];

        private ScenicRoute _scenicRoute;
        private GameState _gameState;
        private HudManager _hudManager;

        public override void _Ready()
        {
            _random = new Random();

            GD.Print("Match Started!");

            _ConnectSignals(); // Connects all the relevant signals
            _LoadGameState(); // Loads the game state passed from the previous screen
            _SetTrumpSuit(); // Sets the trump suit
            _ChooseFirstMove(); // Sets who goes first 
        }

        #region Match Setup

        private void _ConnectSignals()
        {
            _signalBus = GetNode<SignalBus>("/root/SignalBus");

            _signalBus.CardHandUpdated += _UpdateLocalHands;
            _signalBus.CardSelected += _PlayerCardSelected;
            _signalBus.MeldListUpdate += _CalculateMeldsDamage;

            _signalBus.EmitSignal("ShuffleCards");
            _signalBus.EmitSignal("DealCards", 12, "player");
            _signalBus.EmitSignal("DealCards", 12, "opponent");
        }

        private void _LoadGameState()
        {
            _scenicRoute = GetNodeOrNull<ScenicRoute>("/root/ScenicRoute");
            _gameState = (GameState)_scenicRoute.GetCurrentGameState();

            _signalBus.EmitSignal(nameof(_signalBus.PlayerHpChange), -500);
        }

        private void _UpdateLocalHands(string handOwner, Array<CardData> hand)
        {
            if (handOwner == "player")
            {
                _currentPlayerHand = [];
                _currentPlayerHand.AddRange(hand);
            }
            else
            {
                _currentOpponentHand = [];
                _currentOpponentHand.AddRange(hand);
            }
        }

        private void _SetTrumpSuit()
        {
            TrumpSuit = _suits[_random.Next(0, 3)];
            GetNode<RichTextLabel>("Match UI/UI Right/TrumpSuit").Text = "Trump Suit: " + TrumpSuit;
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
                _OpponentsTurn();
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
                _signalBus.EmitSignal(nameof(_signalBus.PlayerAtkChange), totalMeldsDmg);
            }
            else
            {
                TotalOpponentDmg = totalMeldsDmg;
            }

            _UpdateDamageText(listOwner);
        }

        private void _UpdateDamageText(string listOwner)
        {
            bool isLastTrick = _currentTrick == 12;

            if (listOwner == "player" && _selectedPlayerCard != null)
                TotalPlayerDmg += isLastTrick && _selectedPlayerCard.Rank != "Ace" ? 10 : _selectedPlayerCard.Value;
            if (listOwner == "opponent" && _selectedOpponentCard != null)
                TotalOpponentDmg += isLastTrick && _selectedOpponentCard.Rank != "Ace"
                    ? 10
                    : _selectedOpponentCard.Value;

            int totalDmg = listOwner == "player" ? TotalPlayerDmg : TotalOpponentDmg;

            NodePath path = listOwner == "player"
                ? "Match UI/UI Right/PlayerMeldsList/Hand Damage"
                : "Match UI/OpponentMeldsList/Hand Damage";

            if (listOwner == "player" && _selectedPlayerCard != null)
            {
                _signalBus.EmitSignal(nameof(_signalBus.PlayerAtkChange),
                    isLastTrick && _selectedPlayerCard.Rank != "Ace" ? 10 : _selectedPlayerCard.Value);
            }

            GetNode<RichTextLabel>(path).Text =
                "Total Damage: " + totalDmg;
        }

        #endregion

        #region Player Card Playing

        // Select player card
        private void _PlayerCardSelected(CardData cardData)
        {
            _signalBus.EmitSignal(nameof(_signalBus.PlayerHpChange), -500);
            GD.Print(cardData.Title() + " selected");
            _selectedPlayerCard = cardData;
        }

        // Play player card
        private void _PlayerCardPlayed()
        {
            if (_whoWentFirst == "opponent") _RunCurrentTrick(); // If opponent went first run the trick
            if (_whoWentFirst == "player") _OpponentsTurn(); // If player went first run opponent turn
        }

        // Play card button click handler
        private void _on_play_card(InputEvent @event)
        {
            if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
            {
                // prevent playing card if no card is selected
                if (_selectedPlayerCard == null) return;
                _PlayerCardPlayed();
            }
        }

        #endregion

        #region Turn Management

        // Runs the current trick based on cards selected
        private void _RunCurrentTrick()
        {
            string trickWinner = _CheckTrickWinner(); // check to see who won the trick

            if (_currentTrick < 12)
            {
                _currentTrick++; // increase the trick counter 
            }

            GD.Print("Trick winner: " + trickWinner);

            // Update the dmg ui of the trick winner as only they get an increase to their dmg
            _UpdateDamageText(trickWinner);

            // Reset selected cards
            _selectedOpponentCard = null;
            _selectedPlayerCard = null;

            // if opponent went first tell them to run next turn
            if (_whoWentFirst == "opponent") _OpponentsTurn();
        }

        // Checks to see who took the trick based on the selected cards
        private string _CheckTrickWinner()
        {
            string playerSuit = _selectedPlayerCard.Suit;
            int playerValue = _selectedPlayerCard.Value;
            string opponentSuit = _selectedOpponentCard.Suit;
            int opponentValue = _selectedOpponentCard.Value;

            if (_whoWentFirst == "player")
            {
                // Run if player suit is trump suit
                if (playerSuit == TrumpSuit)
                {
                    // if opponent suit is trump suit & player card value is <= to player card value
                    if (opponentSuit == TrumpSuit)
                    {
                        if (opponentValue <= playerValue) return "player";
                        if (opponentValue > playerValue) return "opponent";
                    }

                    return "player";
                }

                // Run if player suit is not trump suit & opponent suit is trump suit
                if (opponentSuit == TrumpSuit) return "opponent";

                // Run if player suit is not trump suit & opponent suit is not trump suit
                if (playerSuit != TrumpSuit && opponentSuit != TrumpSuit)
                {
                    // Run if opponent suit isn't led suit
                    if (playerSuit != opponentSuit || (playerSuit == opponentSuit && playerValue >= opponentValue))
                        return "player";
                    // Run if opponent suit is led suit
                    if (playerSuit == opponentSuit && opponentValue > playerValue) return "opponent";
                }
            }

            // Same as above only if the opponent went first
            if (_whoWentFirst == "opponent")
            {
                if (opponentSuit == TrumpSuit)
                {
                    if (playerSuit == TrumpSuit)
                    {
                        if (playerValue <= opponentValue) return "opponent";
                        if (playerValue > opponentValue) return "player";
                    }
                }

                if (playerSuit == TrumpSuit) return "player";

                if (opponentSuit != TrumpSuit && opponentSuit != TrumpSuit)
                {
                    if (opponentSuit != playerSuit || (opponentSuit == playerSuit && opponentValue >= playerValue))
                        return "opponent";
                    if (opponentSuit == playerSuit && playerValue > opponentValue) return "player";
                }
            }

            return _whoWentFirst;
        }

        #endregion

        #region Opponent AI

        private void _OpponentsTurn()
        {
            _selectedOpponentCard = _FindBestOpponentCard();
            _signalBus.EmitSignal("CardPlayed", "opponent", _selectedOpponentCard);
            GD.Print("Opponent plays: " + _selectedOpponentCard.Title());

            if (_whoWentFirst == "player") _RunCurrentTrick();
        }

        private CardData _FindBestOpponentCard()
        {
            CardData bestCard = null;
            CardData lowestTrumpCard = null;
            CardData lowestCard = null;

            // Run if the player went first
            foreach (CardData cardData in _currentOpponentHand)
            {
                if (_whoWentFirst == "player")
                {
                    // Check if the card has a higher value that the selected card
                    // and if it's higher than the current best card.
                    if (cardData.Value > _selectedPlayerCard.Value && cardData.Suit == _selectedPlayerCard.Suit &&
                        (bestCard == null || cardData.Value <= bestCard.Value))
                    {
                        bestCard = cardData;
                    }
                    // Check if the card has the trump suit and is the lowest trump card
                    else if (cardData.Suit == TrumpSuit &&
                             (lowestTrumpCard == null || cardData.Value < lowestTrumpCard.Value))
                    {
                        lowestTrumpCard = cardData;
                    }
                    // If the selected card is not from the trump suit
                    // and the card has a higher value that the selected card
                    // and if it's higher than the current best card
                    else if (cardData.Suit != TrumpSuit && cardData.Value > _selectedPlayerCard.Value &&
                             (bestCard == null || cardData.Value < bestCard.Value))
                    {
                        bestCard = cardData;
                    }
                }

                // Check if the card has the same suit as the trump suit
                // and is the lowest card with that suit
                if (cardData.Suit == TrumpSuit && (lowestCard == null || cardData.Value < lowestCard.Value))
                {
                    lowestCard = cardData;
                }
                // Check if the card is the lowest overal card
                else if (lowestCard == null || cardData.Value < lowestCard.Value)
                {
                    lowestCard = cardData;
                }
            }

            bestCard ??= lowestTrumpCard ?? lowestCard;

            // If no card with a higher value than the selected card is found
            // use the lowest trump card if available, otherwise use the lowest card
            return bestCard;
        }

        #endregion
    }
}