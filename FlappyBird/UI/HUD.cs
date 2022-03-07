using System;
using Godot;

public class HUD : CanvasLayer
{
    private Label scoreLabel;
    
    public override void _Ready()
    {
        scoreLabel = GetNode<Label>("Score");
    }

    public void UpdateScore(int newScore)
    {
        scoreLabel.Text = Convert.ToString(newScore);
    }
}
