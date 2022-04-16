using Godot;

public class World : Node2D
{
    private int costOneCoin;
    private int score;
    private Player player;
    private Spawner spawner;
    private HUD hud;
    private PackedScene laserScene;
    
    public override void _Ready()
    {
        score = 0;
        costOneCoin = 10;
        player = GetNode<Player>("Player");
        spawner = GetNode<Spawner>("Spawner");
        hud = GetNode<HUD>("HUD");
        hud.InitVal(player.CountLife);
        laserScene = (PackedScene) ResourceLoader.Load("res://Scene/Laser/Laser.tscn");

        hud.Connect(nameof(HUD.MoveAnalogStick), player, nameof(player.SetVelocityFromStick));
        hud.Connect(nameof(HUD.PressedBtnShoot), player, nameof(player.ShootLaser));
        player.Connect(nameof(Player.Shoot), this, nameof(CreateLaser));
        player.Connect(nameof(Player.Dead), this, nameof(ReloadGame));
        player.Connect(nameof(Player.TakeDamage), hud, nameof(hud.DecreaseHealthy));
        spawner.Connect(nameof(Spawner.SpawnEnemy), this, nameof(AddEnemy));
        spawner.Connect(nameof(Spawner.SpawnCoin), this, nameof(AddCoin));
    }

    private void UpScore()
    {
        score += costOneCoin;
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
