using Godot;

public class ObstacleSpawner : Node2D
{
    [Signal]
    public delegate void PlayerPassed();
    
    private Timer _timer;
    private PackedScene _obstacleScene;
    
    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _obstacleScene = (PackedScene) ResourceLoader.Load("res://environment/Obstacle.tscn");
        GD.Randomize();
    }

    private void GenerateObstacles()
    {
        Obstacle obstacle = (Obstacle) _obstacleScene.Instance();
        obstacle.Connect(nameof(Obstacle.PlayerPassed), this, nameof(OnPlayerPassed));
        AddChild(obstacle);
        
        var curObstaclePosition = obstacle.Position;
        curObstaclePosition.y = GD.Randi() % 400 + 150;
        obstacle.Position = curObstaclePosition;
    }

    public void OnPlayerPassed()
    {
        EmitSignal(nameof(PlayerPassed));
    }
    
    public void OnTimerTimeout()
    {
        GenerateObstacles();
    }
    
    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }
}
