using Godot;
using System;
using Godot.Collections;

public partial class Coin : Area2D
{
    [Signal]
    public delegate void CollectedEventHandler();

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node body)
    {
        if (body is Player)
        {
            EmitSignal("Collected");
            QueueFree();
        }
    }
}
