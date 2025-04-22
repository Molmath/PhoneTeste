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
    [Export] float minComboPosition = 150f;
    [Export] float maxComboPosition = 210f;
    float comboPosition;
    Main main = null;
    Vector2 initOffset;
    Queue<Spawnable> spearObjectif = new();
    bool action;

    Spawnable spawnableCurrent;

    Action<Spawnable, float> spearAction;

    private float spearWeigh = 0;
    private float rotationWeigh = 0;

    Vector2 curentOffset;
    Vector2 actualOffset;

    float curentRotation,
          actualRotation;

    RandomNumberGenerator rand = new();
    public void Spear(Spawnable pObj)
    {

        if (action)
        {
            spearObjectif.Enqueue(pObj);
            return;
        }

        action = true;
     
        actualRotation = Rotation;

        spawnableCurrent = pObj;

        comboPosition = rand.RandfRange(minComboPosition, maxComboPosition);
        if (spearObjectif.Count == 0)
        {
            LookAt(pObj.GlobalPosition);
            
           
        }
        spearAction += SpearLerp;
       // spearAction += RotationToObj;

    }


    private void SpearLerp(Spawnable pObj, float pDelta)
    {
        if (spearWeigh < 1)
        {
            spearWeigh += (spearSpeed + (spearSpeed * (spearObjectif.Count * QueueImpact))) * pDelta;
            float easedWeight = LerpEasy.easeCustomPoints.EaseTrans(Mathf.Clamp(spearWeigh, 0f, 1f));
            curentOffset = actualOffset.Lerp(initOffset - new Vector2(0, GlobalPosition.DistanceTo(pObj.GlobalPosition)), easedWeight);
            renderer.Offset = curentOffset;
            return;
        }

        spearAction -= SpearLerp;

        GrabTrash();

        spearWeigh = 0;
        actualOffset = renderer.Offset;

        if (spearObjectif.Count > 0)
        {
            rotaObj = spearObjectif.Peek();

            spearAction += ReturnToComboPosition;
            
            spearAction += RotationToObj;
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
    Spawnable rotaObj;
    private void RotationToObj(Spawnable pObj, float pDelta)
    {
        if (rotationWeigh < 1)
        {
            rotationWeigh += (spearSpeed + (spearSpeed * (spearObjectif.Count * QueueImpact))) * pDelta;
            float easedWeight = LerpEasy.easeBackOutPoints.EaseTrans(Mathf.Clamp(rotationWeigh, 0f, 1f));
            curentRotation = Mathf.Lerp(actualRotation, Utils.LookPosition(GlobalPosition, rotaObj.GlobalPosition), easedWeight);
            Rotation = curentRotation;
            return;
        }
        rotationWeigh = 0;

        spearAction -= RotationToObj;

        //spearAction += SpearLerp;
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
        ScoreManager.GetInstance().GlobalScore += (int)(spawnableCurrent.reward + spawnableCurrent.reward * Main.difficulty);
        spawnableCurrent.QueueFree();
       
    }
    private void ActionFinish()
    {
        actualOffset = renderer.Offset;
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
