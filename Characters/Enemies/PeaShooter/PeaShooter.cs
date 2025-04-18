using Godot;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

public partial class PeaShooter : CharacterBody2D
{
    [Export]
    public PackedScene PeaScene { get; set; }
    [Export]
    public float moveSpeed = 100f;
    [Export]
    public  int direction = -1;
    [Export]
    private float fireRate = 2.4f;
    [Export]
    private bool noGravity = false;
    public bool canTurn = true;
    public bool headHit = false;
    public bool bodyHit = false;
    AnimationPlayer ap = null;
    Timer shotTimer = null;

    // get gravity from project settings 
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        ap = GetNode<AnimationPlayer>("AnimationPlayer");
        //Starts and assigns the fireate of whatever value is set locally on the peashooter. Keeps them from all starting out in sync. 
        shotTimer = GetNode<Timer>("ShotTimer");
        shotTimer.WaitTime = fireRate;
        shotTimer.Start();
        //Used to let the mob float if I need them to stay in the air
        if (noGravity == true)
            gravity = 0;
    }

    public override async void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Flips the sprite and direction if the mob hits a wall or another sprite
        if (IsOnWall() && canTurn == true)
        {
            canTurn = false;
            direction = direction * -1;
            FlipSprite(direction);

            await ToSignal(GetTree().CreateTimer(.05f), SceneTreeTimer.SignalName.Timeout);
            canTurn = true;
        }

        // Flips the sprite and direction if the mob reaches an edge
        if (GetNode<RayCast2D>("FloorChecker").IsColliding() == false && velocity.Y == 0)
        {
            if (noGravity == true)
                return;
            direction = direction * -1;

            FlipSprite(direction);
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

    //used to time the animation to the pea shooting
    public void OnAnimationFinished(string name)
    {
        if (name == "Shoot" && headHit == false)
        {
            ShootThePea();
            ap.Play("Walk");
        }
    }

    public void ShotTimerTimeout() 
    {
        shotTimer.Start();
        ap.Play("Shoot");
    }

    // Checks to see if the peashooter touched the player
    public void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            float realVelocity = player.Velocity.Y;
            bodyHit = true;
            Callable.From(() => ResolveFlagsAfterPhysicsOver(player, realVelocity)).CallDeferred();
        }
    }

    // Checks to see if the player jumped on the peashooters head
    public void OnSquashAreaEntered(Node body)
    {
        if (body is Player player)
        {
            float realVelocity = player.Velocity.Y;
            headHit = true;
            Callable.From(() => ResolveFlagsAfterPhysicsOver(player, realVelocity)).CallDeferred(); 
        }
    }

    // Instantiates and fires a pea in the direction that the peashooter is facing
    public void ShootThePea()
    {
        Pea pea = PeaScene.Instantiate<Pea>();
        
        var peaStart = new Vector2(-50 * direction * -1, -12);

        pea.Position = Position + peaStart;
        pea.direction = direction;

        AddSibling(pea);
    }

    // Deletes the mob if they have been jumped on
    public void OnSquashTimeout()
    {
        QueueFree();
    }

    // Flips the sprite
    public void FlipSprite(int direction)
    {
        if (direction == 1)
        {
            GetNode<Sprite2D>("Sprite2D").FlipH = false;
            if (GetNode<Sprite2D>("Squashed").Visible == true)
            {
                GetNode<Sprite2D>("Squashed").FlipH = false;
            }
        }
        else
        {
            GetNode<Sprite2D>("Sprite2D").FlipH = true;
            if (GetNode<Sprite2D>("Squashed").Visible == true)
            {
                GetNode<Sprite2D>("Squashed").FlipH = true;
            }
        }
    }

    // This was put in place to solve a bug where the player would collide with the hitbox and hurtbox in the same frame and
    // die instead of killing the peashooter.
    public void ResolveFlagsAfterPhysicsOver(Player player, float realVelocity)
    {
        if (headHit == true && realVelocity > 120) 
        {
            GetNode<Area2D>("Area2D").QueueFree();
            player.Bounce();
            GetNode<AudioStreamPlayer>("EnemySquashed").Play();
            GetNode<Timer>("QueueFreeTimer").Start();
            GetNode<Sprite2D>("Sprite2D").Visible = false;
            GetNode<Sprite2D>("Squashed").Visible = true;
            FlipSprite(direction);
            GetNode<Area2D>("SquashZone").QueueFree();
            GetNode<Timer>("Timer").QueueFree();
            moveSpeed = 0;
            return;
        }
        else if(bodyHit == true)
        {
            player.AddDeath();
        }

        bodyHit = false;
        headHit = false;
    }
}