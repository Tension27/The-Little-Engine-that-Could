using Godot;
using System;

public partial class FinishCave : NewFinishBase
{
    public override void _Ready()
    {
        base._Ready();
        OpenDoorAnimation = "LightingTorches";
        CloseDoorAnimation = "UnlitTorches";
        CloseDoor();
    }

    public void OnAnimationFinished(string name)
    {
        if (name == "LightingTorches")
        {
            ap.Play("LitTorches");
        }
    }
}