using Com.IsartDigital.ProjectName;
using Godot;
using System;

public partial class Harpoon : Node2D
{
	[Export] Sprite2D renderer;
	Main main = null;
	Vector2 initOffset;
	public void Spear(Vector2 pObj)
	{
		Tween lTween = CreateTween();
		LookAt(pObj);
		lTween.TweenProperty(renderer, Utils.OFFSET, renderer.Offset - new Vector2(0, GlobalPosition.DistanceTo(pObj)),0.5f);
        lTween.TweenProperty(renderer, Utils.OFFSET, initOffset, 0.5f);
    }
	public override void _Ready()
	{
        main = Main.instance;
        initOffset = renderer.Offset;

        Replace();

		GetWindow().SizeChanged += Replace;
    }
	private void Replace()
	{
		Position = new Vector2(main.screenSize.X * 0.5f, main.screenSize.Y);
	}
	public override void _Process(double delta)
	{
        Replace();
    }
}
