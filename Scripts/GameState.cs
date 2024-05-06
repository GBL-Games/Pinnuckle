using Godot;
using Godot.Collections;
using Pinnuckle.Scripts.Match;
using Pinnuckle.Scripts.Player;

namespace Pinnuckle.Scripts;

[GlobalClass]
public partial class GameState : GodotObject
{
    public string CurrentScene { get; set; }
    public Array<CardData> CurrentDeck { get; set; }
    public PlayerData CurrentPlayer { get; set; }
}