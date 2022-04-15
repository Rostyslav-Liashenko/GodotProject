using Godot;

public class AnalogStick : Node2D
{
    private Vector2 offset;
    private TouchScreenButton topStick;
    private bool isDrag;
    
    [Signal] public delegate void MoveTopStick(Vector2 vec);
    
    public override void _Ready()
    {
        offset = new Vector2(-48, -48);
        isDrag = false;
        topStick = GetNode<TouchScreenButton>("TopStick");
        topStick.Connect("pressed", this, nameof(OnPressed));
        topStick.Connect("released", this, nameof(OnReleased));
    }

    public void OnPressed()
    {
        isDrag = true;
    }

    public void OnReleased()
    {
        ResetStateStick(); 
    }
    public override void _Process(float delta)
    {
        if (isDrag)
        {
            topStick.Position = GetLocalMousePosition() + offset;
            if (topStick.Position.DistanceTo(Vector2.Zero) > 64)
                topStick.Position = topStick.Position.Normalized() * 63;
        }
        EmitSignal(nameof(MoveTopStick), topStick.Position);
    }
    
    private void ResetStateStick()
    {
        isDrag = false;
        topStick.Position = Vector2.Zero;
    }
}
