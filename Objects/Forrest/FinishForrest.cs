using Godot;
using Godot.Collections;
using System;

public partial class FinishForrest : NewFinishBase
{
    public override void _Ready()
    {
        base._Ready();
        OpenDoorAnimation = "Leaves";
        CloseDoorAnimation = "Dead";
        CloseDoorTest();
    }
}
