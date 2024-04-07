using Godot;
using Godot.Collections;
using System.Linq;

public partial class HandManager : Node2D
{
	private SignalBus _signalBus;
	private Array<CardObject> _currentHand = [];

	private PackedScene _card = GD.Load<PackedScene>("res://Objects/Card.tscn");

	[Export]
	public Curve SpreadCurve;
	[Export]
	public Curve HeightCurve;
	[Export]
	public float CardSpacing;

	[Export]
	public string HandOwner = "player";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signalBus = GetNode<SignalBus>("/root/SignalBus");
		_signalBus.GiveCards += AddToHand;
	}

	private void AddToHand(string owner, Array<CardObject> cards)
	{
		if (owner == HandOwner)
		{
			_currentHand.AddRange(cards);
			UpdateHand();
		}
	}

	private void UpdateHand()
	{
		GD.Print(CardSpacing);
		int totalCards = _currentHand.Count;

		for (int i = 0; i < totalCards; i++)
		{
			Vector2 destination = Position;
			float tangentAngle;

			float handPositionRatio;
			float handRotationRatio;

			handPositionRatio = (float)i / (float)(totalCards - 1);
			handRotationRatio = (float)i / (float)totalCards;
			destination.X = SpreadCurve.Sample(handPositionRatio) * CardSpacing;
			destination.Y = HeightCurve.Sample(handPositionRatio) * -15f;

			float deltaX = SpreadCurve.Sample(handPositionRatio + 0.01f) * CardSpacing - destination.X;
			float deltaY = HeightCurve.Sample(handPositionRatio + 0.01f) * -15f - destination.Y;

			tangentAngle = Mathf.Atan2(deltaY, deltaX);

			if (totalCards == 1)
			{
				tangentAngle = 0f;
			}

			Node2D cardInstance = (Node2D)_card.Instantiate();
			cardInstance.Position = destination;
			cardInstance.Rotation = tangentAngle;


			GD.PrintT(_currentHand[i].Title(), tangentAngle);

			AddChild(cardInstance);
		}
	}
}
