using System.Collections.Generic;
using Godot;

public class HUD : CanvasLayer
{
    private List<Sprite> healthyPlayer;
    private Label scoreLabel;
    private ButtonShoot btnShoot;

    [Signal] public delegate void PressedBtnShoot();
    public override void _Ready()
    {
        btnShoot = GetNode<ButtonShoot>("ButtonShoot");
        scoreLabel = GetNode<Label>("ScoreLabel");
        healthyPlayer = new List<Sprite>();

        btnShoot.Connect("ClickShootButton", this, nameof(OnPressedBtnShoot));
    }

    public void OnPressedBtnShoot()
    {
        EmitSignal(nameof(PressedBtnShoot));
    }
    public void InitVal(int countLife)
    {
        var healthyIcon =  ResourceLoader.Load("res://Asset/Player/playerShip3_green.png") as Texture;
        Vector2 begCoordinateLineHealthy = new Vector2(50, 115);
        const int offsetBetweenShip = 67;
        for (int i = 0; i < countLife; i++)
        {
            healthyPlayer.Add(new Sprite {Texture = healthyIcon, Scale = new Vector2(0.53f, 0.53f),
                GlobalPosition = begCoordinateLineHealthy}); 
            begCoordinateLineHealthy.x += offsetBetweenShip;
            AddChild(healthyPlayer[i]);
        }
    }
    
    public void UpScore(int newValue)
    {
        scoreLabel.Text = newValue.ToString();
    }

    public void DecreaseHealthy()
    {
        int indexLastSprite = healthyPlayer.Count - 1;
        var oneSprite = healthyPlayer[indexLastSprite];
        healthyPlayer.RemoveAt(indexLastSprite);
        oneSprite.QueueFree();
    }
}
