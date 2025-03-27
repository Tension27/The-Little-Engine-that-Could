using Godot;
using System;

public partial class LevelSelect : Control
{
    //connects all the levels to a button in the level select menu.
    [Signal]
    public delegate void OnLevelSelectedEventHandler(int levelSelected);

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Quit"))
        {
            OnBackPressed();
        }
    }
    public void OnLevel1Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 1);
    }

    public void OnLevel2Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 2);
    }

    public void OnLevel3Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 3);
    }

    public void OnLevel4Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 4);
    }

    public void OnLevel5Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 5);  
    }

    public void OnLevel6Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 6);
    }

    public void OnLevel7Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 7);
    }

    public void OnLevel8Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 8);
    }

    public void OnLevel9Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 9);
    }

    public void OnLevel10Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 10);
    }

    public void OnBackPressed()
    {
        MainMenu mainMenu = (MainMenu)GetParent().GetChild(5);
        mainMenu.Visible = true;
        Visible = false;
    }
}
