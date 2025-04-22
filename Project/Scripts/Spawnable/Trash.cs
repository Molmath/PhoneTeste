using Godot;
using System;

public partial class Trash : Spawnable
{
    protected override void ExitScreen()
    {
        base.ExitScreen();
        GD.PrintRich("[Wave]je suis sortie[/Wave]");

    }
}
