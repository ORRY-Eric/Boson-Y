using Godot;
using System;
using System.Collections.Generic;
using System.Reflection; // Ajoute Stack et List

public partial class level : Node3D
{
	[Export] float speed = 6.0f; // 4f
	[Export] Node3D axeRotation;
	[Export] CharacterBody3D Player;
	float angleRotation = 45f; // 360 / 8 
	bool aRotate = false;
	float angleVise = 0;
	float vitesseRotation = 200f;
	
	int ecartPlateforme = 5;
	int colonneActuel = 0;
	bool animRotate = false;
	
	
	// int angleDernierePlateform = 0;
	int positionDernierePlateform = 0;
	Global scriptGlobal;

	// int comptPlateform = 0;
	// int maxPlateform = 10;
	int distanceGeneration = 15;

	Stack<int> tableauAngle = new Stack<int>();
	Stack<int> tabAngTemp = new Stack<int>();

	int tauxApparitionCote = 50; // 30 // 50
	int tauxApparitionCentre = 50; // 40 // 50

	int DernierAngleConnu = 0;

	bool animRotateGauche = false;
	bool animRotateDroite = false;
	


	
	//Element charge
	// StaticBody3D plateformInstance;
	
	Node3D dernierePlateform;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scriptGlobal = GetNode<Global>("/root/Global");

		// Generer 5 plateformes
		for(int i = 0; i < 5; i++)
		{
			nouvellePlateformBasic();
			positionDernierePlateform -= ecartPlateforme;
		}
		
		tableauAngle.Push(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if (scriptGlobal.estDansEnergie)
		//GD.Print(scriptGlobal.comptBlocEnergie);
		if(scriptGlobal.comptBlocEnergie > 0)
		{
			//GD.Print(scriptGlobal.energie);
			//GD.Print(scriptGlobal.vitesseGlobalEnergie);
			//GD.Print(scriptGlobal.pourcentageEnergie);
			
			speed += scriptGlobal.vitesseGlobalEnergie;
			scriptGlobal.energie += 0.02f;
			scriptGlobal.vitesseGlobalEnergie += 0.00002f;
			scriptGlobal.pourcentageEnergie = (100 * scriptGlobal.energie) / scriptGlobal.objectifEnergie;
		}
		
		// Déplacer le niveau en arrière. Donne l'illusion que le personnage avance
		Vector3 mouvement = new Vector3(0, 0, speed * (float)delta);
		Transform = Transform.Translated(mouvement);

		if (scriptGlobal.debug)
		{
			scriptGlobal.debugText = "ColonneActuel : " + colonneActuel + "\n" +"Colonne Angle Normalement : " + colonneActuel * 45 + " | angleVise : " + angleVise % 360 + "\n"
			  + axeRotation.RotationDegrees.Z % 360 + " | " + angleVise % 360 + "\n" + "IsOnFloor : " + Player.IsOnFloor()
			+ "\n" + "SautDisponible : " + scriptGlobal.sautDisponible;

			scriptGlobal.debugText += "\n AnimRotate : " + animRotate; 
		}
		
		//GD.Print("animRotate : " + animRotate + " | scriptGlobal.sautDisponible : " + scriptGlobal.sautDisponible);
		
		scriptGlobal.animRotate = animRotate;
		
		if(animRotate == false && scriptGlobal.sautDisponible)
		{
			// Rotation LEFT RIGHT
			if (Input.IsActionPressed("ui_left"))
			{
				angleVise += angleRotation;
				animRotateGauche = true;
			}
			if (Input.IsActionPressed("ui_right"))
			{
				angleVise -= angleRotation; 
				animRotateDroite = true;
			}
			
			colonne();
		}
		
		if(animRotateGauche)
			rotationAnimGauche(delta);
		if(animRotateDroite)
			rotationAnimDroite(delta);
		

		if (-Position.Z < positionDernierePlateform + ecartPlateforme * distanceGeneration)
		{
			AlgorithmCheminPlateform();
		}
	}

	public void rotationAnimDroite(double delta)
	{
		if(axeRotation.RotationDegrees.Z > angleVise)
		{
			//GD.Print("ROTATION !");
			animRotate = true;
			float angle = axeRotation.RotationDegrees.Z;
			angle = Mathf.Clamp(angle - vitesseRotation * (float)delta, angleVise, axeRotation.RotationDegrees.Z);
			axeRotation.RotationDegrees = new Vector3(0,0, angle);
		}
		else
		{
			if(animRotateDroite)
			{
				animRotate = false;
				animRotateDroite = false;
			}

		}
	}
	
	public void rotationAnimGauche(double delta)
	{
		if(axeRotation.RotationDegrees.Z < angleVise)
		{
			//GD.Print("ROTATION !");
			animRotate = true;
			float angle = axeRotation.RotationDegrees.Z;
			angle = Mathf.Clamp(angle + vitesseRotation * (float)delta, axeRotation.RotationDegrees.Z, angleVise);
			axeRotation.RotationDegrees = new Vector3(0,0, angle);
		}
		else
			if(animRotateGauche)
			{
				animRotate = false;
				animRotateGauche = false;
			}
	}
	
	// IL BUG.
	public void colonne()
	{
		if (Input.IsActionJustPressed("ui_left"))
		{
			colonneActuel--;
			if(colonneActuel < 0)
				colonneActuel = 7;
			//GD.Print(colonneActuel + " | " + 45 * colonneActuel);
			//GD.Print(Player.IsOnFloor());
		}
			
		if (Input.IsActionJustPressed("ui_right"))
		{
			colonneActuel++;
			if(colonneActuel >= 8)
				colonneActuel = 0;
			//GD.Print(colonneActuel + " | " + 45 * colonneActuel);
			//GD.Print(Player.IsOnFloor());
		}
	}
	
	public void AlgorithmCheminPlateform()
	{
		// showTabAngle();
		calculTabAngle();

		foreach(int angle in tableauAngle)
		{
			int pourcentageEnergie = nbAleatoirePlage(0, 100);
			if(pourcentageEnergie <= 20)
				nouvellePlateformEnergie();
			else
				nouvellePlateformBasic();

			RotationPlateform(angle);
		}
		
		positionDernierePlateform -= ecartPlateforme; // Permet de passer à la ligne suivante
		
	}

	public void calculTabAngle()
	{
		foreach(int angle in tableauAngle)
		{
			for(int multiplicateurAnglePosible = -1; multiplicateurAnglePosible <= 1; multiplicateurAnglePosible++)
			{
				int variationAngle = angle + (int)angleRotation * multiplicateurAnglePosible;
				//GD.Print(variationAngle);

				int tauxApparition = tauxApparitionCote;
				if(variationAngle == 0) tauxApparition = tauxApparitionCentre;

				// Permet de boucler entre 0 et 360
				if(variationAngle == 360) variationAngle = 0;
				if(variationAngle < 0) variationAngle = 360 - (int)angleRotation;

				int chanceApparition = nbAleatoirePlage(0, 100);

				if(chanceApparition <= tauxApparition)
				{
					if(!tabAngTemp.Contains(variationAngle))
					{
						tabAngTemp.Push(variationAngle);
						DernierAngleConnu = variationAngle;
					}
				}
			}
		}

	 	// Si jamais aucun angle ne génère de plateform
		if(tabAngTemp.Count == 0)
		{
			// GD.Print("VIDE !");
			tabAngTemp.Push(DernierAngleConnu - (int)angleRotation);
			tabAngTemp.Push(DernierAngleConnu + (int)angleRotation);
			tabAngTemp.Push(DernierAngleConnu);
		}

		tableauAngle.Clear();
		foreach(int angle in tabAngTemp)
		{
			tableauAngle.Push(angle);
		}

		tabAngTemp.Clear();
	}

	public void showTabAngle()
	{
		GD.Print("-----------");
		foreach(int angle in tableauAngle)
			GD.Print(angle);
		GD.Print("-----------");
		
	}
	
	public int nbAleatoirePlage(int min, int max)
	{
		var random = new RandomNumberGenerator();
		random.Randomize();
		int nbAleatoire = random.RandiRange(min, max);
		//GD.Print(nbAleatoire);
		
		return nbAleatoire;
	}
	

	public void nouvellePlateformBasic()
	{
		var plateformScene = ResourceLoader.Load<PackedScene>("res://Prefabs/centre_cerlce.tscn").Instantiate();
		Node3D test = (Node3D)plateformScene;
		int distanceAjout = (int)Position.Z;
		test.Transform = Transform.Translated(new Vector3(0,0, positionDernierePlateform - distanceAjout));
		// positionDernierePlateform -= ecartPlateforme;
		axeRotation.AddChild(plateformScene);

		dernierePlateform = test;
	}

	public void nouvellePlateformEnergie()
	{
		var plateformScene = ResourceLoader.Load<PackedScene>("res://Prefabs/centre_cerlce_energie.tscn").Instantiate();
		Node3D test = (Node3D)plateformScene;
		int distanceAjout = (int)Position.Z;
		test.Transform = Transform.Translated(new Vector3(0,0, positionDernierePlateform - distanceAjout));
		// positionDernierePlateform -= ecartPlateforme;
		axeRotation.AddChild(plateformScene);

		dernierePlateform = test;
	}

	public void RotationPlateform(int angle)
	{
		Vector3 rotDeg = dernierePlateform.RotationDegrees;
		rotDeg.Z = angle;
		dernierePlateform.RotationDegrees = rotDeg;
	}
}
