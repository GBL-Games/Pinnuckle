#if TOOLS
using Godot;
using Godot.Collections;

namespace Pinnuckle.addons.ScenicRoute;

[Tool]
public partial class Plugin : EditorPlugin
{
    private Dictionary<string, SceneData> _scenes = new();
    private Control _dock;

    public override void _EnterTree()
    {
        _dock = GD.Load<PackedScene>("res://addons/ScenicRoute/ScenicRouteDock.tscn").Instantiate<Control>();
        AddAutoloadSingleton("ScenicRoute", "res://addons/ScenicRoute/ScenicRoute.cs");
        AddControlToDock(DockSlot.RightUl, _dock);
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton("ScenicRoute");
        RemoveControlFromDocks(_dock);
        if (_dock != null) _dock.Free();
    }
}
#endif