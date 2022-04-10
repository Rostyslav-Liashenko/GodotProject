using Godot;

public class Coin : Area2D
{
    [Signal] public delegate void Collect();
    
    public override void _Ready()
    {
        Connect("body_entered", this, nameof(Take));
    }

    public void Take(PhysicsBody2D phBody)
    {
        if (phBody is Player)
        {
            EmitSignal(nameof(Collect));
            QueueFree();
        }
    }
}
