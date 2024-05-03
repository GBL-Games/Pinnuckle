using Godot;
using Pinnuckle.addons.ScenicRoute;

namespace Pinnuckle.Scripts.ClassSelect;

[GlobalClass]
public partial class ClassSelectItem : PanelContainer
{
    [Export] public string ClassName;

    private ScenicRoute _scenicRoute;

    public override void _Ready()
    {
        _scenicRoute = GetNode<ScenicRoute>("/root/ScenicRoute");
    }
}