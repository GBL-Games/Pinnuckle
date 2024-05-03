using Godot;
using Pinnuckle.addons.ScenicRoute;
using Pinnuckle.Scripts.Player;

namespace Pinnuckle.Scripts.ClassSelect;

[GlobalClass]
public partial class ClassSelectItem : PanelContainer
{
    [Export] public string ClassName;

    private ScenicRoute _scenicRoute;

    private GameState _gameState = new();

    public override void _Ready()
    {
        _scenicRoute = GetNode<ScenicRoute>("/root/ScenicRoute");
        _gameState = (GameState)_scenicRoute.GetCurrentGameState();
    }

    private void _on_class_selected()
    {
        string selectedClass = GetNode<Label>("Label").Text;
        _gameState.CurrentPlayer.PlayerClass.Id = selectedClass.ToCamelCase();
        _gameState.CurrentPlayer.PlayerClass.Name = selectedClass;
        _gameState.CurrentPlayer.PlayerClass.Skills = [];

        _scenicRoute.LoadScene("match", _gameState);
    }
}