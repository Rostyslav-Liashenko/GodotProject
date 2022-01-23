using Godot;
using System;

public class MainScene : Node2D
{
	PackedScene ballScene;
	Random rn;

	public override void _Ready()
	{
		ballScene = GD.Load<PackedScene>("res://Ball/BallScene.tscn");
		rn = new Random();
		// Ball node = (Ball) ballScene.Instance();
		// node.Position = new Vector2(0, 0);
		// AddChild(node);
	}

	public override void _Input(InputEvent inp)
	{
		if (inp is InputEventMouseButton mouseButton && mouseButton.Pressed)
		{
			Ball tmpBall = (Ball) ballScene.Instance();
			tmpBall.Position = mouseButton.Position;
			if (rn.Next(0, 2) == 0)
				tmpBall.AppliedForce = new Vector2(100, 30);
			else
				tmpBall.AppliedForce = new Vector2(-100, 30);
			AddChild(tmpBall);
		}
	}
}
