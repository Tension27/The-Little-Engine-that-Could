using Godot;
using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

public partial class LockedDoor : StaticBody2D
{
    [Export]
    private bool open;
    private string openAnimation;
    private string idleAnimation;
    private string correctDoorSound;
    
    Node openDoorSfx;
    Node pressButtonSfx;

    public override void _Ready()
    {
        int currentLevel = Global.currentSceneNumber;
        SetLevelSprite(currentLevel);

        openDoorSfx = GetNode<AudioStreamPlayer>(correctDoorSound);

        //only used to pass a dummy variable into the on off functions because the signal wouldn't work without "Node2d body"
        Node2D test = GetNode<Sprite2D>("Sprite2D");
        if (open == true)
            LeverOn(test);
        else
            LeverOff(test);
    }

    //Figures out which sprite needs to be displayed for the object depending on the level area i.e. forest, cave, or fortress
    public void SetLevelSprite(int currentLevel)
    {
        if (currentLevel > 0 && currentLevel < 4)
        {
            correctDoorSound = "DoorOpening";
            openAnimation = "Open";
            idleAnimation = "Idle";
        }
        else if (currentLevel > 3 && currentLevel < 7)
        {
            correctDoorSound = "DoorOpeningCave";
            openAnimation = "Open_Cave";
            idleAnimation = "Idle_Cave";
        }
        else if (currentLevel > 6 && currentLevel < 10)
        {
            correctDoorSound = "DoorOpeningFortress";
            openAnimation = "Open_Fortress";
            idleAnimation = "Idle_Fortress";
        }
    }

    public void OnDoorMoveStarted(string name)
    {
        SfxDeconflicter.Instance.Play(openDoorSfx);
    }

    public void OnDoorMoveFinished(string name)
    {
        GetNode<AudioStreamPlayer>(correctDoorSound).Stop();
    }

    //checks to see if the door is starting the level opened or closed
    public void LeverSignal(Node2D body)
    {
        if (open == true)
            LeverOff(body);
        else
            LeverOn(body);
    }

    //opens the door if a lever is switched
    public void LeverOn(Node2D body)
    {
        open = true;

        GetNode<AnimationPlayer>("AnimationPlayer").Play(openAnimation);
    }

    //closes the door  if a lever is switched
    public void LeverOff(Node2D body)
    {
        if (GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation == idleAnimation)
        {
            return;
        }

        open = false;

        GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards(openAnimation);
    }
    
    public void OperateDoor(Node2D body)
    {
        if (open == true)
        {
            open = false;
            GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards(openAnimation);
        }
        else if (open == false)
        {
            open = true;
            GetNode<AnimationPlayer>("AnimationPlayer").Play(openAnimation);
        }
    }
}