using Godot;
using System;

public partial class Button : Area2D
{
    [Export]
    public string connectedObject;
    [Export]
    public string groupName;

    public int onButton = 0;

	public void OnButtonPressed(Node2D body)
	{
		if (body is Player or RigidBody2D)
		{
            if (onButton == 0)
            {
                GetNode<AudioStreamPlayer>("ButtonPress").Play();
            }
            onButton++;
			GetNode<Sprite2D>("Sprite2D").Visible = false;
		}
    }

    public void OnButtonReleased(Node2D body)
    {
        if (body is Player or RigidBody2D)
        {
            onButton--;
            if (onButton == 0)
            {
                GetNode<AudioStreamPlayer>("ButtonRelease").Play();
                GetNode<Sprite2D>("Sprite2D").Visible = true;
            }
        }
    }
}
