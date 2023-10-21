using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene ObstacleScene { get; set; }

    public override void _Ready()
    {
        var player = GetNode<Player>("Player");
        SpawnObstacles();
    }

    private void SpawnObstacles()
    {
        // Instantiate a new obstacle in the scene
        Obstacle obstacle = ObstacleScene.Instantiate<Obstacle>();
        GD.Print(obstacle);

        // Set the initial obstacle position to the same as the floor but off
        // the right hand side of the screen
        var floor = GetNode<StaticBody2D>("Floor");
        // TODO: fix hardcoded values to be based on the viewport dimensions
        Vector2 obstaclePosition = new Vector2(floor.Position.X + 1300, floor.Position.Y - 16);

        // Set the obstacle's initial position
        obstacle.Position = obstaclePosition;
        GD.Print(obstacle.Position);

        // Add the obstacle to the scene
        AddChild(obstacle);
    }
}
