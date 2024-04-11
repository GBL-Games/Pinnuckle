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

        private void UpdateMeldList(Array<MeldData> meldItems)
        {
            GetNode<RichTextLabel>("MeldsList/Title").Text = "Melds: " + meldItems.Count.ToString();
            foreach (MeldData meldData in meldItems)
            {
                MeldListItem meldListItem = new MeldListItem();
                meldListItem.MeldListItemData = meldData;
                GetNode("MeldsList").AddChild(meldListItem);
            }

            GD.Print(GetNode("MeldsList").GetChildren().Count);
        }
    }
}