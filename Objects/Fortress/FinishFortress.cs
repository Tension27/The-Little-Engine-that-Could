using Godot;
using System;

public partial class FinishFortress : NewFinishBase
{
    public override void _Ready()
    {
        base._Ready();
        OpenDoorAnimation = "Open";
        CloseDoorAnimation = "Close";
    }
}