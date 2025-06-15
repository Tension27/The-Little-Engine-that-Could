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

    //The second set of levels in the select menu
    public void OnForrestLevel1Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 11);
    }

    public void OnForrestLevel2Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 12);
    }

    public void OnForrestLevel3Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 13);
    }

    public void OnForrestLevel4Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 14);
    }

    public void OnForrestLevel5Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 15);
    }

    public void OnForrestLevel6Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 16);
    }

    public void OnForrestLevel7Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 17);
    }

    public void OnForrestLevel8Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 18);
    }

    public void OnForrestLevel9Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 19);
    }

    public void OnForrestLevel10Pressed()
    {
        Visible = false;
        EmitSignal(SignalName.OnLevelSelected, 20);
    }

    public void OnBackPressed()
    {
        MainMenu mainMenu = (MainMenu)GetParent().GetChild(5);
        mainMenu.Visible = true;
        Visible = false;
    }
}
