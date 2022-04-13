using Godot;

public class Player : KinematicBody2D
{
    public int CountLife { get; private set; }
    
    private int speed;
    private int halfWidthSheep;
    private int halfHeightSheep;
    private Vector2 velocity;
    private Vector2 screenSize;
    
    private Position2D shootPosition;

    [Signal] public delegate void Shoot(Vector2 fromShoot);
    [Signal] public delegate void Dead();
    [Signal] public delegate void TakeDamage();
    public override void _Ready()
    {
        speed = 500;
        CountLife = 3;
        velocity = Vector2.Zero;
        halfWidthSheep = GetNode<Sprite>("Sprite").Texture.GetWidth() / 2;
        halfHeightSheep = GetNode<Sprite>("Sprite").Texture.GetHeight() / 2;
        shootPosition = GetNode<Position2D>("ShootPosition");
        screenSize = GetViewportRect().Size;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        var collideObj = MoveAndCollide(velocity.Normalized() * speed * delta);
        if (collideObj != null && collideObj.Collider is Enemy enemy)
        {
            DecreaseHealthy();
            enemy.Destroy();
        }
        ClampInScreen();
    }

    private void DecreaseHealthy()
    {
        CountLife --;
        EmitSignal(nameof(TakeDamage));
        if (CountLife == 0)
            EmitSignal(nameof(Dead));
    }
    
    private void ClampInScreen()
    {
       Position = new Vector2(
            Mathf.Clamp(Position.x, halfWidthSheep, screenSize.x - halfWidthSheep),
            Mathf.Clamp(Position.y, halfHeightSheep, screenSize.y - halfHeightSheep));
    }

    public void ShootLaser()
    {
        EmitSignal(nameof(Shoot), shootPosition.GlobalPosition);
    }
    private void GetInput()
    {
        velocity = Vector2.Zero;
        if (Input.IsActionPressed("ui_left"))
            velocity.x--;
        if (Input.IsActionPressed("ui_right"))
            velocity.x++;
        if (Input.IsActionPressed("ui_up"))
            velocity.y--;
        if (Input.IsActionPressed("ui_down"))
            velocity.y++;
        if (Input.IsActionJustPressed("ui_select"))
            ShootLaser();
    }
}
