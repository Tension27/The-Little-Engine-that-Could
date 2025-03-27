using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Spike : Area2D
{
	public void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.AddDeath();
        }
    }
}
