using Godot;

public class World : Node2D
{
    private int score;

    private Player player;
    private Spawner spawner;
    private HUD hud;
    private PackedScene laserScene;
    
    public override void _Ready()
    {
        score = 0;
        player = GetNode<Player>("Player");
        spawner = GetNode<Spawner>("Spawner");
        hud = GetNode<HUD>("HUD");
        laserScene = (PackedScene) ResourceLoader.Load("res://Scene/Laser/Laser.tscn");
        
        player.Connect(nameof(Player.Shoot), this, nameof(CreateLaser));
        player.Connect(nameof(Player.Dead), this, nameof(ReloadGame));
        spawner.Connect(nameof(Spawner.SpawnEnemy), this, nameof(AddEnemy));
        spawner.Connect(nameof(Spawner.SpawnCoin), this, nameof(AddCoin));
    }

    private void UpScore()
    {
        score++;
        hud.UpScore(score);
    }
    private void ReloadGame()
    {
        GetTree().ReloadCurrentScene();
    }

    private void AddEnemy(Enemy enemy)
    {
        enemy.CalculateSpeed(score);
        AddChild(enemy);
    }

    private void AddCoin(Coin coin)
    {
        coin.Connect(nameof(Coin.Collect), this, nameof(UpScore));
        AddChild(coin);
    }
    private void CreateLaser(Vector2 fromShoot)
    {
        var laser = (Laser) laserScene.Instance();
        laser.GlobalPosition = fromShoot;
        AddChild(laser);
    }
}
