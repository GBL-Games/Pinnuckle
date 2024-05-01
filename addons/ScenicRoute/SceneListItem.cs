#if TOOLS
using Godot;

namespace Pinnuckle.addons.ScenicRoute;

[Tool]
public partial class SceneListItem : HBoxContainer
{
    private ScenicRouteSignals _scenicRouteSignals;

    public override void _Ready()
    {
        _scenicRouteSignals = GetNode<ScenicRouteSignals>("/root/ScenicRouteSignals");
        GD.Print("List Item");
    }

    private void _on_remove_pressed()
    {
        string sceneKey = GetNode<Label>("Panel/Label").Text.ToLower();
        _scenicRouteSignals.EmitSignal("RemoveScene", sceneKey);
    }
}
#endif