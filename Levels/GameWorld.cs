using Godot;
using System;
using System.Security.Cryptography;

public partial class GameWorld : Node2D
{
    [Export]
    public PackedScene tutorialLevel { get; set; }
    [Export]
	public PackedScene startLevel { get; set; }
    [Export]
    public PackedScene levelTransition { get; set; }
    [Export]
    public PackedScene Leaderboard { get; set; }

    public bool playerIsOnLevelSelect = false;
    public bool playerIsOnTutorial = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        Signals customSignals = GetNode<Signals>("/root/Signals");
        customSignals.OnFinishReached += PlayNextLevel;
		customSignals.ResetCurrentLevel += RestartCurrentLevel;

        Timer timer = (Timer)GetNode<HUD>("HUD").GetChild(5);
        timer.GetChild<Panel>(0).Visible = false;
        timer.GetChild<Label>(1).Visible = false;
        timer.GetChild<Label>(2).Visible = false;
        timer.GetChild<Label>(3).Visible = false;

        GD.Randomize();
    }
    
    //displays the leaderboard
    public void OnLeaderboardPressed()
    {
        GetNode<Control>("MainMenu").Visible = false;
        GetNode<Control>("Leaderboard").Visible = true;
    }

    //starts the tutorial level
    public void OnTutorialPressed()
    {
        playerIsOnLevelSelect = false;
        playerIsOnTutorial = true;
        HUD deathCounter = GetNode<HUD>("HUD");
        deathCounter.GetChild<Label>(3).Visible = false;
        deathCounter.GetChild<Label>(4).Visible = false;
        GetNode<Control>("MainMenu").Visible = false;
        GetNode<HUD>("HUD").Visible = true;
        CharacterBody2D player = GetNode<CharacterBody2D>("CharacterBody2D");
        player.Visible = true;
        Vector2 playerPOS;
        playerPOS.X = -1279;
        playerPOS.Y = 632;
        player.Position = playerPOS;

        Node level = tutorialLevel.Instantiate();

        GetNode<Node2D>("Level").AddChild(level);
    }

    //starts the game at the first level and automatically plays through them all
    public void OnStartPressed()
	{
        playerIsOnLevelSelect = false;
        playerIsOnTutorial = false;
		GetNode<Control>("MainMenu").Visible = false;
        GetNode<HUD>("HUD").Visible = true;
		CharacterBody2D player = GetNode<CharacterBody2D>("CharacterBody2D");
		player.Visible = true;
        HUD deathCounter = GetNode<HUD>("HUD");
        deathCounter.GetChild<Label>(3).Visible = true;
        deathCounter.GetChild<Label>(4).Visible = true;
        Vector2 playerPOS;
        playerPOS.X = -1279;
        playerPOS.Y = 632;
        player.Position = playerPOS;

        //starts/resets the timer if the player starts the game
        Timer timer = (Timer)GetNode<HUD>("HUD").GetChild(5);
        if (timer.Paused == true)
        {
            timer.Paused = false;
        }
        else
        {
            timer.Start();
        }
        timer.GetChild<Panel>(0).Visible = true;
        timer.GetChild<Label>(1).Visible = true;
        timer.GetChild<Label>(2).Visible = true;
        timer.GetChild<Label>(3).Visible = true;

        Global.currentSceneNumber = 1;

        Node level = startLevel.Instantiate();

        GetNode<Node2D>("Level").AddChild(level);
    }

    //the method called that starts the next level
	public void PlayNextLevel()
	{
        Timer timer = (Timer)GetChild(3).GetChild(5);
        //If the player is on the level select and not the main game, it restarts the current level upon completion instead of playing the next one
        if (playerIsOnLevelSelect == true)
        {
            timer.Start();
            GD.Print($"this was your time: {Global.min}:{Global.sec}.{Global.msec}");
            Global.min = 0;
            Global.sec = 0;
            Global.msec = 0; 
            int listCount = Global.lastLevelTime.Count;
            Global.lastLevelTime.RemoveRange(0, listCount);
            Player player = GetNode<Player>("CharacterBody2D");
            player.ResetDeaths();
            RestartCurrentLevel();
        }
        //If the player is on the tutorial level it takes them to the main menu upon completion, also doesn't have a timer or death counter
        else if (playerIsOnTutorial == true)
        {
            GetNode<Control>("MainMenu").Visible = true;
            GetNode<HUD>("HUD").Visible = false;
            CharacterBody2D player = GetNode<CharacterBody2D>("CharacterBody2D");
            player.Visible = false;
            Global.min = 0;
            Global.sec = 0;
            Global.msec = 0;
            int listCount = Global.lastLevelTime.Count;
            Global.lastLevelTime.RemoveRange(0, listCount);

            GetNode<Node2D>("Level").GetChild(0).QueueFree();
        }
        //If the player is on the main game it runs as normal
        else
        {
            Global.currentSceneNumber++;

            GetNode<CharacterBody2D>("CharacterBody2D").Visible = false;
            GetNode<HUD>("HUD").Visible = false;
            
            timer.Paused = true;
            GetNode<Panel>("HUD/StopWatchTimer/Stopwatch").Visible = false;
            GetNode<Label>("HUD/StopWatchTimer/Label1").Visible = false;
            GetNode<Label>("HUD/StopWatchTimer/Label2").Visible = false;
            GetNode<Label>("HUD/StopWatchTimer/Label3").Visible = false;
            Node transition = levelTransition.Instantiate();

            //Remove and queue free the old level
            Node oldLevel = GetChild(0).GetChild(0);
            Callable.From(() => GetNode<Node2D>("Level").RemoveChild(oldLevel)).CallDeferred();
            oldLevel.QueueFree();

            AddChild(transition); 
        }
	}

    //starts the current level over after dying or pressing the restart button
	public async void RestartCurrentLevel()
	{
        if (playerIsOnLevelSelect == true)
        {
            Timer timer = (Timer)GetChild(3).GetChild(5);
            timer.Start();
        }
		int currentLevelNum = Global.currentSceneNumber;
		//reset the players position to the begining of the level
        PackedScene currentLevel = GD.Load<PackedScene>($"res://Levels/Level_{currentLevelNum}.tscn");

        Node2D levelContainer = GetNode<Node2D>("Level");
		Player player = (Player)GetChild(1);
		Vector2 playerPOS;
		playerPOS.X = player.startPositionX;
		playerPOS.Y = player.startPositionY;
		player.Position = playerPOS;

		//Make sure the players position is reset before instancing a new level to avoid a death loop
        Node oldLevel = levelContainer.GetChild(0);
		Callable.From(() => levelContainer.RemoveChild(oldLevel)).CallDeferred();
		oldLevel.QueueFree();

        await ToSignal(GetTree().CreateTimer(.01f), SceneTreeTimer.SignalName.Timeout);
        //instantiate the new level
        Node level = currentLevel.Instantiate();
	    if (levelContainer.GetChildCount() == 0)
		{
            GetNode<Node2D>("Level").AddChild(level);
        }

		player.isDead = false;
    }

    //Called when the player presses the escape key. Backs out of menus or any level
	public void OnQuitPressed()
	{
        Timer timer = (Timer)GetChild(3).GetChild(5);
        timer.WaitTime = 86400;
        timer.GetChild<Panel>(0).Visible = false;
        timer.GetChild<Label>(1).Visible = false;
        timer.GetChild<Label>(2).Visible = false;
        timer.GetChild<Label>(3).Visible = false;
        Player player = (Player)GetChild(1);
        player.Visible = false;
        GetNode<Control>("MainMenu").Visible = true;
        GetNode<HUD>("HUD").Visible = false;
        Node oldLevel = GetChild(0).GetChildOrNull<Node>(0);
        if (oldLevel != null)
        {
            Callable.From(() => GetNode<Node2D>("Level").RemoveChild(oldLevel)).CallDeferred();
            oldLevel.QueueFree();
        }
    }

    //Used in the level select method plays whatever level the player chooses in the menu
    public void OnLevelSelected(int levelSelected)
    {
        playerIsOnTutorial = false;
        playerIsOnLevelSelect = true;
        Global.currentSceneNumber = levelSelected;

        GetNode<Control>("MainMenu").Visible = false;
        GetNode<HUD>("HUD").Visible = true;
        CharacterBody2D player = GetNode<CharacterBody2D>("CharacterBody2D");
        player.Visible = true;
        HUD deathCounter = GetNode<HUD>("HUD");
        deathCounter.GetChild<Label>(3).Visible = true;
        deathCounter.GetChild<Label>(4).Visible = true;
        Vector2 playerPOS;
        playerPOS.X = -1279;
        playerPOS.Y = 632;
        player.Position = playerPOS;

        Timer timer = (Timer)GetNode<HUD>("HUD").GetChild(5);
        if (timer.Paused == true)
        {
            timer.Paused = false;
        }
        else
        {
            timer.Start();
        }
        timer.GetChild<Panel>(0).Visible = true;
        timer.GetChild<Label>(1).Visible = true;
        timer.GetChild<Label>(2).Visible = true;
        timer.GetChild<Label>(3).Visible = true;

        var nextLevel = GD.Load<PackedScene>($"res://Levels/Level_{levelSelected}.tscn");

        GetChild<Node2D>(0).AddChild(nextLevel.Instantiate());
    }
}