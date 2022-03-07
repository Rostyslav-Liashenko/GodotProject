using System;
using Godot;

public class MenuLayer : CanvasLayer
{
    private TextureRect _startMessage;
    private Tween _tween;
    private Label _lblScore;
    private Label _lblHighScore;
    private Control _gameOverMenu;

    private bool _isGameStart;
    [Signal]
    public delegate void StartGame();
    
    public override void _Ready()
    {
        _isGameStart = false;
        _startMessage = GetNode<TextureRect>("StartMenu/StartMessage");
        _tween = GetNode<Tween>("Tween");
        _lblScore = GetNode<Label>("GameOverMenu/VBoxContainer/ScoreLabel");
        _lblHighScore = GetNode<Label>("GameOverMenu/VBoxContainer/HighScoreLabel");
        _gameOverMenu = GetNode<Control>("GameOverMenu");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("flap") && !_isGameStart)
        {
            EmitSignal(nameof(StartGame));
            _tween.InterpolateProperty(_startMessage, "modulate:a", 1, 0, 0.5f);
            _tween.Start();
            _isGameStart = true;
        }
    }

    public void InitGameOverMenu(int score)
    {
        _lblScore.Text = "SCORE: " + Convert.ToString(score);
        _gameOverMenu.Visible = true;
    }

    public void OnRestartButtonPressed()
    {
        GetTree().ReloadCurrentScene();
    }
}
