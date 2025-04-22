using Com.IsartDigital.ProjectName;
using Godot;
using GodotPlugins.Game;
using System;
using System.Reflection;

public partial class Spawnable : Node2D
{
    [Export] protected Sprite2D renderer;
    [Export] private float speed = 100f;
    [ExportGroup("Shape")]
    [Export] protected  Texture2D[] possibilityTexture;
    [Export] protected  Color[] possibilityColor;
    
    [Export] private float minScale = 0.5f;
    [Export] private float maxScale = 1f;
    [ExportSubgroup("Click")]
    [Export] private float minClickScale = 0.5f;
    [Export] private float maxClickScale = 1f;
    [Export] private float RotationDegreesBounce = 30f;
    [Export] private float ScaleBounce = 0.2f;

    [Export] private float speedClickScale = 0.5f;
    [Export] private int zIndexUp = 50;
    [ExportGroup("Pattern")]
    [Export] private float bounceShift = 1;
    [Export] private float wavePower = 1;
    private float rotationDirection;

    Main main;
    protected RandomNumberGenerator rand = new();
    [ExportGroup("Accessibility")]
    [Export] float distanceClickMarge = 100f;
    [Export] private int baseReward = 100;
    public int reward;

    Action<float> processAction;
    private bool active;

    Vector2 initScale;
    public override void _Ready()
	{
        InitShape();
        reward = baseReward;

        main = Main.instance;
        main.spawnables.Add(this);
        rotationDirection = GlobalPosition.LookPosition(main.screenSize * 0.5f);
        Rotation = rotationDirection;

        bounceShift = rand.RandfRange(0, bounceShift);
        wavePower = rand.RandfRange(0, wavePower);

        if (GlobalPosition - main.screenSize * 0.5f < Vector2.Zero) Scale = new Vector2(Scale.X, -Scale.Y);
        processAction += Move;
    }
    private void InitShape()
    {
        if (possibilityTexture != null) renderer.Texture = possibilityTexture.RandIndex(rand);
        if (possibilityColor != null) Modulate = possibilityColor.RandIndex(rand);
        else renderer.SelfModulate = new Color(rand.RandfRange(0.5f, 1f), rand.RandfRange(0.5f, 1f), rand.RandfRange(0.5f, 1f));
        Scale = Vector2.One * rand.RandfRange(minScale, maxScale);
        initScale = Scale;
        Scale = Vector2.Zero;
        Tween lTween = CreateTween();
        lTween.TweenProperty(this, Utils.SCALE, initScale, speedClickScale);
    }
    public override void _Process(double delta)
    {
       base._Process(delta);
       float lDelta = (float)delta;
        processAction?.Invoke(lDelta);
        if (this.ExitScreen(main.screenSize)) ExitScreen();
    }
    private void Move(float pDelta)
    {
        Vector2 lNewPositon = (Utils.MoveWithRotation(rotationDirection + Mathf.Pi * 0.5f, speed * pDelta) * Mathf.Sin(Main.instance.Time + bounceShift))
                 + (Utils.MoveWithRotation(rotationDirection, (speed * wavePower) * pDelta) * Mathf.Sin(Main.instance.Time + bounceShift))
                 + (Utils.MoveWithRotation(rotationDirection, Main.globalSpeed + speed * pDelta));
        Position += lNewPositon;
        LookAt(Position + lNewPositon.Normalized());
    }
    public bool ActionCLick(Vector2 pClickPosition)
    {
        if (active) return false;

        if(GlobalPosition.DistanceTo(pClickPosition) <= distanceClickMarge * Scale.X)
        {
            active = true; 
            processAction -= Move;
            Tween lTween = CreateTween();
            ZIndex += zIndexUp;
            lTween.TweenProperty(this, Utils.SCALE , Scale + Scale.Normalized() * Vector2.One * rand.RandfRange(minClickScale, maxClickScale), speedClickScale);
            lTween.Parallel().TweenProperty(this, Utils.ROTATION_DEGREES, RotationDegrees + rand.RandfRange(-RotationDegreesBounce, RotationDegreesBounce), speedClickScale);
            lTween.TweenProperty(this, Utils.SCALE, Scale + Scale.Normalized() * Vector2.One * ScaleBounce, speedClickScale);
            lTween.Parallel().TweenProperty(this, Utils.ROTATION_DEGREES,RotationDegrees + rand.RandfRange(-RotationDegreesBounce, RotationDegreesBounce) , speedClickScale);
            lTween.Finished += Finish;
            return true;
        }
        return false;
    }
    protected virtual void ExitScreen()
    {

    }
    protected virtual void Finish()
    {

    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        main.spawnables.Remove(this);
    }
}
