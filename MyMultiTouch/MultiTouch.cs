using Godot;

public class MultiTouch : Node2D
{
    private Godot.Collections.Dictionary storeMulti = new Godot.Collections.Dictionary();
    private Area2D areaMulti;
    
    public override void _Ready()
    {
        areaMulti = GetNode<Area2D>("AreaMulti");
        areaMulti.Connect("input_event", this, nameof(OnInputEvent));
    }

    public override void _Process(float delta)
    {
        Update();
    }

    public override void _Draw()
    {
        foreach (var key in storeMulti.Keys)
        {
            int ptrIndex = (int) key;
            var pos = (Vector2) storeMulti[ptrIndex];
            DrawCircle(pos, 40, Colors.Red);
        }
    }

    public void OnInputEvent(Viewport viewport, InputEvent ev, int shapeIdx)
    {
        if (ev is InputEventScreenTouch screenTouch)
        {
            if (screenTouch.Pressed)
                storeMulti[screenTouch.Index] = screenTouch.Position;
            else
                storeMulti.Remove(screenTouch.Index);
            GetTree().SetInputAsHandled();
        }
        else if (ev is InputEventScreenDrag screenDrag)
        {
            storeMulti[screenDrag.Index] = screenDrag.Position;
            GetTree().SetInputAsHandled();
        }
    }
}
