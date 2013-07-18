using UnityEngine;
using System.Collections;

public class Fitz : Platformer {

	public MovementVariables mondo = new MovementVariables(1.25f, 2.25f, 3f, 0.09375f, 0.0625f, 0.15625f, 0.3125f, 64f, 32f, 1.25f, 4.8125f, 4f, 0.1875f, 0.375f);
	public MovementVariables currentMotor;
	public float CurMaxSpeed{
		get { return isSprinting? mondo.sSpeed : (controller.getRun? currentMotor.rSpeed : currentMotor.wSpeed); }
	}
	public float SkidDecel{
		get { return controller.getRun? currentMotor.rSkidDecel : currentMotor.skidDecel; }
	}
	
	public float CurGravity{
		get { return controller.getJump? currentMotor.gravity : currentMotor.gravityPlus; }
	}
	
	//mondo specific other-variables:
	private bool isSprinting = false;
	private int pMeter = 0;
	
	
	//animation variables
	
	
	private string a_walk = "walk";
	private string a_idle = "idle";
	private string a_fall = "fall";
	private string a_jump = "jump";
	private string a_sJump = "sprintJump";
	private string a_land = "land";
	private string a_dash = "dash";
	private string a_wallGrab = "wallGrab";
	private string a_wallJump = "wallJump";
	private string a_skid = "skid";
	
	// Use this for initialization
	void Start () {
		Setup();
		Application.targetFrameRate = 60;
		//TEMP
		currentMotor = mondo;
	}
	
	// Update is called once per frame
	void Update () {
		controller.GetInputs();
		
		CheckStates();
		
		MondoUpdate();
		
		ApplyMovement();
	}
	
	private void MondoUpdate(){
		
		
		if (pMeter >= 112 && grounded && ((velocity.x <= -mondo.rSpeed && controller.getL) || (velocity.x >= mondo.rSpeed && controller.getR))){ //check to see if I'm sprinting! (pMeter)
			pMeter = 112;	//I need to be on the ground, and I have to be pressing forward in the direction I'm running
			isSprinting = true;
			if (!anim.IsPlaying(a_dash)){
				anim.Play(a_dash);
				velocity = new Vector2((velocity.x > 0? mondo.sSpeed : -mondo.sSpeed), velocity.y);
				Debug.Log("fixing velocity");
			}
		}
		else if (grounded) {
			isSprinting = false;
			if(!anim.IsPlaying(a_walk) && velocity.x != 0){
				anim.Play(a_walk);
			}
		}
		
		//end sprint
		
		if (controller.getL && canMoveLeft){									//if I'm going left...
			if (velocity.x > 0){
			//	Debug.Log(velocity + "before skid");
				velocity += new Vector2(-SkidDecel, 0);		
			//	Debug.Log(velocity + "after skid");
				if (!anim.IsPlaying(a_skid) && grounded){
					anim.Play(a_skid);
					isSprinting = false;
				}
			}
			else{
				velocity += new Vector2(-mondo.accel, 0);
				if (!anim.IsPlaying(a_walk) && grounded && !isSprinting){
					anim.Play(a_walk);
				}
			}
		}
		else if (controller.getR && canMoveRight) {							//if I'm going right... in this instance having getL and getR both be true should be impossible,
			if (velocity.x < 0){																							//but getL takes priority nonetheless
				Debug.Log(velocity + "before skid");
				velocity += new Vector2(SkidDecel, 0);		
				Debug.Log(velocity + "after skid");
				if (!anim.IsPlaying(a_skid) && grounded){
					anim.Play(a_skid);
					isSprinting = false;
				}
			}
			else{
				velocity += new Vector2(mondo.accel, 0);
				if (!anim.IsPlaying(a_walk) && grounded && !isSprinting){
					anim.Play(a_walk);
				}
			}
		}
		else if (!controller.getL && !controller.getR && grounded){		//if I'm not going either direction, apply friction (but not if I'm in air)
			if (velocity.x > mondo.decel) {
				velocity += new Vector2(-mondo.decel, 0);
			}
			else if (velocity.x < -mondo.decel) {
				velocity += new Vector2(mondo.decel, 0);
			}
			
			if (velocity.x <= mondo.decel && velocity.x >= -mondo.decel){
				velocity = new Vector2(0, velocity.y);
				anim.Play(a_idle);
			}
		}
		//end L/R inputs
		
		bool goingMax = false;		//Check if I'm at my max speed	
		
		if (velocity.x > CurMaxSpeed){							
			velocity = new Vector2(CurMaxSpeed, velocity.y);
			if (controller.getRun)
				goingMax = true;
		}
		
		if (velocity.x < -CurMaxSpeed){	
			velocity = new Vector2(-CurMaxSpeed, velocity.y);
			if (controller.getRun)
				goingMax = true;
		}
		
		if (velocity.x == mondo.rSpeed || velocity.x == -mondo.rSpeed && pMeter < 112 && grounded){
			pMeter += 2;
		}	
		else if (velocity.x < mondo.rSpeed && velocity.x > -mondo.rSpeed && pMeter > 0){
			pMeter --;
		}
		
		
		if (!grounded){											//Apply gravity!
			if (velocity.y > -mondo.fallSpeedMax)
				velocity += new Vector2(0, -CurGravity);
			else if (velocity.y < -mondo.fallSpeedMax)
				velocity = new Vector2(velocity.x, -mondo.fallSpeedMax);
		}
		
		if (velocity.x > 0){
			sprite.scale = new Vector3(1, 1, 1);
		}
		
		if (velocity.x < 0){
			sprite.scale = new Vector3(-1, 1, 1);
		}
		
		if (!grounded && controller.getL){
			sprite.scale = new Vector3(-1, 1, 1);
		}
		
		if (!grounded && controller.getR){
			sprite.scale = new Vector3(1, 1, 1);
		}
		
		if (startJump){		//jump initiated!
			velocity = new Vector2(velocity.x, mondo.airSpeedInit);
			float calculator = Mathf.Abs(velocity.x) - mondo.runJumpIncrement;		//add extra height based on jump. 
			while(calculator > 0){
				velocity += new Vector2(0, mondo.airSpeedExtra);
				calculator -= mondo.runJumpIncrement;
			}
			Debug.Log("Starting jump: Am I sprinting? " + isSprinting);
			if (!anim.IsPlaying(a_jump) && !anim.IsPlaying(a_sJump)){
				anim.Play(isSprinting? a_sJump : a_jump);
			}
			
			startJump = false;
		}
		
		if (velocity.y < 0){
			falling = true;
			jumping = false;
			if (!anim.IsPlaying(a_fall) && !isSprinting){
				anim.Play(a_fall);
			}
		}
		
	}
	
	private void KirbyUpdate(){
		
		if (controller.getL && canMoveLeft){									//if I'm going left...
			if (velocity.x > 0){
			//	Debug.Log(velocity + "before skid");
				velocity += new Vector2(-SkidDecel, 0);		
			//	Debug.Log(velocity + "after skid");
				if (!anim.IsPlaying(a_skid) && grounded){
					anim.Play(a_skid);
					isSprinting = false;
				}
			}
			else{
				velocity += new Vector2(-mondo.accel, 0);
				if (!anim.IsPlaying(a_walk) && grounded && !isSprinting){
					anim.Play(a_walk);
				}
			}
		}
		else if (controller.getR && canMoveRight) {							//if I'm going right... in this instance having getL and getR both be true should be impossible,
			if (velocity.x < 0){																							//but getL takes priority nonetheless
				Debug.Log(velocity + "before skid");
				velocity += new Vector2(SkidDecel, 0);		
				Debug.Log(velocity + "after skid");
				if (!anim.IsPlaying(a_skid) && grounded){
					anim.Play(a_skid);
					isSprinting = false;
				}
			}
			else{
				velocity += new Vector2(mondo.accel, 0);
				if (!anim.IsPlaying(a_walk) && grounded && !isSprinting){
					anim.Play(a_walk);
				}
			}
		}
		else if (!controller.getL && !controller.getR && grounded){		//if I'm not going either direction, apply friction (but not if I'm in air)
			if (velocity.x > mondo.decel) {
				velocity += new Vector2(-mondo.decel, 0);
			}
			else if (velocity.x < -mondo.decel) {
				velocity += new Vector2(mondo.decel, 0);
			}
			
			if (velocity.x <= mondo.decel && velocity.x >= -mondo.decel){
				velocity = new Vector2(0, velocity.y);
				anim.Play(a_idle);
			}
		}
	}
}
