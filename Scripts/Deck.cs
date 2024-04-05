using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using System;


public partial class Deck : Node2D
{
	private Array<CardObject> _localDeck = [];
	private SignalBus _signalBus;

	public Array<CardObject> ShuffledDeck = [];

	static Random rand = new Random();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signalBus = GetNode<SignalBus>("/root/SignalBus");
		_localDeck = LoadCardData();
		ShuffledDeck = _localDeck;
		_signalBus.ShuffleCards += ShuffleCards;
		_signalBus.DealCards += DealCards;
	}

	// Uses the FisherYates algorithm to shuffle.
	private void ShuffleCards()
	{
		for (int i = 0; i < ShuffledDeck.Count; i++)
		{
			int j = rand.Next(i + 1); ;
			CardObject tempCard = ShuffledDeck[i];
			ShuffledDeck[i] = ShuffledDeck[j];
			ShuffledDeck[j] = tempCard;
		}
	}


	private void DealCards(int amount, string cardsOwner)
	{
		Array<CardObject> cards = ShuffledDeck[..amount];
		ShuffledDeck = ShuffledDeck.Slice(amount, ShuffledDeck.Count);
		GD.Print(cards.Count);
		GD.Print(ShuffledDeck.Count);
	}

	private void ResetShuffleDeck()
	{
		ShuffledDeck = _localDeck;
		ShuffleCards();
	}

	private Array<CardObject> LoadCardData()
	{
		string file = FileAccess.Open("res://Assets/json/cards.json", FileAccess.ModeFlags.Read).GetAsText();
		return JsonConvert.DeserializeObject<Array<CardObject>>(file);
	}
}


