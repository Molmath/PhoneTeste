using Com.IsartDigital.ProjectName;
using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
    public static Main instance;

    [Export] PackedScene spawnTrash;
    [Export] PackedScene spawnFish;
    [Export] PackedScene harpoonScene;

    [Export] float fishProbability = 0.2f;

    [Export] Timer spawnTimer;
    [Export] float downSpawnTime = 0.1f;
    [Export] float minTime = 0.2f;
    [ExportGroup("accecibility")]
    [Export] float clampSpawnTimeDifficulty = 0.8f;
    [Export] float clampSpawnSpeedDifficulty = 1f;
    private float initTime;

    public static float difficulty = 0;
    public static float globalSpeed;
    [Export] public float addSpeedDifficulty = 100;
    public float Difficulty
    { 
        get { return difficulty; } 
        set {
            difficulty = value;
            if (value >= clampSpawnTimeDifficulty) spawnTimer.WaitTime = (initTime - initTime * clampSpawnTimeDifficulty);
            else spawnTimer.WaitTime = (initTime - initTime * value);
            if (value >= clampSpawnSpeedDifficulty) globalSpeed = addSpeedDifficulty * value;
        } 
    }

    RandomNumberGenerator rand = new();
    Spawnable actualSpawn = null;
    Vector2 lBigDrag = Vector2.Zero;
    InputEventScreenDrag drag = null;

    public Vector2 screenSize;
    
    public List<Spawnable> spawnables = new List<Spawnable>();
    public float Time {  get; private set; }
    [Export] float timeScale = 1f;
    Harpoon harpoon;
    public override void _Ready()
    {
        base._Ready();
        instance = this;
        harpoon = harpoonScene.Instantiate<Harpoon>();
        AddChild(harpoon);
        screenSize = GetWindow().Size;
        GetWindow().SizeChanged += () => screenSize = GetWindow().Size;
        spawnTimer.Timeout += () =>
        {
            CreateSpawn();
          // if(spawnTimer.WaitTime - downSpawnTime >= minTime) spawnTimer.WaitTime -= downSpawnTime;
        };
        initTime = (float)spawnTimer.WaitTime;


    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if(@event is InputEventScreenTouch lTouch && lTouch.Pressed)
        {
            foreach (Spawnable lSpawn in spawnables)
            {
                if(lSpawn.ActionCLick(lTouch.Position)) harpoon.Spear(lSpawn);
            }
        }
    }
    public override void _Process(double delta)
    {
        float lDelta = (float )delta;
        Time += timeScale * lDelta;
        
        Difficulty += 0.01f * lDelta;
        GD.Print(Difficulty);

    }
    private void CreateSpawn()
    {
        
        float lRandFloat = rand.RandfRange(0, 1);
        Spawnable lNode;
        if (lRandFloat <= fishProbability) lNode = spawnFish.Instantiate<Spawnable>();
        else lNode = spawnTrash.Instantiate<Spawnable>();

        int lRandInt = rand.RandiRange(0, 3);
                lNode.Position = new Vector2(rand.RandfRange(0, screenSize.X), rand.RandfRange(0, screenSize.Y));
        AddChild(lNode);
    }
}
