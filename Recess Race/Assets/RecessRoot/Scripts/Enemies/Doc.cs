using UnityEngine;
using System.Collections;

public class Doc : Platformer {
	private BoxCollider tlDetector;
	private BoxCollider trDetector;
	private BoxCollider blDetector;
	private BoxCollider brDetector;
	
	protected FitzDetectBox blDScript;
	protected FitzDetectBox brDScript;
	protected FitzDetectBox tlDScript;
	protected FitzDetectBox trDScript;
	
	private bool goingRight;
	private bool turningAround;
	private bool hiding;
	private bool charging;
	private bool hurt;
	private bool rolling;
	private bool disabled;
	private bool Disabled {
		set {
			anim.Stop();
			disabled = value;
			t.position = new Vector3 (t.position.x, t.position.y, disabled? -3000 : 0);
		}
	}
	
	private bool Offset {
		set { 
			t.position = new Vector3 (t.position.x, t.position.y, value? -75 : 0);
		}
	}
	
	private bool held;
	public bool Held{
		get { return held; }
		set {
			held = value;
			Offset = value;
			if (value && anim.IsPlaying(a_roll)){
				anim.StopAndResetFrame();
			}
		}
	}
	
	public bool Dangerous {
		get { 
			bool goingOppositDirections = rolling && (velocity.x * Fitz.fitz.Velocity.x < 0);
			return !hurt || goingOppositDirections;
		}
	}
	
	private bool TurningAround {
		set { turningAround = value;
			Debug.Log("Setting turningAround");
			if (value){
				anim.Play(a_turn);
				anim.AnimationEventTriggered = FlipSprite;
				velocity = new Vector2(0, velocity.y);
			}
			else{
				goingRight = dummy.transform.localScale.x > 0.9f;
				velocity = new Vector2(movement.wSpeed * dummy.transform.localScale.x, velocity.y);
				anim.Play(a_walk);
			}
		}
	}
	
	private bool Hiding {
		set { 
			hiding = value;
			if (value){
				if (!charging){
					anim.Play(a_hide);
				}
				else if (anim.IsPlaying(a_spin)){
					anim.StopAndResetFrame();
				}
				charging = false;
				velocity = Vector2.zero;
				anim.AnimationCompleted = FitDetectorsOnComplete;
				refreshTimer = refreshTiming;
			}
			else {
				anim.Play(a_emerge);
				hp = maxHP;
				hurt = false;
				anim.AnimationCompleted = BeginTurning;
				velocity = emergeVelocity;
			}
		}
	}
	
	private bool Rolling {
		set {
			rolling = value;
			if (value){
				velocity = new Vector2(Fitz.fitz.transform.position.x > t.position.x? -movement.rSpeed : movement.rSpeed, velocity.y);
				anim.Play(a_roll);
			}
		}
	}
	
	private int collisionMask;
	
	
	private int hp;
	private int maxHP = 9;
	private int hideThreshold = 4;
	private int refreshTiming = 300;
	private int refreshTimer = 0;
	private Vector2 emergeVelocity = Vector2.up * 2;
	
	private string a_walk = "walk";
	private string a_turn = "turn";
	private string a_hide = "hide";
	private string a_emerge = "emerge";
	private string a_spin = "spin";
	private string a_roll = "roll";
	
	
	public MovementVariables movement = new MovementVariables
		(
			1.25f, 				//wSpeed
			2.25f,  			//rSpeed
			3f,  				//sSpeed
			
			0.09375f,  			//accel
			0.0325f,  			//decel
			0.15625f,  			//skidDecel
			0.3125f,  			//running SkidDecel
			64f,  				//jHeight
			32f,  				//jExtraHeight
			1.25f,  			//airSpeedH
			4.8125f,  			//airSpeedInit
			4f,  				//fallSpeedMax
			0.1875f,  			//gravity
			0.375f 				//gravityPlus
		);
	
	// Use this for initialization
	void Start () {
		Setup();
		
		hp = maxHP;
		
		collisionMask = 1 << LayerMask.NameToLayer("normalCollision") | 1 << LayerMask.NameToLayer("softBottom") | 1 << LayerMask.NameToLayer("softTop");
		
		goingRight = dummy.transform.localScale.x > 0.9f;
		Debug.Log ("I'm " + gameObject.GetInstanceID());
		velocity = new Vector2(movement.wSpeed * dummy.transform.localScale.x, velocity.y);
		
		blDetector = GameObject.Find(name + "/blDetector").GetComponent<BoxCollider>();
		brDetector = GameObject.Find(name + "/brDetector").GetComponent<BoxCollider>();
		tlDetector = GameObject.Find(name + "/tlDetector").GetComponent<BoxCollider>();
		trDetector = GameObject.Find(name + "/trDetector").GetComponent<BoxCollider>();
		
		blDetector.transform.localPosition = new Vector3(-bc.size.x / 2 - blDetector.size.x / 2 - 0.05f, -bc.size.y / 2 - blDetector.size.y / 2 - 0.05f, 0);
		brDetector.transform.localPosition = new Vector3(bc.size.x / 2 + brDetector.size.x / 2 + 0.05f, -bc.size.y / 2 - brDetector.size.y / 2 - 0.05f, 0);
		tlDetector.transform.localPosition = new Vector3(-bc.size.x / 2 - tlDetector.size.x / 2 - 0.05f, bc.size.y / 2 + tlDetector.size.y / 2 + 0.05f, 0);
		trDetector.transform.localPosition = new Vector3(bc.size.x / 2 + trDetector.size.x / 2 + 0.05f, bc.size.y / 2 + trDetector.size.y / 2 + 0.05f, 0);
		
		anim.AnimationEventTriggered = FlipSprite;
		anim.AnimationCompleted = KeepGoing;
		
		//Tell the player what I want to do!
		Fitz.fitz.OnLand += CheckForPlayer;
		OnLand += CheckForPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (disabled) return;
		
		FitDetectors();
		ApplyMovement();
		CheckDirection();
		CheckStates();
		
		DocUpdate();
	}
	
	private void DocUpdate () {
		//here's where I do my calculations for Doc, my medicine ball enemy! :D
		
		
		if (rolling && grounded){
			if (velocity.x > movement.decel) {
				velocity += new Vector2(-movement.decel, 0);
			}
			else if (velocity.x < -movement.decel) {
				velocity += new Vector2(movement.decel, 0);
			}
			
			if (velocity.x <= movement.decel && velocity.x >= -movement.decel && grounded){
				rolling = false;
				velocity = Vector2.zero;
			}
		}
		
		if (!grounded && !held){
			velocity = new Vector2(velocity.x, Mathf.Min(velocity.y - movement.gravity, movement.fallSpeedMax));
		}
		
		if (refreshTimer > 0){
			refreshTimer --;
			if (refreshTimer == 0){
				Hiding = false;
			}
		}
	}
	
	public override void NothingLeft(){
		
	}
	
	public override void NothingRight(){
		
	}
	
	public override void HitRight(BoxCollider rightCollider){
	//	Vector2 prevV = velocity;
		if (rolling){
			Offset = true;
			return;
		}
		base.HitRight(rightCollider);
		if (anim.IsPlaying(a_spin)){
			EndCharge();
			Debug.Log("Ending Charge");
		}
		else if (anim.IsPlaying(a_walk)){
			TurningAround = true;
		}/*
		else if (anim.IsPlaying(a_roll)){
			velocity = new Vector2(prevV.x * -1, prevV.y);
			dummy.transform.localScale = new Vector3(dummy.transform.localScale.x * -1, 1, 1);
		}*/
		
		//GetOutOfWall(rightDetector, rightCollider);
		Debug.Log("hit right");
	}
	
	public override void HitLeft(BoxCollider leftCollider){
		Vector2 prevV = velocity;
		if (rolling){
			Offset = true;
			return;
		}
		base.HitLeft(leftCollider);
		if (anim.IsPlaying(a_spin)){
			EndCharge();
		}
		else if (anim.IsPlaying(a_walk)){
			TurningAround = true;
		}/*
		else if (anim.IsPlaying(a_roll)){
			velocity = new Vector2(prevV.x * -1, prevV.y);
			dummy.transform.localScale = new Vector3(dummy.transform.localScale.x * 1, 1, 1);
		}
		Debug.Log("Hit left");*/
	}
	
	
	
	public override void DetectorExit (BoxCollider detector, BoxCollider colExiting)
	{
		base.DetectorExit (detector, colExiting);
		if (botDScript.KnowsOf(colExiting)){
			if (detector == blDetector && !goingRight){
				RaycastHit hit;
				bool linecast = Physics.Linecast(leftDetector.transform.position, blDetector.transform.position, out hit, collisionMask);
				if (!linecast){
					if (charging){
						EndCharge();
					}
					else if (!hiding){
						TurningAround = true;
					}
					canMoveLeft = false;
					Debug.Log("Turning around from !goingRight detector exit");
				}
			}
			if (detector == brDetector && goingRight){
				RaycastHit hit;
				bool linecast = Physics.Linecast(rightDetector.transform.position, brDetector.transform.position, out hit, collisionMask);
				if (!linecast){
					if (charging){
						EndCharge();
					}
					else if (!hiding){
						TurningAround = true;
					}
					canMoveRight = false;
					Debug.Log("Turning around from goingRight detector exit");
				}
			}
		}
		
	}
	
	public void FlipSprite(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip, int index) {
		dummy.transform.localScale = new Vector3(dummy.transform.localScale.x * -1, 1, 1);
		anim.AnimationEventTriggered = null;
	}
	
	public void KeepGoing(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip){
		TurningAround = false;
		CheckForPlayer ();
	}
	
	public void CheckForPlayer (){
		
		if (!grounded || hiding || charging) return;
		
		Ray ray = goingRight? new Ray(t.position, Vector3.right) : new Ray(t.position, Vector3.left);
		RaycastHit hit;
		int mask = 1 << LayerMask.NameToLayer("normalCollision") | 1 << LayerMask.NameToLayer("avatar");
		bool raycast = Physics.Raycast(ray, out hit, 640, mask);
		if (raycast){
			if (hit.collider.gameObject == Fitz.fitz.gameObject){
				anim.Play(a_hide);
				velocity = Vector2.zero;
				anim.AnimationCompleted = BeginCharge;
			}
		}
	}
	
	public void BeginCharge (tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip){
		velocity = new Vector2(movement.rSpeed * dummy.transform.localScale.x, velocity.y);
		anim.Play(a_spin);
		charging = true;
	}
	
	public void EndCharge(){
		velocity = emergeVelocity;
		anim.Play(a_emerge);
		anim.AnimationCompleted = BeginTurning;
		charging = false;
	}
	
	public void BeginTurning (tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip){
		
		FitDetectors();
		Debug.Log("I said to fit the detectors so I do'nt know why it didn't work");
		TurningAround = true;
		charging = false;
		anim.AnimationCompleted = KeepGoing;
		//velocity = Vector2.zero;
	}
	
	public void BeginTurning (){
		BeginTurning(anim, anim.CurrentClip);
	}
	
	public bool JumpedOn (bool getRun){
		
		hp = hideThreshold;
		hurt = true;
		if (!hiding){
			Hiding = true;
			return true;
		}
		else{
			if (!getRun){
				velocity = new Vector2(Fitz.fitz.transform.position.x > t.position.x? movement.rSpeed : -movement.rSpeed, 0);
				Rolling = true;
			}
			return false;	
		}
	}
	
	public void Hurt (int amount){
		hp -= amount;
		if (hp <= hideThreshold && hp > 0){
			hurt = true;
			if (!hiding){
				Hiding = true;
			}
		}
		else if (hp <= 0){		//insert explodo animation
			Debug.Log("BOOM!");
			Destroy(gameObject);
		}
		
	}
	
	public void Kicked(){
		Kicked(0);
	}
	
	public void Kicked (float extra){
		Disabled = false;
		Debug.Log("extra is " + extra);
		//t.position += Vector3.up * 8;
		velocity = new Vector2(Fitz.fitz.transform.position.x > t.position.x? movement.rSpeed + extra : -movement.rSpeed - extra, 1f);
		Debug.Log("VeLOCITy on KIck" + velocity);
		Rolling = true;
		held = false;
	}
	
	public void Inhaled(){
		Disabled = true;
		velocity = Vector2.zero;
		charging = false;
		hiding = true;
		anim.Play(a_hide);
		hurt = true;
		hp = hideThreshold;
		anim.AnimationCompleted = null;
		anim.AnimationEventTriggered = null;
	}
	
	
}
