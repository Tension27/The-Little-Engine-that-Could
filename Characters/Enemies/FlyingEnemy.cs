using Godot;
using System;
using System.Reflection.PortableExecutable;

public partial class FlyingEnemy : EnemyRoot
{
    public override void _Ready()
    {
        RandomizeColor();
        direction = Vector2.One;
        moveSpeed = 200;
    }

    public override async void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        velocity = direction * moveSpeed;

        Velocity = velocity;

        //changes direction and color when it collides wit something
        if ((IsOnWall() || IsOnFloor() || IsOnCeiling()) && canTurn)
        {
            if (IsOnWall())
            {
                direction.X *= -1;
            }
            else
            {
                direction.Y *= -1;
            }
            canTurn = false;

            RandomizeColor();

            await ToSignal(GetTree().CreateTimer(.05f), SceneTreeTimer.SignalName.Timeout);
            canTurn = true;
        }
        MoveAndSlide();
    }

    private void RandomizeColor()
    {
        var rng = new RandomNumberGenerator();

        byte rNum1 = (byte)rng.RandfRange(1, 255);
        byte rNum2 = (byte)rng.RandfRange(1, 255);
        byte rNum3 = (byte)rng.RandfRange(1, 255);

        GetNode<Sprite2D>("Sprite2D").SelfModulate = Color.Color8(rNum1, rNum2, rNum3);
    }
}
