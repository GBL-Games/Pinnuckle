using Godot;
using Pinnuckle.addons.ScenicRoute;
using Pinnuckle.Scripts.Archetype;
using Pinnuckle.Scripts.Player;

namespace Pinnuckle.Scripts.ClassSelect;

[GlobalClass]
public partial class ClassSelectItem : PanelContainer
{
    [Export] public ArchetypeData Archetype;

    private ScenicRoute _scenicRoute;

    private GameState _gameState = new();

    public override void _Ready()
    {
        _scenicRoute = GetNode<ScenicRoute>("/root/ScenicRoute");
        _gameState = (GameState)_scenicRoute.GetCurrentGameState();

        GetNode<Label>("Label").Text = Archetype.Name;
    }

    private void _on_class_selected()
    {
        _gameState.CurrentPlayer = new PlayerData();
        _gameState.CurrentPlayer.Health = Archetype.Health;
        _gameState.CurrentPlayer.PlayerArchetype = Archetype;
        _scenicRoute.LoadScene("match", _gameState);
    }
}