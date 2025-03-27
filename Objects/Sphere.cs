using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Sphere : RigidBody2D
{
    private Vector2 oldVelocity = Vector2.Zero;
    private Vector2 newVelocity;

    private bool ballIsOnSurface;
    private bool currentlyRolling;
    private double stopTime = 0.0f; // Time that the object has been stopped
    private const float STOP_THRESHOLD = 30f; // Small threshold for considering "stopped"
    private const float TIME_THRESHOLD = .1f; // Time threshold (in seconds)
    private int onSurface = 0;
    private AudioStreamPlayer rolling;
    private AudioStreamPlayer impact;
    RigidBody2D rb;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        rolling = GetNode<AudioStreamPlayer>("Rolling");
        impact = GetNode<AudioStreamPlayer>("Impact");
        rb = this;
    }
    
    public override void _Process(double delta)
    {
        newVelocity = LinearVelocity;
        if (MathF.Abs(newVelocity.X - oldVelocity.X) > 200  || MathF.Abs(newVelocity.Y - oldVelocity.Y) > 200)
        {
            impact.Play();
        }
        oldVelocity = newVelocity;

        rolling.VolumeDb = LinearToDb(rb.LinearVelocity.Length());
        if (onSurface > 0)
        {
            // Check if the object is stopped (velocity magnitude below the threshold)
            if (rb.LinearVelocity.Length() < STOP_THRESHOLD)
            {
                // Object is stopped
                stopTime += delta; // Increment the stop time by the frame delta time

                if (stopTime >= TIME_THRESHOLD)
                {
                    // Trigger the action after 2 seconds of being stopped
                    currentlyRolling = false;
                    rolling.Stop();
                }
            }
            else
            {
                currentlyRolling = true;
                // Reset stop time if the object is no longer stopped
                stopTime = 0.0f;
            }

            if (currentlyRolling == true && rolling.Playing == false)
            {
                rolling.Play();
            } 
        }
        else
        {
            rolling.Stop();
        }
    }

    public void BallIsOnSurface(Node2D body)
    {
        
        if (body is TileMap ||
            body is StaticBody2D ||
            body is RigidBody2D ||
            body is CollisionShape2D)
        {
            ballIsOnSurface = true;
            onSurface++;
        }
    }

    public void BallIsOffSurface(Node2D body)
    {
       
        if (body is TileMap ||
            body is StaticBody2D ||
            body is RigidBody2D ||
            body is CollisionShape2D)
        {
            ballIsOnSurface = false;
            onSurface--;
        }
    }

    private float LinearToDb(float linearValue)
    {
        if (linearValue <= 0.0f)
            return -80.0f; // Minimum volume in dB (silent)
        return (linearValue / 100)-5; //Mathf.Log(linearValue) / 10;
    }
}
