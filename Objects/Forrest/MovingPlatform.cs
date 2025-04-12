using Godot;
using System;

//The majority of this method was taken from a youtube video I have very poor understanding of how it actually works

public partial class MovingPlatform : AnimatableBody2D
{
    [Export(PropertyHint.Range, "0.0, 1.0")] float phaseOffset;
    [Export] Vector2 distance;
    [Export] float phaseTime = 6.0f;
    [Export] bool movement = true;
    [Export] bool debug = true;

    Vector2 pivot;
    float time;
    private int onButton = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        pivot = GlobalPosition;
        SetProcess(debug);
	}
    
    public Vector2 GetPosition(float time)
    {
        Vector2 vector;

        float x = pivot.X + (float)Math.Cos(Math.Tau * (time + phaseOffset)) * distance.X;
        float y = pivot.Y + (float)Math.Sin(Math.Tau * time) * distance.Y;

        vector.X = x;
        vector.Y = y;

        return vector;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (movement == true)
        {
            time = (float)(time + delta / phaseTime) % 1.0f;
            GlobalPosition = GetPosition(time); 
        }
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    //when you turn on debug mode this draws a path the platform follows so it's easier to get it where you want it
    public override void _Draw()
    {
        if (debug == false)
        {
            return;
        }

        int resolution = 20;
        float increments = 1.0f/resolution;
        for (int i = 0; i < resolution; i++)
        {
            Color white = new Color(1, 1, 1, 1);

            var a = GetPosition(increments * i) - GlobalPosition;
            var b = GetPosition(increments * (i + 1)) - GlobalPosition;
            DrawLine(a, b, white, -1);
        }

    }

    //moves the platform when a button is pressed
    public void OnButtonPressed(Node2D body)
    {
        if (body is CharacterBody2D || body is RigidBody2D)
        {
            onButton++;
            movement = true;
        }
    }

    //stops the platform when a button is released
    public void OnButtonReleased(Node2D body)
    {
        if (body is CharacterBody2D || body is RigidBody2D)
        {
            onButton--;
            if (onButton == 0)
            {
                movement = false;
            }
        }
    }

    //turns on the platform movement
    public void LeverOn(Node2D body)
    {
        movement = true;
    }

    //turns off the platform movement
    public void LeverOff(Node2D body)
    {
        movement = false;
    }
}