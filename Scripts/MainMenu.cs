using Godot;
using Pinnuckle.addons.ScenicRoute;

namespace Pinnuckle.Scripts;

public partial class MainMenu : Control
{
    private ScenicRoute _scenicRoute;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _scenicRoute = GetNode<ScenicRoute>("/root/ScenicRoute");

        GD.Print(_scenicRoute);
        _scenicRoute.PingManager();
        GD.Print(_scenicRoute.GetCurrentScene().Alias);
    }

    private void _on_start_button_up()
    {
        _scenicRoute.LoadScene("classselect");
    }
}