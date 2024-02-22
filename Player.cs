using Godot;
using godot_2D_tutorial.@enum;

public partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export] public int Speed { get; set; } = 400;

    public Vector2 ScreenSize;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        Hide();
    }

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }

    private void OnBodyEntered(Node2D body)
    {
        Hide();
        EmitSignal(SignalName.Hit);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed(Actions.MoveUp.ToString()))
        {
            velocity.Y -= 1;
        }

        if (Input.IsActionPressed(Actions.MoveDown.ToString()))
        {
            velocity.Y += 1;
        }

        if (Input.IsActionPressed(Actions.MoveLeft.ToString()))
        {
            velocity.X -= 1;
        }

        if (Input.IsActionPressed(Actions.MoveRight.ToString()))
        {
            velocity.X += 1;
        }

        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite2D.Play();
        }
        else
        {
            animatedSprite2D.Stop();
        }

        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );

        if (velocity.X != 0)
        {
            animatedSprite2D.Animation = Animations.WALK.ToString();
            animatedSprite2D.FlipV = false;
        }
        else if (velocity.Y != 0)
        {
            animatedSprite2D.Animation = Animations.UP.ToString();
            animatedSprite2D.FlipV = velocity.Y > 0;
        }

        if (velocity.X < 0)
        {
            animatedSprite2D.FlipH = true;
        }
        else
        {
            animatedSprite2D.FlipH = false;
        }
    }
}
