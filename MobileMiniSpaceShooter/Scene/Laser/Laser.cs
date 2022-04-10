using Godot;

public class Laser : KinematicBody2D
{
    [Export] private int speed = 1000;
    private Vector2 velocity;

    private VisibilityNotifier2D vbn;
    
    public override void _Ready()
    {
        velocity = new Vector2(0, -1);
        vbn = GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
        vbn.Connect("screen_exited", this, nameof(Destroy));
    }

    public override void _PhysicsProcess(float delta)
    { 
        MoveAndCollide(velocity.Normalized() * speed * delta);
    }

    private void Destroy() { QueueFree();}
}
