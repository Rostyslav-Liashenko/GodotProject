using Godot;

public class World : Node2D
{
    private PackedScene laserScene;
    private Player player;
    
    public override void _Ready()
    {
        player = GetNode<Player>("Player");
        laserScene = (PackedScene) ResourceLoader.Load("res://Scene/Laser/Laser.tscn");
        player.Connect(nameof(Player.Shoot), this, nameof(CreateLaser));
        player.Connect(nameof(Player.Dead), this, nameof(ReloadGame));
    }

    private void CreateLaser(Vector2 fromShoot)
    {
        var laser = (Laser) laserScene.Instance();
        laser.GlobalPosition = fromShoot;
        AddChild(laser);
    }

    private void ReloadGame()
    {
        GetTree().ReloadCurrentScene();
    }
}
