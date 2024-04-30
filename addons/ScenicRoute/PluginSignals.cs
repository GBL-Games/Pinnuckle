using Godot;
using Godot.Collections;

namespace Pinnuckle.addons.ScenicRoute;

public partial class PluginSignals : Node
{
    [Signal]
    public delegate void SceneListUpdatedEventHandler(Dictionary<string, SceneData> scenes);

    public override void _Ready()
    {
        GD.Print("Plugin Signals!");
    }
}