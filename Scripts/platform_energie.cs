using Godot;
using System;

public partial class platform_energie : StaticBody3D
{
	Global scriptGlobal;
	
	public override void _Ready()
	{
		scriptGlobal = GetNode<Global>("/root/Global");
	}
	
	private void _on_area_3d_area_entered(Area3D area)
	{
		if (area.GetParent().Name == "Player")
		{
			//GD.Print("Je Rentre");
			//scriptGlobal.estDansEnergie = true;
			scriptGlobal.comptBlocEnergie++;
		}
	}
	
	private void _on_area_3d_area_exited(Area3D area)
	{
		if (area.GetParent().Name == "Player")
		{
			//GD.Print("JE QUITTE !");
			//scriptGlobal.estDansEnergie = false;
			scriptGlobal.comptBlocEnergie--;
		}
	}

}
