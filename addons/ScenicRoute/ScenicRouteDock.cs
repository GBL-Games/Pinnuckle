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
    private string _scenesPath = "res://addons/ScenicRoute/scenes.json";
    private Array<string> _scenePaths;
    private Dictionary<string, SceneData> _scenes;

    private PackedScene _listItem;

    public override void _Ready()
    {
        GD.Print("Scenic Route Dock");
        _LoadSceneList();
        _RefreshSceneList();
    }

    private void _LoadSceneList()
    {
        string file = FileAccess.Open(_scenesPath, FileAccess.ModeFlags.ReadWrite).GetAsText();
        _scenes = JsonConvert.DeserializeObject<Dictionary<string, SceneData>>(file);
    }

    #region Drag N Drop

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        GD.Print(atPosition, data.ToString());
        Dictionary<string, Variant> dropData = data.AsGodotDictionary<string, Variant>();
        _scenePaths = (Array<string>)dropData["files"];
        GD.Print(dropData.ToString());
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
        _RefreshSceneList();

        base._DropData(atPosition, data);
    }

    #endregion

    #region Updating Scene List

    private void _AddScene(string sceneName, string scenePath)
    {
        SceneData sceneData = new SceneData();
        sceneData.Alias = sceneName;
        sceneData.Path = scenePath;

        _scenes[sceneName.ToLower()] = sceneData;
    }

    private void _SaveSceneList()
    {
        using var file = FileAccess.Open("res://addons/ScenicRoute/scenes.json", FileAccess.ModeFlags.Write);
        file.StoreString(JsonConvert.SerializeObject(_scenes));
    }

    private void _RefreshSceneList()
    {
        VBoxContainer listContainer = GetNode<VBoxContainer>("Wrapper/Scene List Container/Scene List");

        if (listContainer.GetChildCount() > 0)
        {
            foreach (Node listItem in listContainer.GetChildren())
            {
                listItem.Free();
            }
        }

        if (listContainer.GetChildCount() == 0)
        {
            _PopulateList(listContainer);
        }
    }

    private void _PopulateList(VBoxContainer listContainer)
    {
        foreach (string sceneKey in _scenes.Keys)
        {
            SceneListItem listItemInstance = new SceneListItem();
            listItemInstance.LabelText = _scenes[sceneKey].Alias;
            listContainer.AddChild(listItemInstance);
        }
    }

    #endregion
}