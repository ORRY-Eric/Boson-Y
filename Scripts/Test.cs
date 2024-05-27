using Godot;
using System;

public partial class Test : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		

		Vector3 rotDeg = GetNode<Node3D>("centreCerlce").RotationDegrees;
		rotDeg.Z = 45;
		GetNode<Node3D>("centreCerlce").RotationDegrees = rotDeg;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/*
		
		// Choisie entre energie ou plateform simple
		int pourcentageEnergie = nbAleatoirePlage(0, 100);
		if(pourcentageEnergie <= 20)
			nouvellePlateformEnergie();
		else
			nouvellePlateformBasic();

		// Angle de rotation
		int angleDegree = nbAleatoirePlage(-1, 1); 
		angleDernierePlateform += angleDegree * (int)angleRotation;
		if(angleDernierePlateform != 0)
			RotationPlateform(angleDernierePlateform);
		
		// int angleDegreeTemp = angleDegree;
		// int positionDernierePlateformTemp = positionDernierePlateform;

		// tableauAngle.Push(angleDernierePlateform);

	*/


}
