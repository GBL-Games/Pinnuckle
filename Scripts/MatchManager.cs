using System;
using System.Linq;
using Godot;
using Godot.Collections;

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

        private CardData _selectedCardData;
        private int _selectedCardIndex;

        private CardData _selectedPlayerCard;


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
            _signalBus.EmitSignal("ShuffleCards");
            _signalBus.EmitSignal("DealCards", 12, "player");
            _signalBus.EmitSignal("DealCards", 12, "opponent");

            _signalBus.CardSelected += _PlayerCardSelected;
        }

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

        private void _SetTrumpSuit()
        {
            TrumpSuit = _suits[_random.Next(0, 3)];
            GetNode<RichTextLabel>("Match UI/UI Right/TrumpSuit").Text = "Trump Suit: " + TrumpSuit;
        }

        private void _PlayerCardSelected(CardData cardData)
        {
            _selectedCardData = cardData;
        }

        private void _on_play_card(InputEvent @event)
        {
            if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
            {
                if (_selectedCardData != null)
                {
                    GD.Print("Total Dmg: " + TotalPlayerDmg, "Selected Dmg: " + _selectedCardData.Value);
                    TotalPlayerDmg += _selectedCardData.Value;
                    _UpdateDamageText("player");
                    _signalBus.EmitSignal("CardPlayed", _selectedCardData.Id);
                    _selectedCardData = null;
                    _selectedCardIndex = -1;
                }
            }
        }
    }
}