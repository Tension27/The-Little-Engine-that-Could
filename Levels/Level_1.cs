using Godot;
using System;

public partial class Level_1 : Node2D
{
    private bool isMouseAndKeyboard;
    const float deadZone = .4f;
	AnimatedSprite2D Left;
    AnimatedSprite2D Right;
    AnimatedSprite2D Up;
    AnimatedSprite2D Down;
    AnimatedSprite2D A;
    AnimatedSprite2D W;
    AnimatedSprite2D D;
    AnimatedSprite2D SpaceBar;
    AnimatedSprite2D Joystick;
    AnimatedSprite2D DPad;
    AnimatedSprite2D Controller_A;
    AnimatedSprite2D Controller_Y;
    AnimatedSprite2D Main_Menu;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Left = GetNode<AnimatedSprite2D>("Keyboard/Left");
        Right = GetNode<AnimatedSprite2D>("Keyboard/Right"); ;
        Up = GetNode<AnimatedSprite2D>("Keyboard/Up"); ;
        Down = GetNode<AnimatedSprite2D>("Keyboard/Down"); ;
        A = GetNode<AnimatedSprite2D>("Keyboard/A"); ;
        W = GetNode<AnimatedSprite2D>("Keyboard/W"); ;
        D = GetNode<AnimatedSprite2D>("Keyboard/D"); ;
        SpaceBar = GetNode<AnimatedSprite2D>("Keyboard/SpaceBar");
        Joystick = GetNode<AnimatedSprite2D>("Controller/Joystick");
        DPad = GetNode<AnimatedSprite2D>("Controller/DPad");
        Controller_A = GetNode<AnimatedSprite2D>("Controller/Controller_A");
        Controller_Y = GetNode<AnimatedSprite2D>("Controller/Controller_Y");
        Main_Menu = GetNode<AnimatedSprite2D>("Controller/Controller_Menu");
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
            GetNode<Node2D>("Keyboard").Visible = true;
            GetNode<Node2D>("Controller").Visible = false;
        }
        else
        {
            GetNode<Node2D>("Keyboard").Visible = false;
            GetNode<Node2D>("Controller").Visible = true;
        }

        //Logic for showing Key Presses
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.Left)
            {
                Left.Frame = 1;
            }
            else if (keyEvent.Keycode == Key.Right)
            {
                Right.Frame = 1;
            }
            else if (keyEvent.Keycode == Key.Up)
            {
                Up.Frame = 1;
            }
            else if (keyEvent.Keycode == Key.Down)
            {
                Down.Frame = 1;
            }
            else if (keyEvent.Keycode == Key.A)
            {
                A.Frame = 1;
            }
            else if (keyEvent.Keycode == Key.W)
            {
                W.Frame = 1;
            }
            else if (keyEvent.Keycode == Key.D)
            {
                D.Frame = 1;
            }
            else if (keyEvent.Keycode == Key.Space)
            {
                SpaceBar.Frame = 1;
            }

        }
        //Logic for showing Key Releases
        else if (@event is InputEventKey keyEvent2 && !keyEvent2.Pressed)
        {
            if (keyEvent2.Keycode == Key.Left)
            {
                Left.Frame = 0;
            }
            else if (keyEvent2.Keycode == Key.Right)
            {
                Right.Frame = 0;
            }
            else if (keyEvent2.Keycode == Key.Up)
            {
                Up.Frame = 0;
            }
            else if (keyEvent2.Keycode == Key.Down)
            {
                Down.Frame = 0;
            }
            else if (keyEvent2.Keycode == Key.A)
            {
                A.Frame = 0;
            }
            else if (keyEvent2.Keycode == Key.W)
            {
                W.Frame = 0;
            }
            else if (keyEvent2.Keycode == Key.D)
            {
                D.Frame = 0;
            }
            else if (keyEvent2.Keycode == Key.Space)
            {
                SpaceBar.Frame = 0;
            }
        }
        //Logic for showing Joystick movement
        else if (@event is InputEventJoypadMotion joyStickEvent)
        {
            if (joyStickEvent.IsActionPressed("Move_Left"))
            {
                Joystick.Frame = 1;
            }
            else if (joyStickEvent.IsActionPressed("Move_Right"))
            {
                Joystick.Frame = 2;
            }
            else if (joyStickEvent.IsActionReleased("Move_Right") || joyStickEvent.IsActionReleased("Move_Left"))
            {
                Joystick.Frame = 0;
            }
        }
        //Logic for Controller Button Presses
        else if (@event is InputEventJoypadButton joyPadEvent && joyPadEvent.Pressed)
        {
            if (joyPadEvent.ButtonIndex == JoyButton.DpadUp)
            {
                DPad.Frame = 3;                         
            }
            else if (joyPadEvent.ButtonIndex == JoyButton.DpadLeft)
            {
                DPad.Frame = 1;
            }
            else if (joyPadEvent.ButtonIndex == JoyButton.DpadRight)
            {
                DPad.Frame = 2;
            }
            else if (joyPadEvent.ButtonIndex == JoyButton.A)
            {
                Controller_A.Frame = 1;   
            }
            else if (joyPadEvent.ButtonIndex == JoyButton.Start)
            {
                Main_Menu.Frame = 1;
            }
            else if (joyPadEvent.ButtonIndex == JoyButton.Y)
            {
                Controller_Y.Frame = 1;
            }

        }
        //Logic for Controller Button Releases
        else if (@event is InputEventJoypadButton joyPadEvent2 && !joyPadEvent2.Pressed)
        {
            if (joyPadEvent2.ButtonIndex == JoyButton.DpadUp)
            {
                DPad.Frame = 0;
            }
            else if (joyPadEvent2.ButtonIndex == JoyButton.DpadLeft)
            {
                DPad.Frame = 0;
            }
            else if (joyPadEvent2.ButtonIndex == JoyButton.DpadRight)
            {
                DPad.Frame = 0;
            }
            else if (joyPadEvent2.ButtonIndex == JoyButton.A)
            {
                Controller_A.Frame = 0;
            }
            else if (joyPadEvent2.ButtonIndex == JoyButton.Start)
            {
                Main_Menu.Frame = 0;
            }
            else if (joyPadEvent2.ButtonIndex == JoyButton.Y)
            {
                Controller_Y.Frame = 0;
            }
        }
    }
}