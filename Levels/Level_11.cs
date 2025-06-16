using Godot;
using System;

public partial class Level_11 : Node2D
{
    private bool isMouseAndKeyboard;
    const float deadZone = .4f;
    Node2D Keys;
	Node2D Word_Arrows;
	Node2D wordOR;
	AnimatedSprite2D Left;
    AnimatedSprite2D Right;
    AnimatedSprite2D Up;
    AnimatedSprite2D Down;
    AnimatedSprite2D A;
    AnimatedSprite2D W;
    AnimatedSprite2D D;
    AnimatedSprite2D SpaceBar;
    AnimatedSprite2D Left_Joystick;
    AnimatedSprite2D Right_Joystick;
    AnimatedSprite2D DPad_Up;
    AnimatedSprite2D DPad_Left;
    AnimatedSprite2D DPad_Right;
    AnimatedSprite2D Controller_A;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Keys = GetNode<Node2D>("Keyboard");
		Word_Arrows = GetNode<Node2D>("Word_Arrows");
        wordOR = GetNode<Sprite2D>("Word_Arrows/Or");
        Left = GetNode<AnimatedSprite2D>("Keyboard/Left");
        Right = GetNode<AnimatedSprite2D>("Keyboard/Right"); ;
        Up = GetNode<AnimatedSprite2D>("Keyboard/Up"); ;
        Down = GetNode<AnimatedSprite2D>("Keyboard/Down"); ;
        A = GetNode<AnimatedSprite2D>("Keyboard/A"); ;
        W = GetNode<AnimatedSprite2D>("Keyboard/W"); ;
        D = GetNode<AnimatedSprite2D>("Keyboard/D"); ;
        SpaceBar = GetNode<AnimatedSprite2D>("Keyboard/SpaceBar");
        Left_Joystick = GetNode<AnimatedSprite2D>("Controller/Left_Joystick");
        Right_Joystick = GetNode<AnimatedSprite2D>("Controller/Right_Joystick");
        DPad_Up = GetNode<AnimatedSprite2D>("Controller/DPad_Up");
        DPad_Left = GetNode<AnimatedSprite2D>("Controller/DPad_Left");
        DPad_Right = GetNode<AnimatedSprite2D>("Controller/DPad_Right");
        Controller_A = GetNode<AnimatedSprite2D>("Controller/Controller_A");
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

        //Shows the key pressed for whichever key the user is pressing on keyboard or controller
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
        else if (@event is InputEventJoypadMotion joyStickEvent)
        {
            if (joyStickEvent.IsActionPressed("Move_Left"))
            {
                Left_Joystick.Frame = 1;
            }
            else if (joyStickEvent.IsActionReleased("Move_Left"))
            {
                Left_Joystick.Frame = 0;
            }
            if (joyStickEvent.IsActionPressed("Move_Right"))
            {
                Right_Joystick.Frame = 1;
            }
            else if (joyStickEvent.IsActionReleased("Move_Right"))
            {
                Right_Joystick.Frame = 0;
            }
        }
        else if (@event is InputEventJoypadButton joyPadEvent && joyPadEvent.Pressed)
        {
            if (joyPadEvent.ButtonIndex == JoyButton.DpadUp)
            {
                DPad_Up.Frame = 1;                         
            }
            else if (joyPadEvent.ButtonIndex == JoyButton.DpadLeft)
            {
                DPad_Left.Frame = 1;
            }
            else if (joyPadEvent.ButtonIndex == JoyButton.DpadRight)
            {
                DPad_Right.Frame = 1;
            }
            else if (joyPadEvent.ButtonIndex == JoyButton.A)
            {
                Controller_A.Frame = 1;   
            }
        }
        else if (@event is InputEventJoypadButton joyPadEvent2 && !joyPadEvent2.Pressed)
        {
            if (joyPadEvent2.ButtonIndex == JoyButton.DpadUp)
            {
                DPad_Up.Frame = 0;
            }
            else if (joyPadEvent2.ButtonIndex == JoyButton.DpadLeft)
            {
                DPad_Left.Frame = 0;
            }
            else if (joyPadEvent2.ButtonIndex == JoyButton.DpadRight)
            {
                DPad_Right.Frame = 0;
            }
            else if (joyPadEvent2.ButtonIndex == JoyButton.A)
            {
                Controller_A.Frame = 0;
            }
        }
    }
}