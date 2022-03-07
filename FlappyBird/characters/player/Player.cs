using Godot;

public class Player : RigidBody2D
{
    private AnimationPlayer _animator;
    private AudioStreamPlayer _hitSound;
    private AudioStreamPlayer _wingSound;
    [Export] private float FLAP_FORCE = -200f;
    [Export] private float MAX_ROTATION_DEGREES = -30.0f;
    
    private bool started;
    private bool isAlive;

    [Signal]
    public delegate void PlayerDied();
    
    public override void _Ready()
    { 
        _animator = GetNode<AnimationPlayer>("AnimationPlayer");
        _hitSound = GetNode<AudioStreamPlayer>("Hit");
        _wingSound = GetNode<AudioStreamPlayer>("Wing");
        
        isAlive = true;
        started = false;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("flap") && isAlive)
        {
            if (!started)
                Start();
            Flap();
        }

        if (RotationDegrees <= MAX_ROTATION_DEGREES)
        {
            AngularVelocity = 0;
            RotationDegrees = MAX_ROTATION_DEGREES;
        }

        if (LinearVelocity.y > 0) 
            AngularVelocity = RotationDegrees <= 90 ? 3 : 0;
    }
    
    private void Start()
    {
        started = true;
        GravityScale = 5.0f;
        _animator.Play("flap");
    }

    private void Flap()
    {
        var tmpLinear = LinearVelocity;
        tmpLinear.y = FLAP_FORCE;
        LinearVelocity = tmpLinear;
        AngularVelocity = -8;
        _wingSound.Play();
    }

    public void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        _animator.Stop();
        EmitSignal(nameof(PlayerDied));
        _hitSound.Play();
    }
}
