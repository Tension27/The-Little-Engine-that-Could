using Godot;
using System;
using System.Globalization;

public partial class LeaderboardInfoReader : Control
{
    [Signal]
    public delegate void OnLeaderboardPressedEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        //loads in the data from the txt file to display to the leaderboard
		string data = LoadOverall();
		string dataS = LoadSegmented();
        GetChild<Panel>(1).GetChild<Label>(1).Text = data;
        GetChild<Panel>(2).GetChild<Label>(1).Text = dataS;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Quit"))
        {
            OnBackPressed();
        }
    }

    //Pulls the data from the text file
    public string LoadOverall()
	{
        var file = FileAccess.Open("C:\\Users\\Seth Friebel\\Documents\\Documents\\Github\\Auto Complete Test\\Hud\\LeaderBoard\\LeaderboardInfo.txt", FileAccess.ModeFlags.Read);
		string info = file.GetAsText();
		return info;
    }

    //Pulls the data from the text file
    public string LoadSegmented()
    {
        var file = FileAccess.Open("C:\\Users\\Seth Friebel\\Documents\\Documents\\Github\\Auto Complete Test\\Hud\\LeaderBoard\\LeaderboardInfoSegmented.txt", FileAccess.ModeFlags.Read);
        string info = file.GetAsText();
        return info;
    }

    public void OnBackPressed()
    {
        MainMenu mainMenu = (MainMenu)GetParent().GetChild(5);
        mainMenu.Visible = true;
        Visible = false;
    }
}
