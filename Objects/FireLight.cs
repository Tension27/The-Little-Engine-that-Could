using Godot;
using System;

public partial class FireLight : OmniLight3D
{
    [Export]
    private NoiseTexture2D noise;
    private float timePassed = 0.0f;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // This method uses a noisemap to make the torches appear to flicker
    public override void _Process(double delta)
    {
        timePassed += (float)delta;

        float sampledNoise = noise.Noise.GetNoise1D(timePassed * 2);
        sampledNoise = Mathf.Abs(sampledNoise);

        PointLight2D light = GetParent<Area2D>().GetNode<PointLight2D>("PointLight2D3");
        PointLight2D light2 = GetParent<Area2D>().GetNode<PointLight2D>("PointLight2D4");
        light.Energy = sampledNoise * 1.8f;
        light2.Energy = sampledNoise * 1.8f;
    }
}
