using Godot;

public class Player : KinematicBody2D
{
    [Export] private int speed = 500;
    [Export] private int countLife = 3;
    
    private int halfWidthSheep;
    private int halfHeightSheep;
    private Vector2 velocity;
    private Vector2 screenSize;
    
    private Position2D shootPosition;

    [Signal] public delegate void Shoot(Vector2 fromShoot);
    [Signal] public delegate void Dead();
    
    public override void _Ready()
    {
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
            TakeDamage();
            enemy.QueueFree();
        }
        ClampInScreen();
    }

    private void TakeDamage()
    {
        countLife--;
        if (countLife == 0)
            EmitSignal(nameof(Dead));
    }
    
    private void ClampInScreen()
    {
       Position = new Vector2(
            Mathf.Clamp(Position.x, halfWidthSheep, screenSize.x - halfWidthSheep),
            Mathf.Clamp(Position.y, halfHeightSheep, screenSize.y - halfHeightSheep));
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
            EmitSignal(nameof(Shoot), shootPosition.GlobalPosition);
    }
}
