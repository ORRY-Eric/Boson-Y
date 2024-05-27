using Godot;
using System;

public partial class ui : Control
{
	Global scriptGlobal;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scriptGlobal = GetNode<Global>("/root/Global");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetNode<Label>("Energie").Text = "Energie : " + String.Format("{0:0.00}", scriptGlobal.pourcentageEnergie) + " %";
		debug();
	}
	
	public void debug()
	{
		GetNode<Label>("Debug").Text = scriptGlobal.debugText;
	}
}
