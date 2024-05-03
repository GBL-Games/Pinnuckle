using System;
using Godot;
using Godot.Collections;
using Newtonsoft.Json;

namespace Pinnuckle.addons.ScenicRoute;

public partial class SceneData : GodotObject
{
    public string Path { get; set; }
    public string Alias { get; set; }
}

[GlobalClass, Tool]
public partial class ScenicRouteDock : Control
{
    private Plugin _plugin;
    private string _scenesPath = "res://addons/ScenicRoute/scenes.json";
    private Array<string> _scenePaths;
    private Dictionary<string, SceneData> _scenes;

    private PackedScene _listItem;
    private VBoxContainer _listContainer;

    public override void _EnterTree()
    {
        _listContainer = GetNode<VBoxContainer>("Wrapper/Scene List Container/Scene List");
        _listItem = ResourceLoader.Load<PackedScene>("res://addons/ScenicRoute/SceneListItem.tscn");

        _LoadSceneList();
    }

    private void _LoadSceneList()
    {
        string file = FileAccess.Open(_scenesPath, FileAccess.ModeFlags.ReadWrite).GetAsText();
        _scenes = JsonConvert.DeserializeObject<Dictionary<string, SceneData>>(file);

        _PopulateList();
    }

    #region Drag N Drop

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        Dictionary<string, Variant> dropData = data.AsGodotDictionary<string, Variant>();
        _scenePaths = (Array<string>)dropData["files"];
        return _scenePaths.Count > 0;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        foreach (string scenePath in _scenePaths)
        {
            string fileName = scenePath.Split(".tscn")[0];
            int lastSlashIndex = fileName.LastIndexOf("/", StringComparison.Ordinal);
            string sceneName = fileName.Substr(lastSlashIndex + 1, fileName.Length - 1);

            _AddScene(sceneName, scenePath);
        }

        _SaveSceneList();
    }

    #endregion

    #region Updating Scene List

    private void _AddScene(string sceneName, string scenePath)
    {
        SceneData sceneData = new SceneData();
        sceneData.Alias = sceneName;
        sceneData.Path = scenePath;

        _scenes[sceneName.ToLower()] = sceneData;
        SceneListItem listItemInstance = _listItem.Instantiate<SceneListItem>();
        listItemInstance.RemoveScene += _RemoveScene;
        _listContainer.AddChild(listItemInstance);
        listItemInstance.GetNode<Label>("Panel/Label").Text = sceneName;
    }

    private void _RemoveScene(string sceneKey)
    {
        string sceneAlias = _scenes[sceneKey].Alias;
        _scenes.Remove(sceneKey);

        foreach (SceneListItem child in _listContainer.GetChildren())
        {
            if (child.GetChild<Panel>(0).GetChild<Label>(0).Text == sceneAlias)
            {
                child.RemoveScene -= _RemoveScene;
                _listContainer.RemoveChild(child);
            }
        }

        _SaveSceneList();
    }

    private void _SaveSceneList()
    {
        using var file = FileAccess.Open("res://addons/ScenicRoute/scenes.json", FileAccess.ModeFlags.Write);
        file.StoreString(JsonConvert.SerializeObject(_scenes));
    }

    private void _PopulateList()
    {
        foreach (string sceneKey in _scenes.Keys)
        {
            SceneListItem listItemInstance = _listItem.Instantiate<SceneListItem>();
            listItemInstance.RemoveScene += _RemoveScene;
            _listContainer.AddChild(listItemInstance);
            listItemInstance.GetNode<Label>("Panel/Label").Text = _scenes[sceneKey].Alias;
        }
    }

    #endregion
}