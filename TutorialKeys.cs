using Godot;
using System;

public partial class TutorialKeys : Node2D
{
    private bool isMouseAndKeyboard;
    const float deadZone = .4f;
    Sprite2D keyboard;
    Sprite2D controller;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        keyboard = GetNode<Sprite2D>("Keyboard");
        controller = GetNode<Sprite2D>("Controller");
    }

    public override void _Input(InputEvent @event)
    {
        //determine what device the user is playing the game with
        switch (@event)
        {
            case InputEventKey or InputEventMouse:
                isMouseAndKeyboard = true;
                break;
            case InputEventJoypadButton:
            case InputEventJoypadMotion { AxisValue: < -deadZone or > deadZone }:
                isMouseAndKeyboard = false;
                break;
        }

        //decide whether to display contoler tutorial or keyboard tutorial
        if (isMouseAndKeyboard == true)
        {
            keyboard.Visible = true;
            controller.Visible = false;
        }
        else
        {
            keyboard.Visible = false;
            controller.Visible = true;
        }
    }
}