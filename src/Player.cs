using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void GameOverEventHandler();

    [Export]
    private const float Gravity = 2000f;
    [Export]
    private const float FloatSpeed = 4000f;

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }

        if (Input.IsActionPressed("jump"))
        {
            // Zero out velocity
            Velocity = new Vector2(0, 0);
            velocity.Y -= FloatSpeed * (float)delta;
        }

        Velocity = velocity;

        // GD.Print(Velocity);
        MoveAndSlide();
    }

    public void Die()
    {
        // Handle player death here
        GD.Print("Player died!");
    }
}
