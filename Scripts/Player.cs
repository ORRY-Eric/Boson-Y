using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export] AnimationPlayer animPlayer;
	[Export] float speedFall = 0.5f; // 1
	[Export] float gravity = 0.4f; // 0.5
	
	float jumpForceMax = 6f; // 8f
	float jumpForceInit = 2f; // 5f
	float jumpForceTemp;
	float jumpAdd = 0.25f; // 0.25f
	
	bool isFall = false;
	
	Vector3 velocity;
	float yVelocity = 0;
	Global scriptGlobal;

	float gravityTemp = 0;

	// bool estToucheMaintenue = false;

	bool estJump = false;
	
	public override void _Ready()
	{
		scriptGlobal = GetNode<Global>("/root/Global");
	}

	public override void _Process(double delta)
	{
		//GD.Print(yVelocity);
		//GD.Print(Position.X);
		if(Position.X != 0)
		{
			var transform = GlobalTransform;
			transform.Origin.X = 0;
			Transform = transform;
		}
		
		Vector3 direction = new Vector3(0,0,0);
		
		
		if (IsOnFloor())
		{
			yVelocity = -0.01f;
			animPlayer.Play("run");
			
			initJump();
		}
		else
		{ 
			// Permet de ne pas pouvoir sauter quand on tombe dans le vide sans avoir sauté
			if (scriptGlobal.sautDisponible && yVelocity <= 0)
				scriptGlobal.sautDisponible = false;
			
			
			// Gestion Gravité
			if(!scriptGlobal.sautDisponible)
			{
				// yVelocity -= gravity;
				// yVelocity = Mathf.Clamp(yVelocity - gravity, -5f, jumpForceMax);
				gravityTemp = Mathf.Clamp(gravityTemp + 0.01f, 0, 3);
				// yVelocity -= gravityTemp;
				yVelocity = Mathf.Clamp(yVelocity - gravityTemp, -6f, jumpForceInit);

			}
			
			if (!isFall)
				animPlayer.Play("jump");
		}
		
		direction.Y = yVelocity;
		velocity = direction * speedFall;
		
		scriptGlobal.estJump = estJump;

		if (Input.IsActionPressed("jump") && scriptGlobal.sautDisponible && !scriptGlobal.animRotate)
		{
			jump();
			estJump = true;
		}
		
		if (Input.IsActionJustReleased("jump"))
		{
			scriptGlobal.sautDisponible = false;
		}
		
		if ((Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))  && scriptGlobal.sautDisponible)
		{
				jump();
				estJump = true;
		}
		
		if (Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
		{
			scriptGlobal.sautDisponible = false;
		}
		
		
		velocity.Y = yVelocity;
		Velocity = velocity;
		MoveAndSlide();
	}
	
	public void jump()
	{
		if (yVelocity < jumpForceMax)
		{
			jumpForceTemp += jumpAdd;
			yVelocity = jumpForceTemp;

			//GD.Print("jumpForceTemp : " + jumpForceTemp + " | y : " + yVelocity);
		}
		else
		{
			scriptGlobal.sautDisponible = false;
			gravityTemp = 0;
		}
	}
	
	public void initJump()
	{
		scriptGlobal.sautDisponible = true;
		jumpForceTemp = jumpForceInit;
		gravityTemp = 0;
		estJump = false;
	}
	
	private void _on_area_3d_recommencer_area_entered(Area3D area)
	{
		if (area.GetParent().Name == "Player")
			recommencer();
	}
	
	public async void recommencer()
	{
		isFall = true;
		animPlayer.Play("falling"); 
		await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
		scriptGlobal.Call("init");
		isFall = false;
		GetTree().ReloadCurrentScene();
	}
}