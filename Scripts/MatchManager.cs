using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts
{
    public partial class MatchManager : Node2D
    {
        [Export] public string TrumpSuit;
        [Export] public int TotalPlayerDmg = 0;

        private Random _random;
        private SignalBus _signalBus;
        private string[] _suits = ["Spades", "Clubs", "Diamonds", "Hearts"];


        public override void _Ready()
        {
            _random = new Random();

            GD.Print("Match Started!");
            _signalBus = GetNode<SignalBus>("/root/SignalBus");
            _signalBus.MeldListUpdate += CalculatePlayerDamage;
            _signalBus.EmitSignal("ShuffleCards");
            _signalBus.EmitSignal("DealCards", 12, "player");

            SetTrumpSuit();
        }

        private void CalculatePlayerDamage(Array<MeldData> meldList)
        {
            TotalPlayerDmg = meldList.Aggregate(0, (acc, cur) => acc + cur.Value);
            UpdatePlayerDamageText();
        }

        private void UpdatePlayerDamageText()
        {
            GetNode<RichTextLabel>("Match UI/MeldsList/Hand Damage").Text =
                "Total Player Damage: " + TotalPlayerDmg.ToString();
        }

        private void SetTrumpSuit()
        {
            TrumpSuit = _suits[_random.Next(0, 3)];
            GetNode<RichTextLabel>("Match UI/MeldsList/TrumpSuit").Text = "Trump Suit: " + TrumpSuit;
        }
    }
}