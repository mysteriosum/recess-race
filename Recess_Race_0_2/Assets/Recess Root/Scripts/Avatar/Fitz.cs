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
	private float spinFallMoveMod					= 0.575f;
	private float propelTimer						= 0f;	
	private float propellerTimePenalty				= 0.25f;
	private float pinkyDoubleJumpMode				= 0.5f;
	
	private bool boogerBoy							= false;
	private float boogerBoySecToMax					= 0.178f;
	private float bBoyDecelMod						= 2.0f;
	private float boogerBoyTiming					= 28.5f;
	private float bBoySpeedMod						= 0.85f;
	private bool wallHanging						= false;
	private float wallHangFallMod					= 0.15f;
	private float wallJumpLockTiming				= 0.15f;
	private float wallJumpLockTimer					= 0;
	private float wallJumpInput						= 0;
	private float wallJumpTimePenalty				= 0.5f;
	
	private bool otto								= false;
	private float tailFallMod						= 0.175f;
	private float ottoTiming						= 38f;
	
	private bool dust								= false;
	private float dustGravityMod					= 0.7f;
	private float dustMaxFallMod					= 0.5f;
	private bool dustStorming						= false;
	private float dustTiming						= 15f;
	private GameObject windArea;
	
	private float itemTimer							= 0;
	
	private bool isRolling							= false;
	
	private float speedBoostMod						= 1.7f;
	private float tempMaxSpeed						= 0;
	private bool speedBoosting						= false;
	private float speedBoostTimer					= 0;
	private float falloffAfter						= 1.7f;
	private float speedBoostSec2Max					= 0.125f;
	private int speedBoostDirection					= 0;
	private float noBoostFor						= 0.3f;
	
	private float SpeedBoostFalloff {
		get{
			float result = maxSpeed / secondsToMax * Time.deltaTime;
			return result;
		}
	}
	
	private GameObject pinkyAnim;
	private GameObject boogerAnim;
	private GameObject ottoAnim;
	
	//--------------------------------------------------------------------------\\
	//-----------------------movement property overrides------------------------\\
	//--------------------------------------------------------------------------\\
	
	protected override float Gravity {
		get {
			if (pinky){
				return base.Gravity * (propelling? propelGravMod : 1);
			}
			if (dust){
				return base.Gravity * (dustStorming? dustGravityMod : 1);
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
			
			if (dust){
				return base.MaxFallSpeed * (dustStorming? dustMaxFallMod : 1);
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
			float baseResult = speedBoosting? tempMaxSpeed : base.MaxSpeed;
			
			if (pinky){
				return baseResult * (spinFalling? spinFallMoveMod : 1);
			}
			if (boogerBoy){
				return baseResult * bBoySpeedMod;
			}
			return baseResult;
		}
	}
	
	protected override float JumpImpulse {
		get {
			if (pinky){
				return jumpImpulse * (spinFalling? pinkyDoubleJumpMode : 1);
			}
			else{
				return base.JumpImpulse;
			}
		}
	}
	
	protected override float SecondsToMax {
		get {
			if (speedBoosting){
				return speedBoostSec2Max;
			}
			if (boogerBoy){
				return boogerBoySecToMax;
			}
			return base.SecondsToMax;
		}
	}
	
	protected override float Deceleration {
		get {
			return base.Deceleration * (boogerBoy? bBoyDecelMod : 1);
		}
	}
	
	//--------------------------------------------------------------------------\\
	//--------------------------Miscellaneous properties------------------------\\
	//--------------------------------------------------------------------------\\
	
	protected override string WalkAnimation {
		get {
			return isRolling? a.roll : a.walk;
		}
	}
	
	protected override string FallAnimation {
		get {
			return isRolling? a.roll : a.fall;
		}
	}
	
	private float HorizontalInput{
		get {
			if (!CanControl || (boogerBoy && wallJumpLockTimer > 0))
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
	
	public bool IsPinky {
		get { return pinky; }
	}
	
	public bool IsBoogerBoy {
		get { return boogerBoy; }
	}
	
	public bool IsOtto {
		get { return otto; }
	}
	
	public bool IsHurt {
		get { return hurt; }
	}
	
	public bool DustStorming{
		get{
			return dustStorming;
		}
		set{
			dustStorming = value;
			windArea.SetActive(value);
		}
	}
	
	//--------------------------------------------------------------------------\\
	//-----------------------------Stunning/Damage------------------------------\\
	//--------------------------------------------------------------------------\\
	
	private bool CanControl {
		get { return !hurt; }
		set { hurt = !value; }
	}
	private float stunTimer = 0;
	private float showFor = 0.1f;
	private float hideFor = 0.05f;
	private float blinkTimer = 0;
	private bool spriteShowing = true;
	
	//finish the race
	private bool lockControls = false;
	
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
		
		//Find the animations from Fitz's children
		foreach(Transform anims in GetComponentsInChildren<Transform>()){
			if (anims.name.Contains ("Pinky")){
				pinkyAnim = anims.gameObject;
				pinkyAnim.SetActive (false);
			}
			if (anims.name.Contains ("BoogerBoy")){
				boogerAnim = anims.gameObject;
				boogerAnim.SetActive (false);
			}
			if (anims.name.Contains ("Otto")){
				ottoAnim = anims.gameObject;
				ottoAnim.SetActive (false);
			}
		}
		
		
		//find the object that makes Dust's wind effect work
		for (int i = 0; i < t.childCount; i++) {
			GameObject child = t.GetChild(i).gameObject;
			if (child.tag == "WindArea"){
				windArea = child;
				windArea.SetActive(false);
			}
		}
		
		if (windArea == null){
			Debug.LogWarning("There's no wind area object on the Avatar so Dust won't work");
		}
	}
	
	//--------------------------------------------------------------------------\\
	//--------------------------------Update!-----------------------------------\\
	//--------------------------------------------------------------------------\\
	
	protected override void FixedUpdate () {
		
		if (!activated) return;
		
		if (!lockControls){
			controller.GetInputs();
		}
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
		
		if (Input.GetKey(KeyCode.Alpha4)){
			ChangeToDust();
		}
		
	//------------------------------------------------------\\
	//------------------Handling Inputs---------------------\\
	//------------------------------------------------------\\
		
		
		if ((grounded || isRolling) && controller.getJumpDown && CanControl){
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
			if (controller.getJumpDown && !grounded && !IsHurt){
//				propelling = true;
//				propelTimer = propelTiming;
				controller.ResetJumpInput();
				spinFalling = true;
				velocity = Jump (velocity, JumpImpulse);
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
	//---------------------------Dust related checks----------------------------\\
	//--------------------------------------------------------------------------\\
		
		if (dust){
			if (!dustStorming && !grounded && controller.getRunDown){
				DustStorming = true;
			} else if (dustStorming && !controller.getRun){
				DustStorming = false;
			}
			
			if (grounded){
				DustStorming = false;
			}
		}
		
	//--------------------------------------------------------------------------\\
	//-----------------------------hurt & blinking------------------------------\\
	//--------------------------------------------------------------------------\\
		
		if (stunTimer > 0){
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
				CanControl = true;
				blinkTimer = 0;
				r.material.color = Color.white;
			}
		}
		
		
	//--------------------------------------------------------------------------\\
	//-------------------------Speed Boost Falloff------------------------------\\
	//--------------------------------------------------------------------------\\
		
		if (speedBoosting){
			speedBoostTimer += Time.deltaTime;
			
			
			if (speedBoostTimer > falloffAfter){
				tempMaxSpeed -= SpeedBoostFalloff;
				
			}
			
			if (tempMaxSpeed < maxSpeed || 
				velocity.x == 0 || 
				((speedBoostDirection > 0 && controller.hAxis <= 0) || (speedBoostDirection < 0 && controller.hAxis >= 0))){
				
				speedBoosting = false;
				speedBoostTimer = 0;
			}
		}
		
		
	//--------------------------------------------------------------------------\\
	//-----------------------other random miscellanous checks-------------------\\
	//--------------------------------------------------------------------------\\
		
		
	}
	
	void ChangeToPinky(){
		CancelInvoke("ChangeToFitz");
		pinky = true;
		otto = false;
		boogerBoy = false;
		pinkyAnim.SetActive (true);
		boogerAnim.SetActive(false);
		ottoAnim.SetActive(false);
		renderer.enabled = true;
		//TEMP
		//r.color = new Color(0.8f, 0, 0.5f, 1f);
		
		
		Invoke("ChangeToFitz", pinkyTiming);
	}
	
	void ChangeToBoogerBoy(){
		CancelInvoke("ChangeToFitz");
		pinky = false;
		otto = false;
		boogerBoy = true;
		dust = false;
		
		pinkyAnim.SetActive (false);
		boogerAnim.SetActive(true);
		ottoAnim.SetActive(false);
		windArea.SetActive(false);
		
		renderer.enabled = false;
		//r.color = new Color(0.1f, 0.9f, 0.25f, 1f);
		Invoke("ChangeToFitz", boogerBoyTiming);
	}
	
	void ChangeToOtto(){
		CancelInvoke("ChangeToFitz");
		pinky = false;
		otto = true;
		boogerBoy = false;
		dust = false;
		
		pinkyAnim.SetActive (false);
		boogerAnim.SetActive(false);
		ottoAnim.SetActive(true);
		windArea.SetActive(false);
		
		renderer.enabled = false;
		//r.color = new Color(0.18f, 0.18f, 0.18f, 1f);
		Invoke("ChangeToFitz", ottoTiming);
	}
	
	void ChangeToFitz (){
		pinky = false;
		otto = false;
		boogerBoy = false;
		dust = false;
		
		pinkyAnim.SetActive (false);
		boogerAnim.SetActive(false);
		ottoAnim.SetActive(false);
		
		DustStorming = false;
		spinFalling = false;
		
		renderer.enabled = true;
		//r.color = Color.white;
		
		//TEMP Play audio on camera
		RecessCamera.cam.PlaySound(RecessCamera.cam.sounds.losePower);
	}
	
	void ChangeToDust (){
		CancelInvoke("ChangeToFitz");
		pinky = false;
		otto = false;
		boogerBoy = false;
		dust = true;
		
		pinkyAnim.SetActive (false);
		boogerAnim.SetActive(false);
		ottoAnim.SetActive(false);
		
		renderer.enabled = true;
		Invoke("ChangeToFitz", dustTiming);
	}
	
	protected override Vector2 Move (Vector2 currentVelocity, float input)
	{
		
		if (pinky && controller.doubleTap){
			dashing = true;
			ResetPropeller();
		}
		
		if (grounded && controller.getDDown){
			BeginRoll();
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
	
	private void OnLand(){
		ResetPropeller();
		if (hurt && anim){
			anim.Play (a.rest);
		}
		
		if (controller.getD){
			BeginRoll();
		}
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
	
	
	public void FinishRace(){
		lockControls = true;
		controller.getR = true;
		controller.ResetJumpInput();
		controller.hAxis = 0;
		controller.getR = false;
		controller.getL = false;
	}
	
	
	void OnTriggerEnter2D (Collider2D other){
		
		
		//get hurt by stuff!
		
		DamageScript dmgScript = other.GetComponent<DamageScript>();
		
		if (dmgScript && dmgScript.enabled && !hurt){
			
			if (other.GetComponent<TennisBall>() != null && CheckCaughtTennisBall(TennisBall.catchBallLeeway)){
				Destroy(other.gameObject);
			} else if (!boogerBoy){
				if (anim){
					anim.Play (a.hurt);
				}
				CanControl = false;
				stunTimer = dmgScript.StunDuration;
				velocity = new Vector2(recoilVelocity.x * (dmgScript.transform.position.x > t.position.x? -1 : 1), recoilVelocity.y);
			}
		}
		
		
		//enter a speed boost object!
		
		if ((speedBoostTimer > noBoostFor ^ !speedBoosting) && other.gameObject.tag == "SpeedBoost" && controller.hAxis != 0){
			other.GetComponent<Animator>().Play(0);
			SpeedBoost();
		}
		
		other.SendMessage("CollideWithFitz", SendMessageOptions.DontRequireReceiver);
	}
	
	void PlayRestAnim (){
		if (anim && grounded){
			anim.Play(a.rest);
		}
	}
	
	void BeginRoll(){
		isRolling = true;
		anim.Play(a.roll);
	}
	
	void EndRoll(){
		isRolling = false;
		if (falling){
			anim.Play(a.fall);
		}
	}
	
	public bool CheckCaughtTennisBall(float leeway){
		if (controller.isSpammingRun) return false;
		
		return Time.time - controller.lastRunDownTime < leeway;
	}
	
	public void SpeedBoost(){
		tempMaxSpeed = maxSpeed * speedBoostMod;
		speedBoosting = true;
		speedBoostTimer = 0;
		speedBoostDirection = Mathf.RoundToInt(controller.hAxis);
	}
	
	public void BananaBoost(){
		SpeedBoost();
		Instantiate(Resources.Load("BananaPeel"), t.position + Vector3.up * box.height, t.rotation);
	}
}
