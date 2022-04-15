using Godot;
public class ButtonShoot : Node2D
{
    private TouchScreenButton shootBtn;
    [Signal] public delegate void ClickShootButton();

    public override void _Ready()
    {
        shootBtn = GetNode<TouchScreenButton>("ShootButton");

        shootBtn.Connect("pressed", this, nameof(OnPressed));
    }

    public void OnPressed()
    {
        EmitSignal(nameof(ClickShootButton));
    }
}
