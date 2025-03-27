using Godot;
using System;

public partial class DoorThatNeedsKey : StaticBody2D
{
	public void OnBodyEntered(Node2D body)
	{
		//checks to see if the player has the correct key and deletes the door if they do.
		foreach (var key in Global.foundKeys)
		{
			if ($"{key}Door" == this.Name)
			{
				QueueFree();
			}
		}
	}
}
