using Godot;

public class World : Node2D
{
    private HUD _hudLayer;
    private ObstacleSpawner _obstacleSpawner;
    private Player _player;
    private StaticBody2D _ground;
    private MenuLayer _menuLayer;
    private int _mainScore;

    private int MainScore
    {
        get => _mainScore;
        set
        {
            _mainScore = value;
            _hudLayer.UpdateScore(_mainScore);
        }
    }
    public override void _Ready()
    {
        _mainScore = 0;
        _hudLayer = GetNode<HUD>("HUD");
        _obstacleSpawner = GetNode<ObstacleSpawner>("ObstacleSpawner");
        _player = GetNode<Player>("Player");
        _ground = GetNode<StaticBody2D>("Ground");
        _menuLayer = GetNode<MenuLayer>("MenuLayer");
        
        _obstacleSpawner.Connect(nameof(ObstacleSpawner.PlayerPassed), this, nameof(IncrementScore));
        _player.Connect(nameof(Player.PlayerDied), this, nameof(OnPlayerDied));
    }
    public void IncrementScore()
    {
        MainScore++;
    }

    public void NewGame()
    {
        MainScore = 0;
        _obstacleSpawner.Start();
    }
    public void OnPlayerDied() // game over
    {
        _obstacleSpawner.Stop();
        _ground.GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        GetTree().CallGroup("obstacles", "StopMove");
        _menuLayer.InitGameOverMenu(MainScore);
    }

    public void OnHitGroundBodyEntered(PhysicsBody2D body)
    {
        if (body is Player player)
            player.Die();
    }

    public void OnMenuLayerStartGame()
    {
        NewGame();
    }
    
}
