using Godot;
using System;
using System.Drawing;

public partial class Goomba : CharacterBody2D
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
    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            float realVelocity = player.Velocity.Y;
            bodyHit = true;
            Callable.From(() => ResolveFlagsAfterPhysicsOver(player, realVelocity)).CallDeferred(); 
        }
    }

    // Checks to see if the player jumped on the goombas head
    public void OnSquashAreaEntered(Node2D body)
    {
        if (body is Player player)
        {
            float realVelocity = player.Velocity.Y;
            headHit = true;
            Callable.From(() => ResolveFlagsAfterPhysicsOver(player, realVelocity)).CallDeferred(); 
        }
    }

    // Deletes the mob if they have been jumped on
    public void OnSquashTimeout()
    {
        QueueFree();
    }

    // This was put in place to solve a bug where the player would collide with the hitbox and hurtbox in the same frame and
    // die instead of killing the goomba.
    public void ResolveFlagsAfterPhysicsOver(Player player, float realVelocity)
    {
        if (headHit == true && realVelocity > 120)
        {
            player.Bounce();
            GetNode<AudioStreamPlayer>("EnemySquashed").Play();
            GetNode<Timer>("QueueFreeTimer").Start();
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").Visible = false;
            GetNode<Sprite2D>("Squashed").Visible = true;
            GetNode<Area2D>("Area2D").QueueFree();
            GetNode<Area2D>("SquashZone").QueueFree();
            moveSpeed = 0;
            return;
        }
        else if (bodyHit == true)
        {
            player.AddDeath();
        }

        bodyHit = false;
        headHit = false;
    }
}
