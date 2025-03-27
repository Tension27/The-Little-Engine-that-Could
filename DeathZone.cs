using Godot;
using System;

public partial class DeathZone : Area2D
{
    //used to kill the player if he goes out of the world
    public void OnBodyEntered2D(Player player)
    {
        player.AddDeath();
    }
}
