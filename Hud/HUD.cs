using Godot;
using System;
using System.Runtime.InteropServices;

public partial class HUD : CanvasLayer
{
	[Signal]
    public delegate void SaveTimeEventHandler();

    public override void _Ready()
	{
        Signals customSignals = GetNode<Signals>("/root/Signals");
		customSignals.OnFinishReached += OnFinishReached;
	}

	//resets the amount of coins displayed on the screen to zero
	public void OnFinishReached()
	{
		GetNode<Label>("Coins").Text = "0";
		EmitSignal(SignalName.SaveTime);
	}

	//updates the death counter on the screen
	public void OnDeath(int deaths)
	{
		GetNode<Label>("DeathCounter").Text = deaths.ToString();
	}

	//updates the coin counter on the screen
	public void OnCoinCollected(int coins)
	{
		GetNode<Label>("Coins").Text = coins.ToString();
	}
}