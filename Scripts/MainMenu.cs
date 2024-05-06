using Godot;
using Pinnuckle.addons.ScenicRoute;
using Pinnuckle.Scripts.Player;

namespace Pinnuckle.Scripts;

public partial class MainMenu : Control
{
    private ScenicRoute _scenicRoute;
    private GameState _gameState = new();
    private PlayerData _playerState = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _scenicRoute = GetNode<ScenicRoute>("/root/ScenicRoute");
    }

    private void _InitGameState()
    {
        _gameState.CurrentScene = _scenicRoute.GetCurrentScene().Alias;
        _gameState.CurrentPlayer = _playerState;
    }

    private void _on_start_button_up()
    {
        _InitGameState();
        _scenicRoute.LoadScene("classselect", _gameState);
    }
}