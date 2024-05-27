using Godot;
using System;

public partial class Area3DDestroyPlateform : Area3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_area_entered(Area3D area)
	{
		// Replace with function body.
		// GD.Print(area.GetParent().Name);
		area.GetParent().QueueFree();
	}
	
	
}


