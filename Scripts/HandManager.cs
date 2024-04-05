using Godot;
using System;
using System.Buffers;
using System.Linq;

public partial class HandManager : Node2D
{
	private SignalBus _signalBus;
	private CardObject[] _currentHand;

	private PackedScene _card = GD.Load<PackedScene>("res://Objects/Card.tscn");

	public Curve SpreadCurve;
	public float CardSpacing = 20f;
	public string HandOwner = "player";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signalBus = GetNode<SignalBus>("/root/SignalBus");
		_signalBus.GiveCards += AddToHand;
	}

	private void AddToHand(string owner, CardObject[] cards)
	{
		if (owner == HandOwner)
		{
			_currentHand.Concat(cards);
			UpdateHand();
		}
	}

	private void UpdateHand()
	{
		int totalCards = _currentHand.Length;

		foreach (var cardData in _currentHand)
		{
			float handPositionRatio = 0.5f;
			Vector2 destination = Position;
			destination.X = SpreadCurve.Sample(handPositionRatio) * CardSpacing;

			Node cardInstance = _card.Instantiate();

			AddChild(cardInstance);
		}
	}
}
