using Godot;
using System;

public partial class MainMenu : Control
{
    //Connects the different levels/scenes to the buttons in the main menu
    [Signal]
    public delegate void OnPlayPressedPassEventHandler();
    [Signal]
    public delegate void OnTutorialPressedPassEventHandler();

    public void OnTutorialPressed()
    {
        EmitSignal(SignalName.OnTutorialPressedPass);
    }

    public void OnLevelSelectPressed()
    {
        LevelSelect levelSelect = (LevelSelect)GetParent().GetChild(6);
        levelSelect.Visible = true;
        Visible = false;
    }

    public void OnPlayPressed()
    {
        EmitSignal(SignalName.OnPlayPressedPass);
    }

    public void OnQuitGamePressed()
    {
        GetTree().Quit();
    }

    public void OnLeaderboardPressed()
    {
        LeaderboardInfoReader leaderboard = (LeaderboardInfoReader)GetParent().GetChild(7);
        leaderboard.Visible = true;
        Visible = false;
    }
}