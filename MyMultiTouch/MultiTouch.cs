using Godot;

public class MultiTouch : Node2D
{
    private Godot.Collections.Dictionary storeMulti = new Godot.Collections.Dictionary();
    
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

    public override void _UnhandledInput(InputEvent ev)
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
