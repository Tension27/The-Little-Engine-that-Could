using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void UpdateDeathCounterEventHandler(int deaths);
    [Signal]
    public delegate void RestartCurrentLevelEventHandler();
    [Signal]
    public delegate void UpdateCoinCounterEventHandler(int coins);
    [Signal]
    public delegate void OnQuitPressedEventHandler();

    private Signals customSignals;

    int slideDirection = 0;
    public bool isDead = false;
    public bool hasWon = false;
    public int pushForce = 150;
    public static int coins = 0;
    public static int deaths = 0;
	public const int SPEED = 700;
	public const float JUMPVELOCITY = -1400;
	public const int ACCELERATION = 75;
	public const int FRICTION = 100;
    public const int MAXJUMPS = 2;
    public const int GRAVITY = 100;
    public const int MAXSPEED = 700;
    private int currentJumps = 1;
    public int startPositionX = -1279;
    public int startPositionY = 632;
    public int i = 10;
    public bool canMove = true;


    public void ResetDeaths()
    {
        deaths = 0;
        EmitSignal(SignalName.UpdateDeathCounter, deaths);
    }

    public override void _PhysicsProcess(double delta)
    {
        PlayerMovement();
        if (canMove == true)
        {
            Jump();
        }
        PushBlocks();
        ReturnToMenu();
        RestartLevel();
    }

    public override void _Ready()
    {
        customSignals = GetNode<Signals>("/root/Signals");
    }

    public async Task PlayerExitTweenAnim(Area2D finishDoor)
    {
        //Gets the collision hitbox of the exit
        CollisionShape2D exitHitBox = finishDoor.GetNode<CollisionShape2D>("CollisionShape2D");

        // Gets the sprite and hitbox of the character for manipulation in the tween
        CollisionShape2D hitBox = GetNode<CollisionShape2D>("Area2D2/CollisionShape2D");
        Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
        Player player = this;

        //saves all the original values that will be modified so they can be set back after the animation is done
        bool StartHitBox = hitBox.Disabled;
        Color startColor = sprite.Modulate;
        Vector2 startSpriteScale = sprite.Scale;
        Vector2 startSpritePosition = sprite.Position;
      
        //disables movement, the hitbox of the player, and the hitbox of the exit so the player doesn't hit it twice
        canMove = false;
        hitBox.CallDeferred("set_disabled", true);
        exitHitBox.CallDeferred("set_disabled", true);

        //Waits for the players current movement to stop and then finds out how far the sprite is from the center of the exit
        await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);

        //Moves the character to the bottom center of the exit
        Tween moveCharacterToExit = GetTree().CreateTween();
        moveCharacterToExit.TweenProperty(player, "position", new Vector2(finishDoor.Position.X, finishDoor.Position.Y + 128), 2f);

        //Waits for the tween to get to the center of the exit and starts the next tween of the character leaving
        await ToSignal(GetTree().CreateTimer(3f), SceneTreeTimer.SignalName.Timeout);

        //Sets the transparent value for the sprite to fade to in the tween
        Color endColor = new Color(startColor.R, startColor.G, startColor.B, 0f);

        //makes the sprite fade and shrink to dissapear into the exit.
        Tween exitLevelAnim = GetTree().CreateTween();
        exitLevelAnim.SetParallel(true);
        exitLevelAnim.TweenProperty(sprite, "modulate", endColor, 2f);
        exitLevelAnim.TweenProperty(sprite, "scale", new Vector2(1, 1), 3f);

        await ToSignal(exitLevelAnim, "finished");
    }

    //checks if the player is moving left or right
    public Vector2 GetInput()
    {
        var inputDir = Vector2.Zero;
        
        inputDir.X = Input.GetAxis("Move_Left", "Move_Right"); 

        return inputDir;
    }

    //accelerates the player when the start moving
    public void Accelerate(Vector2 direction)
    {
        var velocity = Velocity;
        if (Velocity.X > -700 && direction == Vector2.Left)
        {
            velocity.X -= ACCELERATION;
        }
        else if (Velocity.X < 700 && direction == Vector2.Right)
        {
            velocity.X += ACCELERATION;
        }
        Velocity = velocity;
    }

    //deccelerates the player when they stop moving
    public void Decelerate(int localFriction)
    {
        var velocity = Velocity;

        if (velocity.X > 0)
        {
            velocity.X = velocity.X - localFriction;
            if (velocity.X < 0)
            {
                velocity.X = 0;
            }
        }
        else if (velocity.X < 0)
        {
            velocity.X = velocity.X + localFriction;
            if (velocity.X > 0)
            {
                velocity.X = 0;
            }
        }
        Velocity = velocity;
    }

    //applies friction and acceleration to  the player
    public void PlayerMovement()
    {
        Vector2 direction = GetInput();

        if (canMove == false)
        {
            direction = Vector2.Zero;
        }

        int localFriction = FRICTION;

        if (!IsOnFloor())
        {
            localFriction = 55;
        }

        if (direction == Vector2.Left)
        {
            Accelerate(direction);
        }
        else if (direction == Vector2.Right)
        {
            Accelerate(direction);
        }
        else
        {
            Decelerate(localFriction);
        }

        MoveAndSlide();
    }

    //Gives the player the abillity to push blocks when you walk up against them
    public void PushBlocks()
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            Vector2 direction = GetInput();
            KinematicCollision2D collision = GetSlideCollision(i);

            if (collision.GetCollider() is RigidBody2D rb && (direction != Vector2.Zero || !IsOnFloor()) && Velocity != Vector2.Zero && 
                Math.Abs(rb.LinearVelocity.X) < 700 && Math.Abs(rb.AngularVelocity) < .7)
            {
                Vector2 pushDirection = rb.GlobalPosition - this.GlobalPosition;
                rb.ApplyCentralForce(pushDirection.Normalized() * pushForce);

                return;
            }
        }
    }

    //used to fix a bug where if the box is on top of the players head you weren't able to move. So now if  it's on the players head it just bounces off.
    public void BoxIsOnHead(Node2D body)
    {
        if (body is RigidBody2D rb)
        {
            rb.ApplyCentralImpulse(Vector2.Up * 75);
        }
    }

    public void Jump()
    {
        Vector2 velocity = Velocity;

        if (Input.IsActionJustPressed("Jump"))
        {
            if (currentJumps < MAXJUMPS)
            {
                velocity.Y = JUMPVELOCITY;
                currentJumps += 1;
                GetNode<AudioStreamPlayer>("Jump").Play();
            }
        }
        else
        {
            velocity.Y += GRAVITY;
        }
        if (IsOnFloor() == true)
        {
            currentJumps = 1;
        }

        Velocity = velocity;
    }

    //adds a bounce when you jump on an enemys head
    public void Bounce()
    {
        Vector2 velocity = Velocity;

        velocity.Y = JUMPVELOCITY * .7f;

        Velocity = velocity;
    }

    public void OnCoinEntered()
    { 
        coins++;
        EmitSignal(SignalName.UpdateCoinCounter, coins);
    }

    public void AddDeath()
    {
        if (isDead == false)
        {
            GetNode<AudioStreamPlayer>("Death").Play();
            isDead = true;
            coins = 0;
            Global.foundKeys.Clear();
            deaths++;
            EmitSignal(SignalName.UpdateDeathCounter, deaths);
            EmitSignal(SignalName.RestartCurrentLevel);
            EmitSignal(SignalName.UpdateCoinCounter, coins);
        }
    }

    public void ReturnToMenu()
    {
        Timer timer = (Timer)GetParent().GetChild(3).GetChild(5);
        if (Input.IsActionJustPressed("Quit"))
        {
            Node transition = GetParent().GetChildOrNull<LevelTransition>(8);
            if (transition != null)
            {
                GD.Print("Tets");
                transition.QueueFree();
            }
            timer.Paused = false;
            timer.WaitTime = 86400;
            coins = 0;
            deaths = 0;
            Global.foundKeys.Clear();
            Global.min = 0;
            Global.sec = 0;
            Global.msec = 0;
            Global.currentSceneNumber = 0;
            EmitSignal(SignalName.UpdateDeathCounter, deaths);
            EmitSignal(SignalName.UpdateCoinCounter, coins);
            EmitSignal(SignalName.OnQuitPressed);
        }
    }

    public void RestartLevel()
    {
        if (Input.IsActionJustPressed("Reset"))
        {
            Global.foundKeys.Clear();
            coins = 0;
            deaths++;
            EmitSignal(SignalName.UpdateDeathCounter, deaths);
            EmitSignal(SignalName.UpdateCoinCounter, coins);
            customSignals.EmitSignal(nameof(Signals.ResetCurrentLevel));
        }
    }
}