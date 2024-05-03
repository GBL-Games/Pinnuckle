using System;
using Godot;
using Godot.Collections;
using Newtonsoft.Json;

namespace Pinnuckle.Scripts.Match
{
    public partial class Deck : Node2D
    {
        private Array<Match.CardData> _localDeck;
        private Autoload.SignalBus _signalBus;

        public Array<Match.CardData> ShuffledDeck;

        private static readonly Random Rand = new Random();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            GenerateDeck();
            ShuffledDeck = _localDeck;
            ConnectSignals();
        }

        private void GenerateDeck()
        {
            _localDeck = LoadCardData();
        }

        private void ConnectSignals()
        {
            _signalBus = GetNode<Autoload.SignalBus>("/root/SignalBus");
            _signalBus.ShuffleCards += ShuffleCards;
            _signalBus.DealCards += DealCards;
        }

        // Uses the FisherYates algorithm to shuffle.
        private void ShuffleCards()
        {
            for (int i = 0; i < ShuffledDeck.Count; i++)
            {
                int j = Rand.Next(i + 1);
                (ShuffledDeck[i], ShuffledDeck[j]) = (ShuffledDeck[j], ShuffledDeck[i]);
            }
        }

        private void DealCards(int amount, string cardsOwner)
        {
            Array<Match.CardData> cards = ShuffledDeck[..amount];
            ShuffledDeck = ShuffledDeck.Slice(amount, ShuffledDeck.Count);

            _signalBus.EmitSignal("GiveCards", cardsOwner, cards);
        }

        private void ResetShuffleDeck()
        {
            ShuffledDeck = _localDeck;
            ShuffleCards();
        }

        private Array<Match.CardData> LoadCardData()
        {
            string file = FileAccess.Open("res://Assets/json/cards.json", FileAccess.ModeFlags.Read).GetAsText();
            return JsonConvert.DeserializeObject<Array<Match.CardData>>(file);
        }
    }
}