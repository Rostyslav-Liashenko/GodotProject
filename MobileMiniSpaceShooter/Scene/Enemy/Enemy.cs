using Godot;

public class Enemy : KinematicBody2D
{
    [Export] private int boostSpeed = 5;
    [Export] private int speed = 100;
    
    private VisibilityNotifier2D vbn;
    
    public override void _Ready()
    {
        vbn = GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
        vbn.Connect("screen_exited", this, nameof(Destroy));
    }
    
    public override void _PhysicsProcess(float delta)
    {
        speed += boostSpeed;
        var collisionObj = MoveAndCollide(new Vector2(0, 1) * speed * delta);
        if (collisionObj != null && collisionObj.Collider is Laser playerLaser)
        {
            playerLaser.QueueFree();
            QueueFree();
        }
    }

    private void Destroy() { QueueFree(); }
}
