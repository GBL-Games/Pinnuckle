using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using Pinnuckle.Scripts.Autoload;

namespace Pinnuckle.Scripts.Managers;

[GlobalClass]
public partial class HandManager : HBoxContainer
{
    private SignalBus _signalBus;

    private Array<Match.CardData> _currentHand = [];

    private PackedScene _card;

    [Export] public Curve SpreadCurve;
    [Export] public Curve HeightCurve;
    [Export] public float CardSpacing;
    [Export] public NodePath PlayedCardDisplay;
    [Export] public string HandOwner = "player";

    private Array<Match.MeldData> _handMelds;
    private Array<Match.MeldData> _allMelds;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _card = ResourceLoader.Load<PackedScene>("res://Objects/Card.tscn");
        _allMelds = LoadMelds();

        _signalBus = GetNode<Autoload.SignalBus>("/root/SignalBus");
        _signalBus.GiveCards += _AddToHand;
        _signalBus.CardPlayed += _RemoveFromHand;
    }

    private void _AddToHand(string owner, Array<Match.CardData> cards)
    {
        if (owner == HandOwner)
        {
            _currentHand.AddRange(cards);
            _UpdateHand();
        }
    }

    private void _RemoveFromHand(string owner, Match.CardData cardData)
    {
        if (owner == HandOwner)
        {
            GD.Print("Remove ", cardData.Title(), " from " + owner + "'s hand");
            int cardIndex = _currentHand.IndexOf(cardData);
            Array<Node> cards = GetChildren();
            cards[cardIndex].QueueFree();
            _currentHand.Remove(cardData);

            _signalBus.EmitSignal("CardHandUpdated", HandOwner, _currentHand);
        }
    }

    private void _UpdateHand()
    {
        int totalCards = _currentHand.Count;

        for (int i = 0; i < totalCards; i++)
        {
            Match.Card cardInstance = (Match.Card)_card.Instantiate();
            cardInstance.CardInfo = _currentHand[i];
            cardInstance.CardOwner = HandOwner;
            cardInstance.CardIndex = i;
            cardInstance.CardPlayedDisplayPath = PlayedCardDisplay;
            AddChild(cardInstance);
        }

        _handMelds = GetHandMelds();
        _signalBus.EmitSignal("MeldListUpdate", HandOwner, _handMelds);
        _signalBus.EmitSignal("CardHandUpdated", HandOwner, _currentHand);
    }

    private Array<Match.MeldData> GetHandMelds()
    {
        Array<Match.MeldData> madeMelds = new Array<Match.MeldData>();

        foreach (Match.MeldData meldData in _allMelds)
        {
            bool isValidMeld = true;

            Godot.Collections.Dictionary<string, int> comboFrequency =
                new Godot.Collections.Dictionary<string, int>();

            foreach (Match.CardCombination cardCombination in meldData.CardCombinations)
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

                foreach (Match.CardData cardData in _currentHand)
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

                foreach (Match.CardData card in _currentHand)
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

    private static Array<Match.MeldData> LoadMelds()
    {
        string file = FileAccess.Open("res://Assets/json/melds.json", FileAccess.ModeFlags.Read).GetAsText();
        return JsonConvert.DeserializeObject<Array<Match.MeldData>>(file);
    }
}