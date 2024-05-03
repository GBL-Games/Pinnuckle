using System;
using System.Linq;
using Godot;
using Godot.Collections;
using Newtonsoft.Json;

namespace Pinnuckle.addons.ScenicRoute;

public partial class ScenicRoute : Node
{
    private string _scenesPath = "res://addons/ScenicRoute/scenes.json";
    private Dictionary<string, SceneData> _scenes = new();
    private SceneData _currentScene;
    private VBoxContainer _sceneList;

    private Variant? _gameState;

    public override void _Ready()
    {
        GD.Print("Scenic Route Manager");

        _LoadSceneList();
        base._Ready();
    }

    public void LoadScene(string sceneKey, Variant? gameState)
    {
        if (gameState != null) _gameState = gameState;

        if (_scenes[sceneKey] != null)
        {
            _currentScene = _scenes[sceneKey];
            GetTree().ChangeSceneToFile(_scenes[sceneKey].Path);
        }
    }

    public void PingManager()
    {
        GD.Print("We in the manager");
    }

    public SceneData GetCurrentScene()
    {
        return _currentScene;
    }

    public Variant? GetCurrentGameState()
    {
        return _gameState;
    }

    public Dictionary<string, SceneData> GetScenes()
    {
        return _scenes;
    }

    private void _LoadSceneList()
    {
        string file = FileAccess.Open(_scenesPath, FileAccess.ModeFlags.ReadWrite).GetAsText();
        _scenes = JsonConvert.DeserializeObject<Dictionary<string, SceneData>>(file);
        _currentScene = _scenes[_scenes.Keys.First()];
    }
}