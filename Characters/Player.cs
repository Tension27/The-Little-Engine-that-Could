using Godot;
using System;
using System.Runtime.CompilerServices;
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
    public bool isMoveAllowed = true;
    private bool canSlide = true;


    public void ResetDeaths()
    {
        deaths = 0;
        EmitSignal(SignalName.UpdateDeathCounter, deaths);
    }

    public override void _PhysicsProcess(double delta)
    {
        PlayerMovement();
        Jump();
        PushBlocks();
        ReturnToMenu();
        RestartLevel();
    }

    public override void _Ready()
    {
        customSignals = GetNode<Signals>("/root/Signals");
    }

    //checks if the player is moving left or right
    public Vector2 GetInput()
    {
        var inputDir = Vector2.Zero;
        if (isMoveAllowed == true)
        {
            inputDir.X = Input.GetAxis("Move_Left", "Move_Right"); 
        }

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