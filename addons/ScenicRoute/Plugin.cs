#if TOOLS
using Godot;
using Godot.Collections;

namespace Pinnuckle.addons.ScenicRoute;

[Tool]
public partial class Plugin : EditorPlugin
{
    private Dictionary<string, SceneData> _scenes = new();
    private SceneData _currentScene;
    private Control _dock;

    public override void _EnterTree()
    {
        // Add Scenic Route Signal Bus
        AddAutoloadSingleton("ScenicRouteSignals", "res://addons/ScenicRoute/ScenicRouteSignals.cs");
        // Add Scenic Route Manager
        AddAutoloadSingleton("ScenicRouteManager", "res://addons/ScenicRoute/ScenicRouteManager.cs");

        // Initialization of the plugin goes here.

        ScenicRouteSignals scenicRouteSignals = GetNode<ScenicRouteSignals>("/root/ScenicRouteSignals");

        if (scenicRouteSignals == null) return;
        _dock = GD.Load<PackedScene>("res://addons/ScenicRoute/ScenicRouteDock.tscn").Instantiate<Control>();
        AddControlToDock(DockSlot.RightUl, _dock);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveAutoloadSingleton("ScenicRouteManager");
        RemoveAutoloadSingleton("ScenicRouteSignals");
        RemoveControlFromDocks(_dock);
        _dock.Free();
    }
}
#endif