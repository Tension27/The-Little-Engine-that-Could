using Godot;
using System;

public partial class LockedDoor : StaticBody2D
{
    [Export]
    private bool open;
    Node openDoorSfx;
    Node pressButtonSfx;

    public override void _Ready()
    {
        openDoorSfx = GetNode<AudioStreamPlayer>("DoorOpening");
      //pressButtonSfx = GetNode<AudioStreamPlayer>("PressButton");
      //flipLeverSfx = GetNode<AudioStreamPlayer>("Flip");

        //only used to pass a dummy variable into the on off functions because the signal wouldn't work without "Node2d body"
        Node2D test = GetNode<Sprite2D>("Sprite2D");
        if (open == true)
            LeverOn(test);
        else
            LeverOff(test);
    }

    public void OnDoorMoveStarted(string name)
    {
        SfxDeconflicter.Instance.Play(openDoorSfx);
    }

    public void OnDoorMoveFinished(string name)
    {
        GetNode<AudioStreamPlayer>("DoorOpening").Stop();
    }

    //checks to see if the door is starting the level opened or closed
    public void LeverSignal(Node2D body)
    {
        //GetNode<AudioStreamPlayer>("Flip").Play();
        if (open == true)
            LeverOff(body);
        else
            LeverOn(body);
    }

    //opens the door if a lever is switched
    public void LeverOn(Node2D body)
    {
        open = true;

        GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
    }

    //closes the door  if a lever is switched
    public void LeverOff(Node2D body)
    {
        if (GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation == "Idle")
        {
            return;
        }

        open = false;

        GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("Open");
    }
    
    public void OperateDoor(Node2D body)
    {
        if (open == true)
        {
            open = false;
            GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("Open");
        }
        else if (open == false)
        {
            open = true;
            GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
        }
    }
}
