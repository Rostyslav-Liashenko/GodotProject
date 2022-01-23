using Godot;
using System;

public class Ball : RigidBody2D
{
    public void OnVisibilityNotifier2DScreenExited()
    {
        GD.Print("resource delete");
        QueueFree();
    }
}
