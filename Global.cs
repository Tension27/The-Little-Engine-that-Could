using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node2D
{
	public static int test = 0;
	public static List<string> foundKeys = new List<string>();
	public static List<string> lastLevelTime = new List<string>();
	public static int min = 0;
    public static int sec = 0;
	public static int msec = 0;
    public static double deathTime = 0;
    public static string currentScene = "";
    public static int currentSceneNumber = 0;
    public static int testDeaths = 0;

    //whenever called adds the current time on the stopwatch to min, sec, msec
	public static void addToTotalTime(string d, string e, string f)
	{
        string a = d + e + f;
        string minute = string.Empty;
        string second = string.Empty;
        string milisecond = string.Empty;

        for (int i = 0; i < a.Length; i++)
        {
            if (Char.IsDigit(a[i]) && i < 2)
            {
                minute += a[i];
            }
            else if (Char.IsDigit(a[i]) && i < 5)
            {
                second += a[i];
            }
            else if (Char.IsDigit(a[i]))
            {
                milisecond += a[i];
            }
        }
        min += minute.ToInt();
        sec += second.ToInt();
        msec += milisecond.ToInt();

        if (msec >= 1000)
        {
            msec -= 1000;
            sec++;
        }
        if (sec >= 60)
        {
            sec -= 60;
            min++;
        }
    }

    //checks what scene is currently instanced, used to know what the next level is to be played
    public static int GetCurrentSceneNumber()
    {
        string currentScene = Global.currentScene;
        string nextLevelString = currentScene.ToString();

        //pull the number out of the file path string
        string a = nextLevelString;
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
            nextLevelNum = int.Parse(b) + 1;
        }

        currentSceneNumber = nextLevelNum - 1;

        return nextLevelNum;
    }
}
