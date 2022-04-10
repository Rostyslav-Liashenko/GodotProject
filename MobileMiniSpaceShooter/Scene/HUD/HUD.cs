using Godot;

public class HUD : CanvasLayer
{
    private Label scoreLabel;
    
    public override void _Ready()
    {
        scoreLabel = GetNode<Label>("ScoreLabel");
    }

    public void UpScore(int newValue)
    {
        scoreLabel.Text = newValue.ToString();
    }
}
