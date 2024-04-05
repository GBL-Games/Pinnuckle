using System.Linq;
using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.IO;

public partial class Deck : Node2D
{
	private List<CardObject> _localDeck = [];
	private SignalBus _signalBus;

	public Array<CardObject> ShuffledDeck = [];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signalBus = GetNode<SignalBus>("/root/SignalBus");

		GD.Print(LoadJsonFromFile("res://Assets/json/", "cards.json"));

		// _localDeck = (List<CardObject>)CardData.Data;
		_signalBus.ShuffleCards += ShuffleCards;
		// _signalBus.DealCards += DealCards;
	}

	private void ShuffleCards()
	{
		GD.Print("Shuffling cards!");
		Array<CardObject> shuffledCards = [];

		// if (CardData != null)
		// {
		// 	// ShuffledDeck = _localDeck;

		// 	foreach (CardObject cardData in ShuffledDeck)
		// 	{
		// 		GD.PrintS(cardData);
		// 		// GD.Print(cardData.Title());
		// 	}
		// }

		// GD.Print(jsonParsed);
		/* 		Dictionary cardDictionary = (Dictionary)Json.ParseString(jsonParsed);
				GD.Print(cardDictionary.Count);
				foreach (var cardData in cardDictionary)
				{
					CardObject cardObj = new CardObject();
					shuffledCards.Append(cardObj);
					GD.Print(cardData.ToString());
				} */
	}

	private string LoadJsonFromFile(string path, string fileName)
	{
		string data;

		path = Path.Join(path, fileName);

		if (!File.Exists(path)) return null;

		try
		{
			data = File.ReadAllText(path);
		}
		catch (System.Exception e)
		{
			GD.PrintErr(e);
			throw;
		}

		return data;
	}
}
