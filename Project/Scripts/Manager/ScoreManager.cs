using Com.IsartDigital.ProjectName;
using Godot;
using System;

// Author : Mathys Moles


public partial class ScoreManager : Node2D
{
    [Export] private PackedScene microDigit;
    [Export] private CanvasLayer ScoreContainer;
    [Export] Timer timer;
    private int globalScore;
    
    public int GlobalScore
    {
        get { return globalScore; }
        set
        {

            int lValueChange = value - globalScore;
            if(value < 0)
            {
                value = 0;
                scoreHead.Score = 0;
            }
                
            if(!scoreHead.empty && lValueChange > 0) scoreHead.Score += lValueChange;
            else if(scoreHead.empty) scoreHead.Score += lValueChange;
            globalScore = value;
            
            //if (lValueChange < 0 && scoreHead.Score <= 0)
            //{
            //    scoreHead.Score = 0;
            //}
            //else
            //{


            //}
        }
    }
    #region Singleton
    static private ScoreManager instance;

    private ScoreManager() { }

    static public ScoreManager GetInstance()
    {
        if (instance == null) instance = new ScoreManager();
        return instance;

    }
    #endregion
    ScoreDigit scoreHead;

    int state = 0;
    int State
    {
        get { return state; }
        set { state = Mathf.Wrap(value, 0, 3); }
    }
    public override void _Ready()
    {
        #region Singleton Ready
        if (instance != null)
        {
            QueueFree();
            GD.Print(nameof(ScoreManager) + " Instance already exist, destroying the last added.");
            return;
        }

        instance = this;


        #endregion
        scoreHead = CreateDigit(ScoreContainer);
        timer.Timeout += Up;
        
    }

    //public override void _Process(double delta)
    //{
    //    base._Process(delta);
    //    if (Input.IsActionPressed("Click")) 
    //    {
    //        //State++;
    //        //GD.Print(State);
    //
    //        GlobalScore += 99;
    //    }
    //    if (Input.IsActionPressed("Click2"))
    //    {
    //        GlobalScore -= 15;
    //    }
    //}

    private void Up()
    {
        //switch (State)
        //{
        //    case 0:
        //        GlobalScore += 11;
        //        break;
        //
        //    case 2:
        //        GlobalScore -= 11;
        //        break;
        //}

    }
    public ScoreDigit CreateDigit(Node pContainer)
    {
        ScoreDigit lScoreDigit = microDigit.Instantiate<ScoreDigit>();
        pContainer.AddChild(lScoreDigit);
        return lScoreDigit;
    }

    protected override void Dispose(bool pDisposing)
    {
        instance = null;
        base.Dispose(pDisposing);
    }
}
