using Com.IsartDigital.ProjectName;
using Com.IsartDigital.Utils.Tweens;
using Godot;
using Godot.NativeInterop;
using System;
using static System.Formats.Asn1.AsnWriter;

public partial class ScoreDigit : Control
{
    [ExportGroup("LabelSetting")]
    [Export] Label label;
    [Export] private Vector2 digitDistance = new Vector2(50, 0);
    [ExportGroup("shake")]
    [Export] private float shackDurationStart = 0.2f;
    [Export] private float shackDurationEnd = 0.2f;
    [Export] private Color[] shackColor;
    [ExportSubgroup("intensity")]
    [Export]
    private float shackIntensityRight = 5f,
                  shackIntensityLeft = 5f,
                  shackIntensityUp = 5f,
                  shackIntensityDown = 5f;

    [Export] float depthIntensities = 1;


    private RandomNumberGenerator rand = new();
    private const int digitRange = 9;
    private int score;
    public Vector2 initPos;
    private Color initColor;
    private int nChild = 1;

    public bool empty = true;

    ScoreManager scoreManager;
    static bool ok;

    public int depth;
    public override void _Ready()
    {
        base._Ready();
        scoreManager = ScoreManager.GetInstance();
        initColor = label.SelfModulate;
        nChild = GetChildCount();
        this.GetFirstParentWithClasse(out depth);
        GD.Print(depth);
    }
    public int Score
    {
        get { return score; }
        set
        {
            ok = false; 
            empty = true;
            Tween lShack = CreateTween();
            lShack.TweenProperty(this, TweenProp.POSITION,initPos + new Vector2(rand.RandfRange(-shackIntensityLeft * (depth * depthIntensities), shackIntensityRight * (depth * depthIntensities)), rand.RandfRange(-shackIntensityUp * (depth * depthIntensities), shackIntensityDown * (depth * depthIntensities))), shackDurationStart).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            lShack.TweenProperty(this, TweenProp.POSITION, initPos + Vector2.Zero, shackDurationEnd).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            // lShack.Parallel().TweenProperty(label, Utils.SELF_MODULATE, new Color(rand.RandfRange(.5f,1f), rand.RandfRange(.5f, 1f), rand.RandfRange(.5f, 7f)), shackDurationEnd).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            lShack.Parallel().TweenProperty(label, Utils.SELF_MODULATE, shackColor.RandIndex(rand), .5f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            lShack.TweenProperty(label, Utils.SELF_MODULATE, initColor, 1.5f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            if (value > digitRange)
            {
                int remainderValue = value - digitRange;
                Score = remainderValue - 1;
                if (GetChildCount() > nChild)
                {
                    ScoreDigit LFirst = (ScoreDigit)GetChildren()[nChild];
                    LFirst.Score++;
                }
                else
                {


                    ScoreDigit lChild = ScoreManager.GetInstance().CreateDigit(this);
                    lChild.ZIndex = ZIndex - 1;


                    ScoreDigit lParent = this.GetFirstParentWithClasse();
                    lParent.initPos = lParent.Position + digitDistance;
                    lChild.initPos = lChild.Position - digitDistance;
                    lChild.Score++;
                    Tween lTween = CreateTween();
                    lTween.TweenProperty(lChild, TweenProp.POSITION, lChild.Position - digitDistance, 0.2f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
                    lTween.Parallel().TweenProperty(lParent, TweenProp.POSITION, lParent.Position + digitDistance, 0.2f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
                    lTween.Parallel().TweenProperty(lChild, TweenProp.SCALE, Vector2.One * 1.5f, 0.3f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
                    lTween.TweenProperty(lChild, TweenProp.SCALE, Vector2.One, 0.2f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);

                }
            }
            else
            {
                if (GetChildCount() > nChild)
                    while (value < 0)
                    {
                        ScoreDigit LFirst = (ScoreDigit)GetChildren()[nChild];
                        if (LFirst.Score < 0)
                        {
                            empty = false;
                            break;
                        }

                        LFirst.Score--;
                        KillColumn(LFirst);
                        value += 10;
                    }
                else if (value < 0) { value = 0; empty = false; }
                if (!empty) value = 0;

                label.Text = value.ToString();
                score = value;
            }
        }
    }


    private void KillColumn(ScoreDigit pRoot)
    {
        if (EndColumnIsEmpty(pRoot, out ScoreDigit lChild))
        {
            lChild.QueueFree();
            if (!ok)
            {
                GD.Print("ok");
                ok = true;
                pRoot.GetFirstParentWithClasse().Position -= digitDistance;
            }

        }

    }
    private bool EndColumnIsEmpty(ScoreDigit pRoot, out ScoreDigit pChild)
    {
        ScoreDigit lChild = pRoot.GetDeepestChildWithClasse<ScoreDigit>();
        pChild = lChild;
        return lChild.Score <= 0;
    }
}
