using Godot;
using System;

public partial class Signals : Node
{
    [Signal]
    public delegate void OnFinishReachedEventHandler();
    [Signal]
    public delegate void ResetCurrentLevelEventHandler();
    [Signal]
    public delegate void OnDoorShouldBeOpenEventHandler();
}