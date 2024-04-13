using Godot;
using Godot.Collections;
using Array = System.Array;

namespace Pinnuckle.Scripts
{
    public partial class MeldsList : HBoxContainer
    {
        private SignalBus _signalBus;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _signalBus = GetNode<SignalBus>("/root/SignalBus");
            _signalBus.MeldListUpdate += UpdateMeldList;
        }

        private void UpdateMeldList(string listOwner, Array<MeldData> meldItems)
        {
            bool isPlayer = listOwner == "player";
            NodePath titlePath = listOwner == "player" ? "UI Right/PlayerMeldsList/Title" : "OpponentMeldsList/Title";
            NodePath listPath = isPlayer ? "UI Right/PlayerMeldsList" : "OpponentMeldsList";
            string ownerPossessive = isPlayer ? "Your" : "Their";

            GetNode<RichTextLabel>(titlePath).Text =
                $"{ownerPossessive} Melds: {meldItems.Count}";

            foreach (MeldData meldData in meldItems)
            {
                MeldListItem meldListItem = new MeldListItem();
                meldListItem.MeldListItemData = meldData;
                meldListItem.MeldListOwner = listOwner;
                GetNode(listPath).AddChild(meldListItem);
            }
        }
    }
}