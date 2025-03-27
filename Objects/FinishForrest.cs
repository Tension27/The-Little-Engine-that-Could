using Godot;
using System;

public partial class FinishForrest : Area2D
{
    [Export]
    private int coinsToFinish = 0;

    private int coins = 0;
    AnimationPlayer ap = null;
    private Signals customSignals;

    public override void _Ready()
    {
        ap = GetNode<AnimationPlayer>("AnimationPlayer");
        customSignals = GetNode<Signals>("/root/Signals");
        customSignals.OnDoorShouldBeOpen += OpenDoor;
        ap.Play("Dead");
    }

    public override void _ExitTree()
    {
        customSignals.OnDoorShouldBeOpen -= OpenDoor;
    }

    //checks to see if the player has enough coins to pass the level
    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            if (Player.coins == coinsToFinish)
            {
                customSignals.EmitSignal(nameof(Signals.OnFinishReached));
                Player.coins = 0;
            }
        }
    }

    public void OpenDoor()
    {
        coins++;
        if (coins == coinsToFinish)
        {
            ap.Play("Leaves");
        }
    }
}
