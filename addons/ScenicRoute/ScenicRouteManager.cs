using Godot;
using Godot.Collections;
using Newtonsoft.Json;

namespace Pinnuckle.addons.ScenicRoute;

public partial class ScenicRouteManager : Node
{
    private string _scenesPath = "res://addons/ScenicRoute/scenes.json";
    private Dictionary<string, SceneData> _scenes = new();
    private SceneData _currentScene;
    private VBoxContainer _sceneList;

    public override void _Ready()
    {
        GD.Print("Scenic Route Manager");

        base._Ready();
    }

    public void SwitchCurrentScene(string sceneKey)
    {
        if (_scenes[sceneKey] != null)
        {
            _currentScene = _scenes[sceneKey];
            GetTree().ChangeSceneToFile(_scenes[sceneKey].Path);
        }
    }

    public SceneData GetCurrentScene()
    {
        return _currentScene;
    }

    public Dictionary<string, SceneData> GetScenes()
    {
        return _scenes;
    }

    private void _LoadSceneList()
    {
        string file = FileAccess.Open(_scenesPath, FileAccess.ModeFlags.ReadWrite).GetAsText();
        _scenes = JsonConvert.DeserializeObject<Dictionary<string, SceneData>>(file);
    }
}