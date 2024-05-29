using Godot;
using System;

public partial class Global : Node
{
	public float energie = 0;
	public float objectifEnergie = 150;
	public bool estDansEnergie = false;
	public float vitesseGlobalEnergie = 0;
	public float pourcentageEnergie = 0;
	
	public bool sautDisponible = true;
	
	public int comptBlocEnergie = 0;
	
	public bool debug = false;
	public string debugText = "";
	public bool animRotate = false;
	
	public bool estJump = false;

	public void init()
	{
		//comptBlocEnergie = 0;
		energie = 0;
		estDansEnergie = false;
		vitesseGlobalEnergie = 0;
		pourcentageEnergie = 0;
	}

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
			GetTree().Quit();
    }
}
