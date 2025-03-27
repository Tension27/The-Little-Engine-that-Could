using Godot;
using System;
using System.Drawing;
using System.IO;

public partial class LevelTransition : Control
{
    [Signal]
    public delegate void OnQuitPressedLevelTransitionEventHandler();

    private Signals customSignals;
    public override void _Ready()
    {
        //displays the congratulations message on the transition screen
        customSignals = GetNode<Signals>("/root/Signals");
        int lastLevelNumber = Global.currentSceneNumber;
        GetNode<Label>("Congratulations").Text = $"Level {lastLevelNumber - 1} Complete!";

        //displays the time on the transition scene between levels
        GetChild<Panel>(0).GetChild<Label>(2).Text = $"{Global.lastLevelTime[0]}{Global.lastLevelTime[1]}{Global.lastLevelTime[2]}";
        PrintOverallTime();
        
        //This resets the last level time. The last level time is already added to the total time in the stopwatchtimer.cs file
        int listCount = Global.lastLevelTime.Count;
        Global.lastLevelTime.RemoveRange(0, listCount);

        PlayNextLevel();
    }

    public async void PlayNextLevel()
    {
        await ToSignal(GetTree().CreateTimer(5f), SceneTreeTimer.SignalName.Timeout);

        //Set's the players position back to the starting position for the level
        HUD hud = (HUD)GetParent<Node2D>().GetChild(3);
        CharacterBody2D player = (CharacterBody2D)GetParent<Node2D>().GetChild(1);
        Timer timer = (Timer)GetParent<Node2D>().GetChild(3).GetChild(5);
        Vector2 playerPOS;
        playerPOS.X = -1279;
        playerPOS.Y = 632;
        player.Position = playerPOS;
        player.Visible = true;
        hud.Visible = true;
        timer.Paused = false;
        timer.Start();
        timer.GetChild<Panel>(0).Visible = true;
        timer.GetChild<Label>(1).Visible = true;
        timer.GetChild<Label>(2).Visible = true;
        timer.GetChild<Label>(3).Visible = true;

        //getst the current scene level and plays the next one
        int currentLevelNum = GetNumberFromLevel();

        var nextLevel = GD.Load<PackedScene>($"res://Levels/Level_{currentLevelNum + 1}.tscn");

        GetParent<Node2D>().GetChild<Node2D>(0).AddChild(nextLevel.Instantiate());

        //delets the current scene
        QueueFree();
    }

    //figures out what level is currently being played
    public int GetNumberFromLevel()
    {
        string currentScene = (Global.currentSceneNumber - 1).ToString();

        //pull the number out of the file path string
        string a = currentScene;
        string b = string.Empty;
        int nextLevelNum = 0;

        for (int i = 0; i < a.Length; i++)
        {
            if (Char.IsDigit(a[i]))
            {
                b += a[i];
            }
        }

        if (b.Length > 0)
        {
            nextLevelNum = int.Parse(b);
        }

        return nextLevelNum;
    }

    public void PrintOverallTime()
    {
        string min;
        string sec;
        string msec;

        if (Global.msec < 10)
        {
            msec = $"00{Global.msec}";
        }
        else if (Global.msec < 100)
        {
            msec = $"0{Global.msec}";
        }
        else
        {
            msec = Global.msec.ToString();
        }
        if (Global.sec < 10)
        {
            sec = $"0{Global.sec}";
        }
        else
        {
            sec = $"{Global.sec}";
        }
        if (Global.min < 10)
        {
            min = $"0{Global.min}";
        }
        else
        {
            min = $"{Global.min}";
        }

        GetChild<Panel>(1).GetChild<Label>(2).Text = $"{min}:{sec}.{msec}";
    }
}
