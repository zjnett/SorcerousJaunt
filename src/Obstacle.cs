using Godot;
using System;

public partial class Obstacle : Area2D
{
    [Export]
    public float Speed = 750f;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move to the right at Speed
        Position += new Vector2(-Speed * (float)delta, 0);
    }
    
    public void OnBodyEntered(Node body)
    {
        if (body is Player)
        {
            var player = (Player)body;
            player.Die();
        }
    }

    public void OnVisibleOnScreenNotified2DScreenExited()
    {
        QueueFree();
    }
}
