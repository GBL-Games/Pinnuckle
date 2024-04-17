using Godot;
using Godot.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pinnuckle.Scripts
{
    public partial class HandManager : HBoxContainer
    {
        private SignalBus _signalBus;
        private Array<CardData> _currentHand = [];

        private PackedScene _card;

        [Export] public Curve SpreadCurve;
        [Export] public Curve HeightCurve;
        [Export] public float CardSpacing;

        [Export] public string HandOwner = "player";

        private Array<MeldData> _handMelds;
        private Array<MeldData> _allMelds;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _card = ResourceLoader.Load<PackedScene>("res://Objects/Card.tscn");
            _allMelds = LoadMelds();

            _signalBus = GetNode<SignalBus>("/root/SignalBus");
            _signalBus.GiveCards += AddToHand;
        }

        private void AddToHand(string owner, Array<CardData> cards)
        {
            if (owner == HandOwner)
            {
                _currentHand.AddRange(cards);
                UpdateHand();
            }
        }

        private void UpdateHand()
        {
            int totalCards = _currentHand.Count;

            for (int i = 0; i < totalCards; i++)
            {
                Card cardInstance = (Card)_card.Instantiate();
                cardInstance.CardInfo = _currentHand[i];
                cardInstance.CardOwner = HandOwner;
                cardInstance.CardIndex = i;
                AddChild(cardInstance);
            }

            _handMelds = GetHandMelds();
            _signalBus.EmitSignal("MeldListUpdate", HandOwner, _handMelds);
        }

        private Array<MeldData> GetHandMelds()
        {
            Array<MeldData> madeMelds = new Array<MeldData>();

            foreach (MeldData meldData in _allMelds)
            {
                bool isValidMeld = true;

                Godot.Collections.Dictionary<string, int> comboFrequency =
                    new Godot.Collections.Dictionary<string, int>();

                foreach (CardCombination cardCombination in meldData.CardCombinations)
                {
                    string key = $"{cardCombination.Rank}-{cardCombination.Suit}";

                    if (comboFrequency.ContainsKey(key))
                    {
                        comboFrequency[key]++;
                    }
                    else
                    {
                        comboFrequency[key] = 1;
                    }

                    bool cardFound = false;
                    string comboRank = cardCombination.Rank;
                    string comboSuit = cardCombination.Suit;

                    foreach (CardData cardData in _currentHand)
                    {
                        if (cardData.Rank == comboRank && (comboSuit == null || cardData.Suit == comboSuit))
                        {
                            cardFound = true;
                            break;
                        }
                    }

                    if (!cardFound)
                    {
                        isValidMeld = false;
                        break;
                    }
                }

                foreach (KeyValuePair<string, int> pair in comboFrequency)
                {
                    int requiredFrequency = pair.Value;
                    int foundFrequency = 0;

                    foreach (CardData card in _currentHand)
                    {
                        string rank = pair.Key.Split('-')[0];
                        string suit = pair.Key.Split('-')[1];

                        if (card.Rank == rank && (suit == "" || card.Suit == suit))
                        {
                            foundFrequency++;
                        }
                    }

                    if (foundFrequency < requiredFrequency)
                    {
                        isValidMeld = false;
                        break;
                    }
                }

                if (isValidMeld)
                {
                    madeMelds.Add(meldData);
                }
            }

            return madeMelds;
        }

        private static Array<MeldData> LoadMelds()
        {
            string file = FileAccess.Open("res://Assets/json/melds.json", FileAccess.ModeFlags.Read).GetAsText();
            return JsonConvert.DeserializeObject<Array<MeldData>>(file);
        }
    }
}