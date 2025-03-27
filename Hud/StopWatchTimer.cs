using Godot;
using System;

public partial class StopWatchTimer : Timer
{
    double time = 0;
    int minutes = 0;
    int seconds = 0;
    int miliseconds = 0;

    public override void _Process(double delta)
    {
        //Starts with a timer of 24 hours. subtracts the time left on the countdown resulting in a counter starting at zero and counting up. 
        // And then adds any time saved from previous timers that were run before Ex: once you die the timer restarts but saves the time that
        // was on the timer and adds it to the next timer so it keeps counting.
        time = 86400 - TimeLeft + Global.deathTime;
        miliseconds = (int)((time % 1) * 1000);
        seconds = (int)(time % 60);
        minutes = (int)(time % 3600) / 60;

        Label msec = GetNode<Label>("Label3");
        Label sec = GetNode<Label>("Label2");
        Label min = GetNode<Label>("Label1");

        //formats the timer on the screen into a more stopwatch like display
        if (miliseconds <10)
        {
            msec.Text = $"00{miliseconds}";
        }
        else if (miliseconds < 100)
        {
            msec.Text = $"0{miliseconds}";
        }
        else
        {
            msec.Text = miliseconds.ToString();
        }
        if (seconds < 10)
        {
            sec.Text = $"0{seconds}.";
        }
        else
        {
            sec.Text = $"{seconds}.";
        }
        if (minutes < 10)
        {
            min.Text = $"0{minutes}:";
        }
        else
        {
            min.Text = $"{minutes}:";
        }
    }
    public override void _Ready()
    {
        Start();
    }

    //after completing a level this takes the final time and adds it to a running total of the time it's taken the player
    // to finish the level. This way at the end of each level and the end of the game it can display the total time taken
    // to beat the game
    public void OnSaveTime()
    {
        string msec = GetNode<Label>("Label3").Text;
        string sec = GetNode<Label>("Label2").Text;
        string min = GetNode<Label>("Label1").Text;

        Global.lastLevelTime.Add(min);
        Global.lastLevelTime.Add(sec);
        Global.lastLevelTime.Add(msec);
        Global.addToTotalTime(min, sec, msec);
        Global.deathTime = 0;

        GD.Print($"{Global.min} {Global.sec} {Global.msec}");
    }

    //adds the time currently on the clock to an integer to be added back into the timer
    public void OnAddTime()
    {
        Global.deathTime += (86400 - TimeLeft);
    }
}