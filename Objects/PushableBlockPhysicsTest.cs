using Godot;
using System;

public partial class PushableBlockPhysicsTest : RigidBody2D
{
    private bool currentlyFalling;
    private int onSurface = 0;
    private bool boxIsOnSurface;
    private AudioStreamPlayer falling;
    RigidBody2D rb;


    public override void _Ready()
    {
        rb = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        float temp = rb.LinearVelocity.X + rb.LinearVelocity.Y;
    }

    public void BoxIsOnSurface(Node2D body)
    {
        if (body is TileMap ||
            body is StaticBody2D ||
            body is RigidBody2D ||
            body is CollisionShape2D)
        {
            boxIsOnSurface = true;
            onSurface++;
        }
    }

    public void BoxIsOffSurface(Node2D body)
    {
        if (body is TileMap ||
            body is StaticBody2D ||
            body is RigidBody2D ||
            body is CollisionShape2D)
        {
            boxIsOnSurface = false;
            onSurface--;
        }
    }

    //NOT WORKING RIGHT//

    private float LinearToDb(float linearValue)
    {
        if (linearValue <= 0.0f)
            return -80.0f; // Minimum volume in dB (silent)
        return (linearValue / 300) - 10; //Mathf.Log(linearValue) / 10;
    }
}