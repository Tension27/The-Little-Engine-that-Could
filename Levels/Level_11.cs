using Godot;
using System;

public partial class Level_11 : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node2D Keys = GetNode<Node2D>("Keys");
		Node2D Word_Arrows = GetNode<Node2D>("Word_Arrows");
		Node2D wordOR = GetNode<Sprite2D>("Word_Arrows/Or");
	}

    public override void _Input(InputEvent @event)
	{

	}
}