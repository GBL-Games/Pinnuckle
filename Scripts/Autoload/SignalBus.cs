using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts.Autoload;

public partial class SignalBus : Node
{
    // Deck Signals
    [Signal]
    public delegate void DealCardsEventHandler(int amount, string owner);

    [Signal]
    public delegate void GiveCardsEventHandler(string owner, Array<Match.CardData> cards);

    [Signal]
    public delegate void ShuffleCardsEventHandler();

    // Hand Signals
    [Signal]
    public delegate void DisplayMeldsEventHandler(string owner, Array<Match.MeldData> melds);

    [Signal]
    public delegate void CardSelectedEventHandler(Match.CardData cardData);

    [Signal]
    public delegate void CardPlayedEventHandler(string owner, Match.CardData cardData);

    [Signal]
    public delegate void CardHandUpdatedEventHandler(string owner, Array<Match.CardData> hand);

    // Player signals
    [Signal]
    public delegate void PlayerActionEventHandler(string action);

    [Signal]
    public delegate void PlayerHpChangeEventHandler(int amount);

    [Signal]
    public delegate void PlayerAtkChangeEventHandler(int amount);

    [Signal]
    public delegate void PlayerDefChangeEventHandler(int amount);

    [Signal]
    public delegate void PlayerMpChangeEventHandler(int amount);

    [Signal]
    public delegate void PlayerXpChangeEventHandler(int amount);

    // UI Signals
    [Signal]
    public delegate void MeldListUpdateEventHandler(string owner, Array<Match.MeldData> meldData);
}