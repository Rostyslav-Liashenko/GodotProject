using Godot;

public class AnalogStick : Node2D
{
    private Sprite topStick;
    private Area2D hitBox;
    private bool isDrag;
    
    [Signal] public delegate void MoveTopStick(Vector2 vec);
    
    public override void _Ready()
    {
        isDrag= false;
        topStick = GetNode<Sprite>("TopStick");
        hitBox = GetNode<Area2D>("TopStick/HitBox");

        hitBox.Connect("input_event", this, nameof(OnInputEvent));
    }

    public override void _Process(float delta)
    {
        if (isDrag)
        {
            topStick.Position = GetLocalMousePosition();
            if (topStick.Position.DistanceTo(Vector2.Zero) > 64)
                topStick.Position = topStick.Position.Normalized() * 63;
        }
        EmitSignal(nameof(MoveTopStick), topStick.Position);
    }
    
    public override void _UnhandledInput(InputEvent ev)
    {
        if (ev is InputEventScreenTouch screenTouch && !screenTouch.Pressed)
            ResetStateStick();
    }

    private void ResetStateStick()
    {
        isDrag = false;
        topStick.Position = Vector2.Zero;
    }
    public void OnInputEvent(Viewport viewport, InputEvent ev, int shapeIdx)
    {
        if (ev is InputEventScreenTouch screenTouch)
        {
            if (screenTouch.Pressed)
                isDrag = true;
            else
                ResetStateStick();
        }
    }
    
}
