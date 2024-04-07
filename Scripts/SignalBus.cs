using Godot;
using Godot.Collections;

public partial class SignalBus : Node
{
  // Deck Signals
  [Signal]
  public delegate void DealCardsEventHandler(int amount, string owner);
  [Signal]
  public delegate void GiveCardsEventHandler(string owner, Array<CardObject> cards);
  [Signal]
  public delegate void ShuffleCardsEventHandler();
}