#if TOOLS
using Godot;
using Godot.Collections;

namespace Pinnuckle.addons.ScenicRoute;

[Tool]
public partial class Plugin : EditorPlugin
{
    private Dictionary<string, SceneData> _scenes = new();
    private SceneData _currentScene;
    private ScenicRouteDock _dock;

    [Signal]
    public delegate void SceneListUpdatedEventHandler(Dictionary<string, SceneData> scenes);

    public override void _EnterTree()
    {
        AddAutoloadSingleton("ScenicRouteSignals", "res://addons/ScenicRoute/PluginSignals.cs");

        // Initialization of the plugin goes here.
        _dock = GD.Load<PackedScene>("res://addons/scenic_route/ScenicRouteDock.tscn").Instantiate<ScenicRouteDock>();
        AddControlToDock(DockSlot.RightUl, _dock);
    }

    private void _AddScene(string sceneKey, string scenePath, string sceneAlias)
    {
        SceneData sceneData = new SceneData();
        sceneData.Alias = sceneAlias;
        sceneData.Path = scenePath;
        _scenes.Add(sceneKey, sceneData);

        EmitSignal("SceneListUpdated", _scenes);
    }

    private void _RemoveScene(string sceneKey)
    {
        _scenes.Remove(sceneKey);
        EmitSignal("SceneListUpdated", _scenes);
    }

    private void _SwitchScene(string sceneKey)
    {
        if (_scenes[sceneKey] != null)
        {
            _currentScene = _scenes[sceneKey];
            GetTree().ChangeSceneToFile(_scenes[sceneKey].Path);
        }
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveAutoloadSingleton("ScenicRouteSignals");
        RemoveControlFromDocks(_dock);
        _dock.Free();
    }
}
#endif