using Godot;
using System;

public partial class GameManager : Node
{
    [Export] public string CurrentScene = "mainMenu";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Welcome to Pinnuckle. Let's get to moidering!");
    }
}