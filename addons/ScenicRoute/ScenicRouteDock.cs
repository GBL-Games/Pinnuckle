using Godot;
using Godot.Collections;

namespace Pinnuckle.addons.ScenicRoute;

public partial class SceneData : GodotObject
{
    public string Path { get; set; }
    public string Alias { get; set; }
}

[GlobalClass]
public partial class ScenicRouteDock : Control
{
    private Dictionary<string, SceneData> _scenes;

    private PluginSignals _signals;

    public override void _Ready()
    {
        GD.Print("Taking the scenic route");
        _signals = GetNode<PluginSignals>("/root/ScenicRouteSignals");
        _signals.SceneListUpdated += _UpdateSceneList;
        base._Ready();
    }

    private void _UpdateSceneList(Dictionary<string, SceneData> scenes)
    {
        _scenes = scenes;
        _LoadScenes();
    }

    private void _LoadScenes()
    {
        foreach (string scenesKey in _scenes.Keys)
        {
            Label sceneItem = new Label();
            sceneItem.Text = _scenes[scenesKey].Alias;
            AddChild(sceneItem);
        }
    }
}