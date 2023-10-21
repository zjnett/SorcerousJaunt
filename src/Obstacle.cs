using Godot;
using System;

public partial class Obstacle : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node body)
    {
        if (body is Player)
        {
            var player = (Player)body;
            player.Die();
        }
    }
}
