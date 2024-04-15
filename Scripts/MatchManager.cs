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
            _signalBus.MeldListUpdate += CalculateDamage;
            _signalBus.EmitSignal("ShuffleCards");
            _signalBus.EmitSignal("DealCards", 12, "player");
            _signalBus.EmitSignal("DealCards", 12, "opponent");

            SetTrumpSuit();
        }

        private void CalculateDamage(string listOwner, Array<MeldData> meldList)
        {
            TotalPlayerDmg = meldList.Aggregate(0, (acc, cur) => acc + cur.Value);
            NodePath path = listOwner == "player"
                ? "Match UI/UI Right/PlayerMeldsList/Hand Damage"
                : "Match UI/OpponentMeldsList/Hand Damage";
            UpdatePlayerDamageText(path);
        }

        private void UpdatePlayerDamageText(NodePath path)
        {
            GetNode<RichTextLabel>(path).Text =
                "Total Damage: " + TotalPlayerDmg.ToString();
        }

        private void SetTrumpSuit()
        {
            TrumpSuit = _suits[_random.Next(0, 3)];
            GetNode<RichTextLabel>("Match UI/UI Right/TrumpSuit").Text = "Trump Suit: " + TrumpSuit;
        }
    }
}