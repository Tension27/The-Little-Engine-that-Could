using Godot;
using System;

public partial class ZombieNew : CharacterBody2D
{
    public float moveSpeed = 100f;
    [Export]
    public int direction = -1;
    public bool canTurn = true;
    public bool bodyHit = false;
    public bool headHit = false;
    public bool playerIsNear = false;
    private float playerLastPosition;
    private bool stopMoving;
    private bool zombieIsDead = false;
    AnimationPlayer ap = null;

    // get gravity from project settings 
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        ap = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override async void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        
        // If the player leaves the search zone the zombie slows back down to walking speed
        if (playerIsNear == false && zombieIsDead == false)
        {
            moveSpeed = 100f;
        }

        // Flips the sprite and direction if the mob hits a wall or another sprite
        if ((IsOnWall() || IsOnBlock()) && canTurn && !playerIsNear && !zombieIsDead == true)
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
            direction = direction * -1;
            FlipSprite(direction);
        }

        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
        }

        //Chase logic
        if (playerIsNear == true)
        {
            //speeds up the zombie
            if (zombieIsDead == false)
            {
                moveSpeed = 350f;
            }
            
            CharacterBody2D player = GetParent().GetParent().GetParent().GetParent().GetChild<CharacterBody2D>(1);
            //checks to see if the zombie is under the player
            stopMoving = ZombieisUnderPlayer(player);

            //If the zombie is not under the player it plays the chase animation and moves toward the player
            if (stopMoving == false)
            {
                if (zombieIsDead == false)
                {
                    ap.Play("Chase");
                }
                if (player.Position.X > Position.X)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }
                FlipSprite(direction); 
            }
            //If the zombie is under the player it stops moving and plays the lookup animation
            else
            {
                if (direction == 1)
                {
                    //keeps the animation from playing after the zombie has been killed
                    if (zombieIsDead == false)
                    {
                        ap.Play("LookUp");
                        GetNode<Sprite2D>("WalkSS").FlipH = true;
                    }
                }
                else
                {
                    //keeps the animation from playing after the zombie has been killed
                    if (zombieIsDead == false)
                    {
                        ap.Play("LookUp");
                        GetNode<Sprite2D>("WalkSS").FlipH = false;
                    }
                }
                moveSpeed = 0f;
            }
        }
        velocity.X = moveSpeed * direction;

        Velocity = velocity;
        MoveAndSlide(); 
    }

    //checks when the zombie is directly under the player
    public bool ZombieisUnderPlayer(CharacterBody2D player)
    {
        if (Math.Abs(Position.X - player.Position.X) <= 3)
        {
            return true;
        }

        return false;
    }

    // Triggers when the player enters the zombies search area
    public void PlayerCheckerEntered(Node2D area)
    {
        if (area.Name == "Area2D2" && area.GetParent() is Player player && zombieIsDead == false)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            int randomNumber = rng.RandiRange(1,3);

            switch (randomNumber)
            {
                case 1:
                    GetNode<AudioStreamPlayer>("Chase1").Play();
                    break;
                case 2:
                    GetNode<AudioStreamPlayer>("Chase2").Play();
                    break;
                case 3:
                    GetNode<AudioStreamPlayer>("Chase3").Play();
                    break;
                default:
                    GD.Print("Unexpected Case");
                    break;
            }

            playerIsNear = true;
            ap.Play("Chase");
        }
    }

    // Triggers when the player exits the zombies search area
    public void PlayerCheckerExited(Node2D area)
    {
        if (area.Name == "Area2D2" && area.GetParent() is Player player && zombieIsDead == false)
        {
            playerIsNear = false;
            ap.Play("Walk");
        }
    }

    // Makes it so zombies treat blocks the same as floors or walls
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

    // Checks to see if the zombie touched the player
    public void OnAreaEntered(Node2D area)
    {
        if (area.GetParent() is Player player)
        {
            if (player.Velocity.Y > 2500)
            {
                float realVelocity = player.Velocity.Y;
                headHit = true;
                Callable.From(() => ResolveFlagsAfterPhysicsOver(player, realVelocity)).CallDeferred();
            }
            else if (area.Name == "Area2D2")
            {
                float realVelocity = player.Velocity.Y;
                bodyHit = true;
                Callable.From(() => ResolveFlagsAfterPhysicsOver(player, realVelocity)).CallDeferred();
            }
        }
    }

    // Checks to see if the player jumped on the zombies head
    public void OnSquashAreaEntered(Node body)
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

    // Flips the sprite
    public void FlipSprite(int direction)
    {
        if (direction == -1)
        {
            GetNode<Sprite2D>("WalkSS").FlipH = false;
            if (GetNode<Sprite2D>("Squashed").Visible == true)
            {
                GetNode<Sprite2D>("Squashed").FlipH = false;
            }
        }
        else
        {
            GetNode<Sprite2D>("WalkSS").FlipH = true;
            if (GetNode<Sprite2D>("Squashed").Visible == true)
            {
                GetNode<Sprite2D>("Squashed").FlipH = true;
            }
        }
    }

    // This was put in place to solve a bug where the player would collide with the hitbox and hurtbox in the same frame and
    // die instead of killing the zombie.
    public void ResolveFlagsAfterPhysicsOver(Player player, float realVelocity)
    {
        if (headHit == true && realVelocity > 120)
        {
            player.Bounce();
            GetNode<AudioStreamPlayer>("EnemySquashed").Play();
            zombieIsDead = true;
            GetNode<Timer>("QueueFreeTimer").Start();
            GetNode<Sprite2D>("WalkSS").Visible = false;
            GetNode<Sprite2D>("Squashed").Visible = true;
            FlipSprite(direction);
            GetNode<Area2D>("Area2D").QueueFree();
            GetNode<Area2D>("SquashZone").QueueFree();
            playerIsNear = false;
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