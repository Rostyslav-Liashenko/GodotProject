using Godot;

public class Obstacle : Node2D
{
    private AudioStreamPlayer _pointSound;
    [Signal]
    public delegate void PlayerPassed();
    private const float SPEED = 200f;
    private bool _isMoveLeft;

    public override void _Ready()
    {
        _isMoveLeft = true;
        _pointSound = GetNode<AudioStreamPlayer>("Point");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!_isMoveLeft) return;
        Vector2 curPosition = Position;
        curPosition.x += -SPEED * delta;
        Position = curPosition;
        if (GlobalPosition.x <= -200)
            QueueFree();
    }

    public void OnWallBodyEntered(PhysicsBody2D body)
    {
        if (body is Player player && player.HasMethod("Die"))
            player.Die();
    }

    public void OnScoreAreaBodyExited(PhysicsBody2D body)
    {
        if (body is Player player)
        {
            EmitSignal(nameof(PlayerPassed));
            _pointSound.Play();
        }
    }

    public void StopMove()
    {
        _isMoveLeft = false;
    }
    
}
