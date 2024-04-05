using System;
using System.Runtime;
using Godot;

public partial class SignalBus : Node
{
  // Deck Signals
  [Signal]
  public delegate void DealCardsEventHandler(int amount, string owner);
  [Signal]
  public delegate void GiveCardsEventHandler(string owner, CardObject[] cards);
  [Signal]
  public delegate void ShuffleCardsEventHandler();
}