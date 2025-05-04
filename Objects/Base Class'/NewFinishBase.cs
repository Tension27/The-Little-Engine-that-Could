using Godot;
using Godot.NativeInterop;
using System;

public partial class NewFinishBase : Area2D
{
    [Export]
    private int coinsToFinish = 0;

    private int coins = 0;
    public AnimationPlayer ap = null;
    private Signals customSignals;
    public string OpenDoorAnimation;
    public string CloseDoorAnimation;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        ap = GetNode<AnimationPlayer>("AnimationPlayer");
        customSignals = GetNode<Signals>("/root/Signals");
        customSignals.OnDoorShouldBeOpen += OpenDoor;
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
                player.TestAnim();
                //customSignals.EmitSignal(nameof(Signals.OnFinishReached));
                //Player.coins = 0;
            }
        }
    }

    public void OpenDoor()
    {
        coins++;
        if (coins == coinsToFinish)
        {
            ap.Play(OpenDoorAnimation);
        }
    }
}
