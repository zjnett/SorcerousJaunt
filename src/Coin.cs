using Godot;
using System;
using Godot.Collections;

public partial class Coin : Area2D
{
    [Signal]
    public delegate void CollectedEventHandler();

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

    public int GetWidth()
    {
        var sprite = GetNode<Sprite2D>("Sprite");
        return (int)sprite.Texture.GetSize().X;
    }

    public void OnBodyEntered(Node body)
    {
        if (body is Player)
        {
            EmitSignal("Collected");
            QueueFree();
        }
    }

    public void OnVisibleOnScreenNotified2DScreenExited()
    {
        QueueFree();
    }
}
