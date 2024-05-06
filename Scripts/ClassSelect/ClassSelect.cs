using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using Pinnuckle.Scripts.Archetype;

namespace Pinnuckle.Scripts.ClassSelect;

public partial class ClassSelect : Control
{
    private Dictionary<string, ArchetypeData> _archetypes;
    private PackedScene _classSelectItem;

    public override void _Ready()
    {
        _classSelectItem = ResourceLoader.Load<PackedScene>("res://Objects/ClassSelectItem.tscn");
        _LoadArchetypes();
        _RenderArchetypesGrid();
    }

    private void _LoadArchetypes()
    {
        string file = FileAccess.Open("res://Assets/json/archetypes.json", FileAccess.ModeFlags.ReadWrite).GetAsText();
        _archetypes = JsonConvert.DeserializeObject<Dictionary<string, ArchetypeData>>(file);
    }

    private void _RenderArchetypesGrid()
    {
        foreach (string key in _archetypes.Keys)
        {
            ClassSelectItem selectItem = _classSelectItem.Instantiate<ClassSelectItem>();
            selectItem.Archetype = _archetypes[key];
            GetNode<GridContainer>("Class Grid").AddChild(selectItem);
        }
    }
}