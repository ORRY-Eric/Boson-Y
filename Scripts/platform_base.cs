using Godot;
using System;

public partial class platform_base : StaticBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global script = GetNode<Global>("/root/Global");
		if (script.debug)
			GetNode<MeshInstance3D>("ligneRougeCentre").Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
