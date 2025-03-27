using Godot;
using System;

public partial class Lever : Area2D
{
    public void OnLeverFlipped(Node2D area)
	{
		if (area is RigidBody2D)
		{
            GetNode<AudioStreamPlayer>("LeverFlipped").Play();
        }
    }
}
