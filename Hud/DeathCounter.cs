using Godot;
using System;

public partial class DeathCounter : Label
{
	public override void _Process(double delta)
	{
		GD.Print("deathCounter");
		Text = Player.deaths.ToString();
	}
}
