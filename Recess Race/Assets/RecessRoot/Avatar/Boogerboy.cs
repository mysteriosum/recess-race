using UnityEngine;
using System.Collections;

public class Boogerboy : Character {
	
	protected override float Acceleration {
		get {
			if (Dashing)
				return acceleration * dashSpeedMod;
			if (WallJumping)
				return acceleration * wallHangFallSpeedMod;
			
			return base.Acceleration;
		}
	}
	
	protected override float MaxSpeed {
		get {
			return base.MaxSpeed * (Dashing? dashSpeedMod : 1);
		}
	}
	
	protected override float MaxFallSpeed {
		get {
			return base.MaxFallSpeed * (wallHanging? wallHangFallSpeedMod : 1);
		}
	}
	
	protected override string A_walk {
		get {
			return Dashing? "dash" : endingDash? "endDash" : base.A_walk;
		}
	}
	
	protected override string A_fall {
		get {
			return wallHanging? "wallGrab" : base.A_fall;
		}
	}
	
	protected override string A_jump {
		get {
			return WallJumping? "wallJump" : base.A_jump;
		}
	}
	
	protected bool Dashing {
		get { return dashTimer > 0; }
		set {
			if (!value && anim.IsPlaying("dash")){
				anim.Play("endDash");
				endingDash = true;
				anim.AnimationCompleted = DoneEndDash;
			}
			dashTimer = value? dashTiming : 0; 
		}
	}
	
	protected bool WallJumping {
		get { return wallJumpLockTimer > 0; }
		set {
			wallJumpLockTimer = value? wallJumpLockTiming : 0;
			
		}
	}
	
	public float dashSpeedMod;
	public float dashTiming;
	public float wallJumpLockTiming;
	public float wallHangFallSpeedMod = 0.5f;
	
	private bool endingDash = false;
	private bool wallHanging = false;
	private float wallJumpLockTimer = 0;
	
	private float runTimer;
	
	private bool RunInput {
		get { return runTimer < maxPress2Jump; }
	}
	
	
	protected override float HorizontalInput {
		get {
			float temp = base.HorizontalInput;
			if (Dashing && grounded){
				if ((FacingRight && temp < 0) || (!FacingRight && temp > 0)){
					Dashing = false;
				}
			}
			
			if (WallJumping){
				temp = 0;
			}
			return temp;
		}
	}
	
	private float dashTimer;
	void Awake() {
		base.Awake();
		name = "Fitzwilliam";
	}
	// Use this for initialization
	void Start () {
		base.Start();
		doubleTap = Dash;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Dashing && grounded){
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0){
				Dashing = false;
			}
		}
		
		if (WallJumping){
			wallJumpLockTimer -= Time.deltaTime;
		}
		
		if (!Input.GetButton("Jump") && velocity.y > 0 && !WallJumping){
			velocity = new Vector2(velocity.x, 0);
		}
		
		base.Update();
		
		
	}
	
	void Dash () {
		if (!grounded) return;
		Dashing = true;
	}
	
	void OnLand () {
		Dashing = false;
		
	}
	
	void DoneEndDash (tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip) {
		endingDash = false;
		anim.AnimationCompleted = null;
	}
	
	
	protected override void DoInputs ()
	{
		if (Input.GetButton("Run")){
			if (runTimer == float.MaxValue)
				runTimer = 0;
			runTimer += Time.deltaTime;
		}
		else{
			runTimer = float.MaxValue;
		}
		
		if (grounded){						//whether I should jump
			if (RunInput && !disabled && !Dashing){
				//do the jump! Add the last part there to give myself a running jump
				Dash();
			}
		}
		
		base.DoInputs ();
		
		
		
		
		
		if (wallHanging && HorizontalInput == 0){
			StopWallHanging();
		}
		
		if (wallHanging && Mathf.Abs(velocity.x) > Acceleration){
			StopWallHanging();
		}
		
		if (JumpInput && !WallJumping && wallHanging){
			velocity = new Vector2(MaxSpeed * (FacingRight? -1 : 1), initialJumpVelocity);
			WallJumping = true;
			wallHanging = false;
			falling = false;
			Debug.Log("JUMP");
			Debug.Log(velocity);
			anim.Play(A_jump);
		}
		
		
		
		
		
	}
	
	void HitWall () {
		if (!wallHanging){
			wallHanging = true;
			if (velocity.y < 0){
				anim.Play(A_fall);
			}
		}
		Dashing = false;
		
	}
	
	void StopWallHanging () {
		wallHanging = false;
		anim.Play(A_fall);
	}
	
	void RefreshJumpAnimation () {
		anim.Play(A_jump);
		anim.AnimationCompleted = null;
	}
}
