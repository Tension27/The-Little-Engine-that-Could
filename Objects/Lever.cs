using Godot;
using System;

public partial class Lever : Area2D
{
    [Export]
    public Texture2D ForrestHandle;
    [Export]
    public Texture2D ForrestBase;
    [Export]
    public Texture2D CaveHandle;
    [Export]
    public Texture2D CaveBase;
    [Export]
    public Texture2D FortressHandle;
    [Export]
    public Texture2D FortressBase;

    public override void _Ready()
    {
        int currentLevel = Global.currentSceneNumber;
        SetLevelSprite(currentLevel);
    }

    public void OnLeverFlipped(Node2D area)
	{
        if (area is RigidBody2D)
		{
            if (GetNode<AudioStreamPlayer>("LeverFlipped").IsInsideTree())
            {
                GetNode<AudioStreamPlayer>("LeverFlipped").Play();
            }
        }
    }

    //Figures out which sprite needs to be displayed for the object depending on the level area i.e. forest, cave, or fortress
    public void SetLevelSprite(int currentLevel)
    {
        Sprite2D Base = GetNode<Sprite2D>("StaticBody2D/Sprite2D2");
        Sprite2D Handle = GetNode<Sprite2D>("RigidBody2D/Sprite2D");

        if (currentLevel > 0 && currentLevel < 4)
        {
            Base.Texture = ForrestBase;
            Handle.Texture = ForrestHandle;
        }
        else if (currentLevel > 3 && currentLevel < 7)
        {
            Base.Texture = CaveBase;
            Handle.Texture = CaveHandle;
        }
        else if (currentLevel > 6 && currentLevel < 10)
        {
            Base.Texture = FortressBase;
            Handle.Texture = FortressHandle;
        }
    }
}
