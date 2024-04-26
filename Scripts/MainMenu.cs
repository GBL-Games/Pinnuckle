using Godot;

namespace Pinnuckle.Scenes;

public partial class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    private void _on_start_button_up()
    {
        GD.Print("Start the game!");
    }
}