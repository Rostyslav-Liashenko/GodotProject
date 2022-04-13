using Godot;
using System;

public class ButtonShoot : Node2D
{
    private Area2D area2d;

    [Signal] public delegate void ClickShootButton();
    public override void _Ready()
    {
        area2d = GetNode<Area2D>("Area2D");
        area2d.Connect("input_event", this, nameof(OnInputEvent));
    }

    public void OnInputEvent(Viewport viewport, InputEvent ev, int shapeIdx)
    {
        if (ev is InputEventScreenTouch screenTouch && screenTouch.Pressed)
        {
            GD.Print("ClickShootButton");
            EmitSignal(nameof(ClickShootButton));
        }
    }
    
}
