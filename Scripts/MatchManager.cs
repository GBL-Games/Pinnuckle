using Godot;

public partial class MatchManager : Node2D
{
  private SignalBus _signalBus;
  public override void _Ready()
  {
    _signalBus = GetNode<SignalBus>("/root/SignalBus");

    GD.Print("Match Started!");

    _signalBus.EmitSignal("ShuffleCards");
    _signalBus.EmitSignal("DealCards", 12, "player");
  }
}