using Godot;
using System;
using System.Reflection.PortableExecutable;

public partial class FlyingEnemy : EnemyRoot
{
    Sprite2D sprite;
    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
        RandomizeColor();
        direction = Vector2.One;
        moveSpeed = 300;
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
                if ((direction.X == -1 && direction.Y == -1) ||
                     direction.X == 1 && direction.Y == 1)
                {
                    sprite.RotationDegrees += 90;
                }
                else
                {
                    sprite.RotationDegrees -= 90;
                }
                direction.X *= -1;
            }
            else
            {
                if ((direction.X == 1 && direction.Y == -1) ||
                     direction.X == -1 && direction.Y == 1)
                {
                    sprite.RotationDegrees += 90;
                }
                else
                {
                    sprite.RotationDegrees -= 90;
                }
                direction.Y *= -1;
            }
            canTurn = false;

            //RandomizeColor();

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

        sprite.SelfModulate = Color.Color8(rNum1, rNum2, rNum3);
    }
}