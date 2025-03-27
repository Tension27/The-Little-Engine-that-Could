using Godot;
using System;
using static Godot.TextServer;
using System.Text.RegularExpressions;

// ATTEMPT AT INHERITANCE FOR AN ENEMY, IT DID NOT WORK.




public partial class EnemyRoot : CharacterBody2D
{
    public float moveSpeed = 100f;
    [Export]
    public Vector2 direction;
    public bool canTurn = true;
    public bool bodyHit = false;
    public bool headHit = false;

    public override void _PhysicsProcess(double delta)
    {
        //Vector2 velocity = Velocity;

        //Velocity = velocity;
        //MoveAndSlide();
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            float realVelocity = player.Velocity.Y;
            bodyHit = true;
            Callable.From(() => ResolveFlagsAfterPhysicsOver(player, realVelocity)).CallDeferred();
        }
    }

    public void OnSquashAreaEntered(Node2D body)
    {
        if (body is Player player)
        {
            float realVelocity = player.Velocity.Y;
            headHit = true;
            Callable.From(() => ResolveFlagsAfterPhysicsOver(player, realVelocity)).CallDeferred();
        }
    }

    public void OnSquashTimeout()
    {
        QueueFree();
    }

    public void ResolveFlagsAfterPhysicsOver(Player player, float realVelocity)
    {
        if (headHit == true && realVelocity > 120)
        {
            player.Bounce();
            GetNode<Timer>("QueueFreeTimer").Start();
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").Visible = false;
            Node2D squashedSprite = GetNode<Sprite2D>("Squashed");
            Node2D normalSprite = GetNode<Sprite2D>("Sprite2D");
            normalSprite.Visible = false;
            squashedSprite.Visible = true;
            squashedSprite.SelfModulate = normalSprite.SelfModulate;
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
