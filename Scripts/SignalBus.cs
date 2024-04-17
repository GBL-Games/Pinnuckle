using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts
{
    public partial class SignalBus : Node
    {
        // Deck Signals
        [Signal]
        public delegate void DealCardsEventHandler(int amount, string owner);

        [Signal]
        public delegate void GiveCardsEventHandler(string owner, Array<CardData> cards);

        [Signal]
        public delegate void ShuffleCardsEventHandler();

        // Hand Signals
        [Signal]
        public delegate void DisplayMeldsEventHandler(string owner, Array<MeldData> melds);

        [Signal]
        public delegate void CardDroppedEventHandler(CardData cardData, int cardIndex);

        // Player signals
        [Signal]
        public delegate void PlayerActionEventHandler(string action);

        [Signal]
        public delegate void PlayerHpChangeEventHandler(int amount);

        [Signal]
        public delegate void PlayerMpChangeEventHandler(int amount);

        [Signal]
        public delegate void PlayerXpChangeEventHandler(int amount);

        // UI Signals
        [Signal]
        public delegate void MeldListUpdateEventHandler(string owner, Array<MeldData> meldData);
    }
}