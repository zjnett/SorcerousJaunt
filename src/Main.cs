using Godot;
using System;
using Godot.Collections;

public partial class Main : Node
{
    [Export]
    public PackedScene ObstacleScene { get; set; }

    public override void _Ready()
    {
        // Setup handling here
        GD.Randomize();
    }

    private void OnObstacleSpawnTimerTimeout()
    {
        SpawnObstacles();
        // Set the timer to a random value between 0.5 and 5 seconds
        var timer = GetNode<Timer>("ObstacleSpawnTimer");
        timer.WaitTime = new Random().Next(500, 5000) / 1000f;
    }

    private enum ObstacleArrangement : uint
    {
        Floor,
        Ceiling,
        FloorAndCeiling
    }

    private void SpawnObstacles()
    {
        // Instantiate a new obstacle in the scene
        Obstacle obstacle = ObstacleScene.Instantiate<Obstacle>();

        // Pick a random arrangement of obstacles-- either one on the floor,
        // one on the ceiling, or one on the floor and one on the ceiling
        uint arrangement = GD.Randi() % 3;

        // Pre-generated obstacle positions:
        var floor = GetNode<StaticBody2D>("Floor");
        var ceiling = GetNode<StaticBody2D>("Ceiling");
        // TODO: fix hardcoded values to be based on the viewport dimensions
        Vector2 floorPosition = new Vector2(floor.Position.X + 1300, floor.Position.Y - 16);
        Vector2 ceilingPosition = new Vector2(ceiling.Position.X + 1300, ceiling.Position.Y + 16);

        Vector2 obstaclePosition = new Vector2();

        switch(arrangement)
        {
            case (uint) ObstacleArrangement.Floor:
                obstaclePosition = floorPosition;
                break;
            case (uint) ObstacleArrangement.Ceiling:
                obstaclePosition = ceilingPosition;
                break;
            case (uint) ObstacleArrangement.FloorAndCeiling:
                obstaclePosition = floorPosition;
                // We need to instantiate and set the position of the
                // second obstacle here since the first is already
                // instantiated and then positioned below.
                var obstacle2 = ObstacleScene.Instantiate<Obstacle>();
                obstacle2.Position = ceilingPosition;
                AddChild(obstacle2);
                break;
            
        }

        // Set the obstacle's initial position
        obstacle.Position = obstaclePosition;

        // Add the obstacle to the scene
        AddChild(obstacle);
    }
}
