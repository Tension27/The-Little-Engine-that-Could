using Godot;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

public partial class Pea : CharacterBody2D
{
	public float moveSpeed =  600f;
	public int direction = -1;
	public int pushForce = 40;
	private bool canSpin = true;
    AnimationPlayer ap = null;

    //Kill the player if it collides with them
    public void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.AddDeath();
		}
    }
	
	public override void _Ready()
	{
        ap = GetNode<AnimationPlayer>("AnimationPlayer");

        Vector2 velocity = Velocity;

		velocity.X = moveSpeed * direction;

		Velocity = velocity;
	}

    public override void _PhysicsProcess(double delta)
    {
		if (direction == -1 && canSpin == true)
		{
            RotationDegrees -= 3;
        }
		else if(canSpin == true)
		{
			RotationDegrees += 3;
		}
        MoveAndSlide();

		//applies force to interactable objects in the environment
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var c = GetSlideCollision(i);
            if (c.GetCollider() is RigidBody2D rb)
            {
                rb.ApplyImpulse(-c.GetNormal() * pushForce);
            }
        }
    }

	//breaks the pea if it hits something
	public void OnWallEntered(Node2D body)
	{
		if (body is TileMap || 
			body is StaticBody2D ||
			body is RigidBody2D)
		{
			moveSpeed = 0;
            canSpin = false;
            RotationDegrees = 0;
            GetNode<CollisionShape2D>("Area2D/CollisionShape2D").QueueFree();
            GetNode<Sprite2D>("Sprite2D").Visible = false;
            GetNode<Sprite2D>("PeaPop").Visible = true;
            ap.Play("Pop");
        }
	}

	//delete the pea after the pop animation is done
	public void OnAnimationFinished(Node2D node)
	{
		QueueFree();
	}
}