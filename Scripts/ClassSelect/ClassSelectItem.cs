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

    private void _on_class_selected()
    {
        GD.Print(GetNode<Label>("Label").Text);
    }
}