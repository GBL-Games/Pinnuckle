using Godot;
using Godot.Collections;

namespace Pinnuckle.addons.ScenicRoute;

public partial class ScenicRouteManager : Node
{
    public Dictionary<string, SceneData> Scenes = new();

    private SceneData _currentScene;

    private VBoxContainer _sceneList;

    public override void _Ready()
    {
        GD.Print("Scenic Route Manager");

        base._Ready();
    }

    public void SwitchScene(string sceneKey)
    {
        if (Scenes[sceneKey] != null)
        {
            _currentScene = Scenes[sceneKey];
            GetTree().ChangeSceneToFile(Scenes[sceneKey].Path);
        }
    }

    public SceneData GetCurrentScene()
    {
        return _currentScene;
    }
}