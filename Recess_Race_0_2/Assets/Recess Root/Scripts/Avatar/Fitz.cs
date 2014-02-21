using UnityEngine;
using System.Collections;

public class Fitz : Movable {
	
	public static Fitz fitz;
	
	//--------------------------------------------------------------------------\\
	//------------------------special movement variables------------------------\\
	//--------------------------------------------------------------------------\\
	
	
	private bool pinky								= false;
	private float pinkyTiming						= 20f;
	private bool dashing							= false;
	private float dashingLerp						= 0.1f;
	private bool propelling							= false;
	private float propelGravMod						= -1.35f;
	private bool spinFalling						= false;
	private float spinFallMod						= 0.5f;
	private float spinFallMoveMod					= 0.70f;
	readonly private float propelTiming				= 0.3f;
	private float propelTimer						= 0f;
	private float propellerTimePenalty				= 0.25f;
	
	private bool boogerBoy							= false;
	private float boogerBoySecToMax					= 0.158f;
	private float boogerBoyTiming					= 18f;
	private bool wallHanging						= false;
	private float wallHangFallMod					= 0.15f;
	private float wallJumpLockTiming				= 0.15f;
	private float wallJumpLockTimer					= 0;
	private float wallJumpInput						= 0;
	private float wallJumpTimePenalty				= 0.5f;
	
	private bool otto								= false;
	//protected bool tailFalling						= false;
	private float tailFallMod						= 0.25f;
	private float ottoTiming						= 20f;
	
	private float itemTimer							= 0;
	//--------------------------------------------------------------------------\\
	//-----------------------movement property overrides------------------------\\
	//--------------------------------------------------------------------------\\
	
	protected override float Gravity {
		get {
			if (pinky){
				return base.Gravity * (propelling? propelGravMod : 1);
			}
			return base.Gravity;
		}
	}
	
	protected override float MaxFallSpeed {
		get {
			if (pinky){
				return base.MaxFallSpeed * (spinFalling? spinFallMod : 1);
			}
			if (otto){
				return base.MaxFallSpeed * (controller.getJump? tailFallMod : 1);
			}
			if (boogerBoy){
				return base.MaxFallSpeed * (wallHanging? wallHangFallMod : 1);
			}
			return base.MaxFallSpeed;
		}
	}
	
	protected override float LerpAccel {
		get {
			if (dashing){
				return dashingLerp;
			}
			
			return base.LerpAccel;
			
		}
	}
	
	protected override float MaxSpeed {
		get {
			if (pinky){
				return base.MaxSpeed * (spinFalling? spinFallMoveMod : 1);
			}
			return base.MaxSpeed;
		}
	}
	
	protected override float JumpImpulse {
		get {
			if (pinky || boogerBoy){
				return jumpImpulse;
			}
			else{
				return base.JumpImpulse;
			}
		}
	}
	
	protected override float SecondsToMax {
		get {
			if (boogerBoy){
				return boogerBoySecToMax;
			}
			return base.SecondsToMax;
		}
	}
	
	//--------------------------------------------------------------------------\\
	//--------------------------Miscellaneous properties------------------------\\
	//--------------------------------------------------------------------------\\
	
	private float HorizontalInput{
		get {
			if (!canControl || (boogerBoy && wallJumpLockTimer > 0))
				return 0;
			
			return controller.hAxis;
		}
	}
	
	private bool WallJumping {
		get { return wallJumpLockTimer > 0; }
		set {
			wallJumpLockTimer = value? wallJumpLockTiming : 0;
		}
	}
	
	
	
	//--------------------------------------------------------------------------\\
	//-----------------------------Stunning/Damage------------------------------\\
	//--------------------------------------------------------------------------\\
	
	private bool canControl = true;
	private float stunTimer = 0;
	private float showFor = 0.1f;
	private float hideFor = 0.05f;
	private float blinkTimer = 0;
	private bool spriteShowing = true;
	
	//debug things
	private float ballDropRate = 0.1f;
	private float ballDropTimer = 0;
	private Object ball;
	public bool leaveBalls = true;
	private Vector2 recoilVelocity = new Vector2(140, 0) / TileProperties.tileDimension;
	
	//NeverUsed
	//private float jumpTimer = 0;
	
	protected void Awake(){
		fitz = this;
	}
	
	protected override void Start () {
		base.Start();
		
		ball = Resources.Load("devBall");
	}
	
	//--------------------------------------------------------------------------\\
	//--------------------------------Update!-----------------------------------\\
	//--------------------------------------------------------------------------\\
	
	protected override void FixedUpdate () {
		controller.GetInputs();
	//------------------------------------------------------\\
	//------------------TEST AND HACK-----------------------\\
	//------------------------------------------------------\\
		
		if (Input.GetKey(KeyCode.Alpha1)){
			ChangeToOtto();
		}
		
		if (Input.GetKey(KeyCode.Alpha2)){
			ChangeToPinky();
		}
		
		if (Input.GetKey(KeyCode.Alpha3)){
			ChangeToBoogerBoy();
		}
		
	//------------------------------------------------------\\
	//------------------Handling Inputs---------------------\\
	//------------------------------------------------------\\
		
		
		if (grounded && controller.getJumpDown && canControl){
			velocity = Jump(velocity, JumpImpulse);
			grounded = false;

			//jumpTimer = 0; //Never USed
			if (leaveBalls){
				GameObject newThing = Instantiate(ball, new Vector3(box.center.x, box.yMin, t.position.z), t.rotation) as GameObject;
				ballDropTimer = 0;
				newThing.SendMessage("ChangeColour", Couleur.black);
			}
		}
		
		base.FixedUpdate();
		
		
		velocity = Move(velocity, HorizontalInput);
		
		#region devTime
//		if (!grounded){
//			if (controller.getJumpUp)
//				Debug.Log("Jump up at " + jumpTimer);
//			else
//				Debug.Log("Jump time is " + jumpTimer);
//			jumpTimer += Time.deltaTime;
//		}
		if (leaveBalls){
			ballDropTimer += Time.deltaTime;
			if (ballDropTimer > ballDropRate){
				GameObject newThing = Instantiate(ball, new Vector3(box.center.x, box.yMin, t.position.z), t.rotation) as GameObject;
				ballDropTimer = 0;
				if (Mathf.Abs(velocity.x) == MaxSpeed)
					newThing.SendMessage("ChangeColour", Couleur.yellow);
			}
		}
		#endregion
		if (itemTimer > 0){
			itemTimer -= Time.deltaTime;
			if (itemTimer <= 0){
				ChangeToFitz();
			}
		}
		
	//--------------------------------------------------------------------------\\
	//---------------------------Pinky related checks---------------------------\\
	//--------------------------------------------------------------------------\\
		if (pinky){
			if (controller.getJumpDown && !grounded){
				velocity = new Vector2(velocity.x, 0);
				propelling = true;
				propelTimer = propelTiming;
				controller.ResetJumpInput();
				spinFalling = true;
				itemTimer -= propellerTimePenalty;
			}
			if (propelTimer > 0){
				propelTimer -= Time.deltaTime;
			}
			if (propelTimer <= 0){
				propelling = false;
			}
			
			if (spinFalling && (controller.getD || controller.getRunDown)){
				ResetPropeller();
			}
		}
		
	//--------------------------------------------------------------------------\\
	//------------------------BoogerBoy related checks--------------------------\\
	//--------------------------------------------------------------------------\\
		if (boogerBoy){
			if (controller.getJumpDown && wallHanging){
				Debug.Log("Wall jump!");
				wallJumpInput = HorizontalInput * -1;
				wallJumpLockTimer = wallJumpLockTiming;
				velocity = new Vector2(MaxSpeed * wallJumpInput, JumpImpulse);
				itemTimer -= wallJumpTimePenalty;
			}
			
			if (wallJumpLockTimer > 0){
				wallJumpLockTimer -= Time.deltaTime;
			}
			
		}
		
		
	//--------------------------------------------------------------------------\\
	//-----------------------------hurt & blinking------------------------------\\
	//--------------------------------------------------------------------------\\
		
		if (!canControl){
			stunTimer -= Time.deltaTime;
			blinkTimer += Time.deltaTime;
			if (blinkTimer > showFor && spriteShowing){
				spriteShowing = false;
				r.material.color = Color.clear;
				blinkTimer = 0;
			}else if(blinkTimer > hideFor && !spriteShowing){
				spriteShowing = true;
				r.material.color = Color.white;
				blinkTimer = 0;
			}
			
			if (stunTimer <= 0){
				canControl = true;
				blinkTimer = 0;
				r.material.color = Color.white;
			}
		}
		
	}
	
	void ChangeToPinky(){
		CancelInvoke("ChangeToFitz");
		pinky = true;
		otto = false;
		boogerBoy = false;
		//TEMP
		r.color = new Color(0.8f, 0, 0.5f, 1f);
		Invoke("ChangeToFitz", pinkyTiming);
	}
	
	void ChangeToBoogerBoy(){
		CancelInvoke("ChangeToFitz");
		pinky = false;
		otto = false;
		boogerBoy = true;
		r.color = new Color(0.1f, 0.9f, 0.25f, 1f);
		Invoke("ChangeToFitz", boogerBoyTiming);
	}
	
	void ChangeToOtto(){
		CancelInvoke("ChangeToFitz");
		pinky = false;
		otto = true;
		boogerBoy = false;
		r.color = new Color(0.18f, 0.18f, 0.18f, 1f);
		Invoke("ChangeToFitz", ottoTiming);
	}
	
	void ChangeToFitz (){
		pinky = false;
		otto = false;
		boogerBoy = false;
		r.color = Color.white;
	}
	
	protected override Vector2 Move (Vector2 currentVelocity, float input)
	{
		
		if (pinky && controller.doubleTap){
			dashing = true;
			ResetPropeller();
		}
		
		Vector2 basic = base.Move (currentVelocity, input);
		
		if (input * basic.x <= 0 && !dashing){
			dashing = false;
		}
		
		wallHanging = boogerBoy && CheckIfConnected(sideRays) && (input != 0);
		return basic;
	}
	
	protected override float Accelerate (float input)
	{
		/*
		if (boogerBoy){
			float newX = velocity.x + boogerBoyAccel * input;
			newX = Mathf.Clamp(newX, -MaxSpeed, MaxSpeed);
			return newX;
		}*/
		return base.Accelerate (input);
	}
	
	protected override Vector2 Jump (Vector2 currentVelocity, float amount)
	{
		Vector2 result = base.Jump (currentVelocity, amount);
		controller.ResetJumpInput();
		return result;
	}
	private void SetVelocity (Vector2 newVelocity){
		velocity = newVelocity;
	}
	
	private void OnLand(){
		ResetPropeller();
	}
	
	private void ResetPropeller(){
		spinFalling = false;
		propelling = false;
		propelTimer = 0;
	}
	
	private bool CheckIfConnected(RaycastHit2D[] rays){
		foreach(RaycastHit2D r in rays){
			if (r.fraction > 0){
				return true;
			}
		}
		return false;
	}
	
	
	void OnTriggerEnter2D (Collider2D other){
		DamageScript dmgScript = other.GetComponent<DamageScript>();
		
		if (dmgScript && dmgScript.enabled){
			canControl = false;
			stunTimer = dmgScript.StunDuration;
			dmgScript.SendMessage("CollideWithFitz", t);
			velocity = new Vector2(recoilVelocity.x * (dmgScript.transform.position.x > t.position.x? -1 : 1), recoilVelocity.y);
		}
	}
}
