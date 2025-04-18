using Godot;
using System;
using System.Collections.Generic;

public partial class SfxDeconflicter : Node
{
    // Number of milliseconds before the same sound can play a second time.
    private const int DEFAULT_SUPPRESS_SFX_MSEC = 20;

    // Dictionary to store the last played time of each sound.
    private Dictionary<string, int> lastPlayedMsecByResourcePath = new Dictionary<string, int>();

    public static SfxDeconflicter Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    // Plays the specified sound effect, unless it was recently played.
    public void Play(Node player, float fromPosition = 0.0f)
    {
        if (player == null)
            return;
        
        if (!(player is AudioStreamPlayer) && !(player is AudioStreamPlayer2D))
        {
            GD.PushWarning($"Unrecognized AudioStreamPlayer: {player.GetPath()} ({player.GetClass()})");
            return;
        }

        if (!ShouldPlay(player))
        {
            // Suppress sound effect; sound was played too recently.
            return;
        }

        // Play sound effect.
        var audioPlayer = (AudioStreamPlayer)player;
        audioPlayer.Play(fromPosition);
    }

    // Returns 'true' if the specified sound effect should play, updating our state to ensure it doesn't play again.
    public bool ShouldPlay(Node player, int suppressSfxMsec = DEFAULT_SUPPRESS_SFX_MSEC)
    {
        if (player == null)
            return false;

        if (!(player is AudioStreamPlayer) && !(player is AudioStreamPlayer2D))
        {
            GD.PushWarning($"Unrecognized AudioStreamPlayer: {player.GetPath()} ({player.GetClass()})");
            return false;
        }

        bool result = true;
        string resourcePath = ((AudioStreamPlayer)player).Stream.ResourcePath;
        int lastPlayedMsec = lastPlayedMsecByResourcePath.ContainsKey(resourcePath) ? lastPlayedMsecByResourcePath[resourcePath] : 0;

        if (lastPlayedMsec + suppressSfxMsec >= (int)Time.GetTicksMsec())
        {
            // Suppress sound effect; sound was played too recently.
            result = false;
        }
        else
        {
            // Update 'lastPlayed'; sound is about to be played.
            lastPlayedMsecByResourcePath[resourcePath] = (int)Time.GetTicksMsec();
        }

        return result;
    }
}