using Godot;
using System;
using System.Reflection.PortableExecutable;

public partial class FlyingEnemy : EnemyRoot
{
    Sprite2D sprite;
    CollisionShape2D hitBox;
    CollisionShape2D hurtBox;
    CollisionShape2D squashBox;
    public override void _Ready()
    {
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
                    RotationDegrees += 90;
                }
                else
                {
                    RotationDegrees -= 90;
                }
                direction.X *= -1;
            }
            else
            {
                if ((direction.X == 1 && direction.Y == -1) ||
                     direction.X == -1 && direction.Y == 1)
                {
                    RotationDegrees += 90;
                }
                else
                {
                    RotationDegrees -= 90;
                }
                direction.Y *= -1;
            }
            canTurn = false;

            await ToSignal(GetTree().CreateTimer(.05f), SceneTreeTimer.SignalName.Timeout);
            canTurn = true;
        }
        MoveAndSlide();
    }
}