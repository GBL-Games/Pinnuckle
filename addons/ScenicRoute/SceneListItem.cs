#if TOOLS
using Godot;

namespace Pinnuckle.addons.ScenicRoute;

[GlobalClass, Tool]
public partial class SceneListItem : HBoxContainer
{
    [Signal]
    public delegate void RemoveSceneEventHandler(string sceneKey);

    private void _on_remove_pressed()
    {
        string sceneKey = GetNode<Label>("Panel/Label").Text.ToLower();
        EmitSignal(nameof(RemoveScene), sceneKey);
    }
}
#endif