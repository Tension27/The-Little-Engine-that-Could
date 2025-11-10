using Godot;
using System;
using System.Drawing;

public partial class Crab : CharacterBody2D
{
    public float moveSpeed = 100f;
    [Export]
    public int direction = -1;
    public bool canTurn = true;
    public bool bodyHit = false;
    public bool headHit = false;

    // get gravity from project settings 
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override async void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Flips the sprite and direction if the mob hits a wall or another sprite
        if ((IsOnWall() || IsOnBlock()) && canTurn == true)
        {
            canTurn = false;
            direction = direction * -1;

            //fixes a bug where the sprite would spaz out sometimes when it collided with something and it would just flip back and forth until it was bumped again
            await ToSignal(GetTree().CreateTimer(.05f), SceneTreeTimer.SignalName.Timeout);
            canTurn = true;
        }

        // Flips the sprite and direction if the mob reaches an edge
        if (GetNode<RayCast2D>("FloorChecker").IsColliding() == false && velocity.Y == 0)
        {
            direction = direction * -1;
        }

        // Applys gravity when goomba is off the floor
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
        }

        velocity.X = moveSpeed * direction;

        Velocity = velocity;
        MoveAndSlide();
    }

    // Makes it so Goombas treat blocks the same as floors or walls
    public bool IsOnBlock()
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);

            if (collision.GetCollider() is RigidBody2D rb)
            {
                return true;
            }
        }
        return false;
    }

    // Checks to see if the goomba touched the player
    public void OnAreaEntered(Node2D area)
    {
        if (area.GetParent() is Player player)
        {
            player.AddDeath();
        }
    }
}