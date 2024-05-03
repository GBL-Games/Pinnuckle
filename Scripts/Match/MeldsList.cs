using Godot;
using Godot.Collections;

namespace Pinnuckle.Scripts.Match;

public partial class MeldsList : HBoxContainer
{
    private Autoload.SignalBus _signalBus;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _signalBus = GetNode<Autoload.SignalBus>("/root/SignalBus");
        _signalBus.MeldListUpdate += UpdateMeldList;
    }

    private void UpdateMeldList(string listOwner, Array<Match.MeldData> meldItems)
    {
        bool isPlayer = listOwner == "player";
        NodePath titlePath = listOwner == "player" ? "UI Right/PlayerMeldsList/Title" : "OpponentMeldsList/Title";
        NodePath listPath = isPlayer ? "UI Right/PlayerMeldsList" : "OpponentMeldsList";
        string ownerPossessive = isPlayer ? "Your" : "Their";

        GetNode<RichTextLabel>(titlePath).Text =
            $"{ownerPossessive} Melds: {meldItems.Count}";

        foreach (Match.MeldData meldData in meldItems)
        {
            Match.MeldListItem meldListItem = new Match.MeldListItem();
            meldListItem.MeldListItemData = meldData;
            meldListItem.MeldListOwner = listOwner;
            GetNode(listPath).AddChild(meldListItem);
        }
    }
}