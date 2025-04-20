using Com.IsartDigital.ProjectName;
using Godot;
using System;
using System.Buffers;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

public partial class Harpoon : Node2D
{
    [Export] Sprite2D renderer;
    [Export] float spearSpeed = 5f;
    [Export] float QueueImpact = 1f;
    [Export] float comboPosition = 150f;
    Main main = null;
    Vector2 initOffset;
    Queue<Spawnable> spearObjectif = new();
    bool action;

    Spawnable spawnableCurrent;

    Action<Spawnable, float> spearAction;

    private float spearWeigh = 0;

    Vector2 curentOffset;
    Vector2 actualOffset;

    float curentRotation,
          actualRotation;

    public void Spear(Spawnable pObj)
    {

        if (action)
        {
            spearObjectif.Enqueue(pObj);
            return;
        }

        action = true;
        //LookAt(pObj.GlobalPosition);
        actualRotation = Rotation;
        spawnableCurrent = pObj;
        spearAction += RotationToObj;
    }
    private Vector2[] easeCustomPoints = new Vector2[]
 {
    new Vector2(0.00f, 0.00f),
    new Vector2(0.10f, 0.25f),
    new Vector2(0.25f, 0.55f),
    new Vector2(0.45f, 0.85f),
    new Vector2(0.60f, 1.05f), // dépasse 1 ici
    new Vector2(0.75f, 1.02f),
    new Vector2(0.90f, 1.01f),
    new Vector2(1.00f, 1.00f)
 };

    private float EaseCustomA(float t)
    {
        // t est entre 0 et 1
        for (int i = 0; i < easeCustomPoints.Length - 1; i++)
        {
            Vector2 p0 = easeCustomPoints[i];
            Vector2 p1 = easeCustomPoints[i + 1];

            if (t >= p0.X && t <= p1.X)
            {
                float localT = (t - p0.X) / (p1.X - p0.X);
                return Mathf.Lerp(p0.Y, p1.Y, localT);
            }
        }

        return 1f; // Si t dépasse 1, on retourne la fin de la courbe
    }

    private void SpearLerp(Spawnable pObj, float pDelta)
    {
        if (spearWeigh < 1)
        {
            spearWeigh += (spearSpeed + (spearSpeed * (spearObjectif.Count * QueueImpact))) * pDelta;
            float easedWeight = EaseCustomA(Mathf.Clamp(spearWeigh, 0f, 1f));
            curentOffset = actualOffset.Lerp(initOffset - new Vector2(0, GlobalPosition.DistanceTo(pObj.GlobalPosition)), easedWeight);
            renderer.Offset = curentOffset;
            return;
        }

        spearAction -= SpearLerp;
        pObj.QueueFree();
        spearWeigh = 0;
        actualOffset = renderer.Offset;

        if (spearObjectif.Count > 0)
        {
            spearAction += ReturnToComboPosition;
        }
        else
        {
            spearAction += ReturnToBase;
        }
    }

    public void ReturnToComboPosition(Spawnable pObj, float pDelta)
    {
        if (spearWeigh < 1)
        {
            spearWeigh += (spearSpeed + (spearSpeed * (spearObjectif.Count * QueueImpact))) * pDelta;
            curentOffset = actualOffset.Lerp(initOffset - new Vector2(0, comboPosition), Mathf.Clamp(spearWeigh, 0f, 1f));
            renderer.Offset = curentOffset;
            return;
        }

        spearWeigh = 0;
        spearAction -= ReturnToComboPosition;
        ActionFinish();
    }
    private void RotationToObj(Spawnable pObj, float pDelta)
    {
        if (spearWeigh < 1)
        {
            spearWeigh += (spearSpeed + (spearSpeed * (spearObjectif.Count * QueueImpact))) * pDelta;
            curentRotation = Mathf.Lerp(actualRotation, Utils.LookPosition(GlobalPosition, pObj.GlobalPosition), spearWeigh);
            Rotation = curentRotation;
            return;
        }
        spearWeigh = 0;
        spearAction -= RotationToObj;
        spearAction += SpearLerp;
    }
    public void ReturnToBase(Spawnable pObj, float pDelta)
    {
        if (spearWeigh < 1)
        {
            spearWeigh += (spearSpeed + (spearSpeed * (spearObjectif.Count * QueueImpact))) * pDelta;
            curentOffset = actualOffset.Lerp(initOffset, Mathf.Clamp(spearWeigh, 0f, 1f));
            renderer.Offset = curentOffset;
            return;
        }
        spearWeigh = 0;
        spearAction -= ReturnToBase;
        ActionFinish();
    }
    public override void _Process(double delta)
    {
        float lDelta = (float)delta;
        Replace();
        spearAction?.Invoke(spawnableCurrent, lDelta);
    }
    private void GrabTrash()
    {
        spawnableCurrent.QueueFree();
    }
    private void ActionFinish()
    {
        actualOffset = renderer.Offset;
        GD.Print("dedans");
        GD.Print(spearObjectif.Count);
        if (spearObjectif.Count > 0)
        {
            action = false;
            Spear(spearObjectif.Dequeue());
            return;
        }
        action = false;
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

}
