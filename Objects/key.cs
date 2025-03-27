using Godot;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class key : Area2D
{
	//adds a key to the player when they walk into it
	public void OnBodyEntered(Node2D body)
	{
		Global.foundKeys.Add(Name);

		QueueFree();
	}
}
