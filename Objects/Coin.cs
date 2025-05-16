using Godot;
using System;

public partial  class Coin : Area2D
{
    private Signals customSignals;

    public override void _Ready()
    {
        customSignals = GetNode<Signals>("/root/Signals");
    }

    private void OnBodyEntered2D(Node2D body)
    {
        if (body is Player player)
        {
            //Emits signal to tell the door that a coin has been collected
            customSignals.EmitSignal(nameof(Signals.OnDoorShouldBeOpen));
            //Calls a function in the player script that adds a coin
            player.OnCoinEntered();
            GetNode<AnimationPlayer>("AnimationPlayer").Play("CoinGrab");
            GetNode<AudioStreamPlayer>("Collection").Play();
            GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();
        }
	}

    private void OnAnimationFinished(string animName)
    {
        if (animName == "CoinGrab")
        {
            CoinReadyToBeDeleted();
        }
    }

    private void CoinReadyToBeDeleted()
    {
        QueueFree();
    }
}
