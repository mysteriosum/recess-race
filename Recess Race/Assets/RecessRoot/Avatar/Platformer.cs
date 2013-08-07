using UnityEngine;
using System.Collections;

public class Platformer : MonoBehaviour {
	
	public class Controller {
		public bool getRun;
		public bool getRunDown;
		public bool getRunUp;
		
		public bool getJump;
		public bool getJumpDown;
		public bool getJumpUp;
		
		public bool getL;
		public bool getLUp;
		public bool getLDown;
		
		public bool getR;
		public bool getRUp;
		public bool getRDown;
		
		public bool getD;
		public bool getDDown;
		public bool getDUp;
		
		public bool getU;
		public bool getUUp;
		public bool getUDown;
		
		public bool doubleTap;
		public bool aboutFace;
	
		public bool locked;
		
		private float hAxisLast = 0.0f;
		private float getLLastTime = 0f;
		private float getRLastTime = 0f;
		private float doubleTapTime = 0.2f;
		
		private Platformer parent;
		
		public Controller (Platformer parent){
			this.parent = parent;
		}
		
		public void GetInputs(){
			if (locked) return;
			
			getRun = Input.GetButton("Run");
			getRunDown = Input.GetButtonDown("Run");
			getRunUp = Input.GetButtonUp("Run");
			
			if (getRunDown && parent.RunDown != null)
				parent.RunDown();
			
			if (getRunUp && parent.RunUp != null)
				parent.RunUp();
			
			getJump = Input.GetButton("Jump");
			getJumpDown = Input.GetButtonDown("Jump");
			getJumpUp = Input.GetButtonUp("Jump");
			
			if (getJumpDown && parent.JumpDown != null)
				parent.JumpDown();
			
			if (getJumpUp && parent.JumpUp != null)
				parent.JumpUp();
			
			float hAxis = Input.GetAxis("Horizontal");
			
			getL = hAxis < -0.3f;
			getR = hAxis > 0.3f;
			
			getLDown = (hAxisLast < 0.3f && hAxisLast > -0.3f) && getL;
			getRDown = (hAxisLast < 0.3f && hAxisLast > -0.3f) && getR;
			
			getLUp = hAxisLast < -0.3f && !getL;
			getRUp = hAxisLast > 0.3f && !getR;
			
			if (getLDown){						//check for doubleTap
				if (Time.time - getLLastTime < doubleTapTime){
					doubleTap = true;
					if (parent.DoubleTap != null)
						parent.DoubleTap();
				}
				getLLastTime = Time.time;
			}
			
			if (getRDown){
				if (Time.time - getRLastTime < doubleTapTime){
					doubleTap = true;
					if (parent.DoubleTap != null)
						parent.DoubleTap();
				}
				getRLastTime = Time.time;
			}
			
			
			hAxisLast = hAxis;
		}
		
	}
	
	protected Controller controller;
	
	
	//[System.SerializableAttribute]
	public class MovementVariables {
		public float wSpeed;
		public float rSpeed;
		public float sSpeed;
		public float accel;
		public float decel;
		public float decelA;
		public float decelB;
		
		public float jHeight;
		public float jExtraHeight;
		public float airSpeedInit;
		public float airSpeedExtra = 0.15625f;
		public float runJumpIncrement = 0.5f;
		public float fallSpeedMax;
		public float gravity;
		public float gravityA;
		
		
		public MovementVariables(float wSpeed, float rSpeed, float sSpeed, float accel, float decel, float decelA, float decelB, float jHeight, float jExtraHeight, 
			float airSpeedH, float airSpeedInit, float fallSpeedMax, float gravity, float gravityA){
			this.wSpeed = wSpeed;
			this.rSpeed = rSpeed;
			this.sSpeed = sSpeed;
			this.accel = accel;
			this.decel = decel;
			this.decelA = decelA;
			this.decelB = decelB;
			this.jHeight = jHeight;
			this.jExtraHeight = jExtraHeight;
			this.airSpeedInit = airSpeedInit;
			this.fallSpeedMax = fallSpeedMax;
			this.gravity = gravity;
			this.gravityA = gravityA;
		}
		
	}
	
	public MovementVariables jumpman;
	
	public bool falling = false;
	public bool jumping = false;
	public bool grounded = false;
	
	public bool canMoveLeft = true;
	public bool canMoveRight = true;
	
	protected bool headBump = false;
	
	protected int jumpCounter = 0;
	
	protected float jumpImpulse = 0f;
	protected float jumpStartY;
	protected bool startJump = false;
	protected bool justLanded = false;
	
	protected Vector2 velocity;
	protected Vector2 lastVelocity;
	
	public Vector2 Velocity {
		get { return velocity; }
		set { velocity = value; }
	}
	
	protected Transform t;
	protected GameObject go;
	protected Rigidbody rb;
	protected BoxCollider bc;
	protected tk2dSpriteAnimator anim;
	protected tk2dSprite sprite;
	
	protected BoxCollider botDetector;
	protected BoxCollider leftDetector;
	protected BoxCollider rightDetector;
	protected BoxCollider topDetector;
	
	protected BoxCollider botCollider = null;
	protected BoxCollider leftCollider = null;
	protected BoxCollider rightCollider = null;
	protected BoxCollider topCollider = null;
	
	protected FitzDetectBox botDScript;
	protected FitzDetectBox leftDScript;
	protected FitzDetectBox rightDScript;
	protected FitzDetectBox topDScript;
	
	protected BoxCollider[] groundColliders;
	
	protected GameObject dummy;
	
	
	//LOTS OF DELEGATES! YEAAAAAAAAAAAAAAAAAAAAAAAH
	public delegate void InputDelegate();
	
	public InputDelegate StartJump;
	public InputDelegate RunDown;
	public InputDelegate RunUp;
	public InputDelegate JumpUp;
	public InputDelegate JumpDown;
	public InputDelegate DoubleTap;
	public InputDelegate AboutFace;
	//public InputDelegate OnLand;
	
	
	//events, wtf (IKR!)
	
	public delegate void OnLandEvent();
	public event OnLandEvent OnLand;
	
	// Use this for initialization
	void Start () {
		Setup();
	}
	
	protected virtual void Setup(){
		controller = new Controller(this);
		
		t = transform;
		rb = rigidbody;
		go = gameObject;
		sprite = GetComponentInChildren<tk2dSprite>();
		anim = GetComponentInChildren<tk2dSpriteAnimator>();
		
		bc = GetComponent<BoxCollider>();
		groundColliders = GameObject.Find("collisions").GetComponentsInChildren<BoxCollider>();
		
		botDetector = GameObject.Find(name + "/downDetector").GetComponent<BoxCollider>();
		leftDetector = GameObject.Find(name + "/leftDetector").GetComponent<BoxCollider>();
		rightDetector = GameObject.Find(name + "/rightDetector").GetComponent<BoxCollider>();
		topDetector = GameObject.Find(name + "/topDetector").GetComponent<BoxCollider>();
		
		botDScript = botDetector.GetComponent<FitzDetectBox>();
		leftDScript = leftDetector.GetComponent<FitzDetectBox>();
		rightDScript = rightDetector.GetComponent<FitzDetectBox>();
		topDScript = topDetector.GetComponent<FitzDetectBox>();
		
		dummy = GameObject.Find(name + "/dummy");
		bc.size = new Vector3(sprite.CurrentSprite.colliderVertices[1].x * 2, sprite.CurrentSprite.colliderVertices[1].y * 2, 10);
		FitDetectors();
	}
	
	protected void FitDetectors(){
		bc.size = new Vector3(sprite.CurrentSprite.colliderVertices[1].x * 2, sprite.CurrentSprite.colliderVertices[1].y * 2, 10);
		
		//new sizes: 						x										y													z
		botDetector.size = 		new Vector3	(bc.size.x - 1, 						Mathf.Abs(Mathf.Min(velocity.y, 0)), 				botDetector.size.z);
		leftDetector.size =	 	new Vector3	(Mathf.Abs(Mathf.Min(velocity.x, 0)), 	bc.size.y - Mathf.Max(Mathf.Abs(velocity.y) *2, 2),	leftDetector.size.z);
		rightDetector.size = 	new Vector3	(Mathf.Max(velocity.x, 0), 				bc.size.y - Mathf.Max(Mathf.Abs(velocity.y) *2, 2),	rightDetector.size.z);
		topDetector.size = 		new Vector3	(bc.size.x - 1, 						Mathf.Max(velocity.y, 0), 							topDetector.size.z);
		
		//new positions: 										x											y											z = 0
		botDetector.transform.localPosition = 		new Vector3(0, 											-bc.size.y / 2 - botDetector.size.y / 2, 	0);
		leftDetector.transform.localPosition = 		new Vector3(-bc.size.x / 2 - leftDetector.size.x / 2, 	0, 											0);
		rightDetector.transform.localPosition = 	new Vector3(bc.size.x / 2 + rightDetector.size.x / 2, 	0, 											0);
		topDetector.transform.localPosition = 		new Vector3(0, 											bc.size.y / 2 + topDetector.size.y / 2, 	0);
	}
	protected void FitDetectorsOnComplete(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip){
		bc.size = new Vector3(sprite.CurrentSprite.colliderVertices[1].x * 2, sprite.CurrentSprite.colliderVertices[1].y * 2, 10);
		FitDetectors();
	}
	// Update is called once per frame
	void Update () {
		
		controller.GetInputs();
		
		CheckStates();
		
		FitDetectors();
		
		ApplyMovement();
	}
	
	protected virtual void Cleanup(){
		justLanded = false;
		startJump = false;
		controller.doubleTap = false;
		controller.aboutFace = false;
	}
	
	public virtual void HitTop(BoxCollider topCollider){
		if (jumping && !rightDScript.KnowsOf(topCollider) && !leftDScript.KnowsOf(topCollider)){
			
			falling = true;
			jumping = false;
			headBump = true;
			velocity = new Vector2(velocity.x, 0);
		}
	}
	
	public virtual void HitBottom(BoxCollider botCollider){
		if (!grounded && velocity.y <= 0){
			grounded = true;
			jumping = false;
			falling = false;
			justLanded = true;
			velocity = new Vector2(velocity.x, 0);
			t.position = new Vector3(t.position.x, botCollider.bounds.center.y + botCollider.size.y / 2 + bc.size.y / 2, 0);
			OnLand();
		}
	}
	
	public virtual void HitRight(BoxCollider rightCollider){
		if (botDScript.KnowsOf(rightCollider) || topDScript.KnowsOf(rightCollider)){
			Debug.Log("I know this one already, on the top or bottom thing or whatever");
			return;
		}
		canMoveRight = false;
		velocity = new Vector2(0, velocity.y);				//reset velocity and set myself to the correct position if I'm up against a wall
		t.position = new Vector3(rightCollider.collider.bounds.center.x - rightCollider.collider.bounds.size.x / 2 - bc.bounds.size.x / 2, t.position.y, t.position.z);
	}
	
	public virtual void HitLeft(BoxCollider leftCollider){
		if (botDScript.KnowsOf(leftCollider) || topDScript.KnowsOf(leftCollider)){
			Debug.Log("I know this one already, on the top or bottom thing or whatever");
			return;
		}
		canMoveLeft = false;
		velocity = new Vector2(0, velocity.y);				//reset velocity and set myself to the correct position if I'm up against a wall
		t.position = new Vector3(leftCollider.collider.bounds.center.x + leftCollider.collider.bounds.size.x / 2 + bc.bounds.size.x / 2, t.position.y, t.position.z);
	}
	
	public virtual void NothingBottom(){
		grounded = false;
	}
	
	public virtual void NothingLeft(){
		canMoveLeft = true;
	}
	
	public virtual void NothingRight(){
		canMoveRight = true;
	}
	
	public virtual void NothingTop(){
		
	}
	
	public virtual void DetectorExit(BoxCollider detector, BoxCollider colExiting){
		if (detector == botDetector){
			NothingBottom();
		}
		else if (detector == leftDetector){
			NothingLeft();
		}
		else if (detector == rightDetector){
			NothingRight();
		}
		else if (detector == topDetector){
			NothingTop();
		}
	}
	
	public virtual void DetectorEnter (BoxCollider detector, BoxCollider colEntering){
		if (colEntering.gameObject.layer == 31 || colEntering.gameObject.layer == 30 || colEntering.gameObject.layer == 29){
			if (detector == rightDetector){
				HitRight(colEntering);
			}
			else if (detector == leftDetector){
				HitLeft(colEntering);
			}
			else if (detector == botDetector){
				HitBottom(colEntering);
			}
			else if (detector == topDetector){
				HitTop(colEntering);
			}
		}
	}
	
	protected void JumpmanUpdate(){
		
		if (controller.getL){
			velocity = new Vector2(-jumpman.wSpeed, velocity.y);		//Get whether the guy is going left
		}
		
		if (controller.getR){
			velocity = new Vector2(jumpman.wSpeed, velocity.y);
		}
		
		
		if (jumping){
			if (t.position.y >= jumpStartY + jumpman.jHeight){
				jumping = false;
			}
			else{
				velocity = new Vector2(velocity.x, jumpman.airSpeedInit);
			}
		}
		
		if (falling){
			velocity = new Vector2(velocity.x, Mathf.Max(-jumpman.fallSpeedMax, lastVelocity.y - jumpman.gravity));
		}
		
		if (headBump){
			velocity = new Vector2(velocity.x, 0);
			headBump = false;
		}
	}
	
	protected void CheckStates(){
	
		if (velocity.x > 0){						//sprite directions. Check what direction I'm facing and flip sprite accordingly
			sprite.scale = new Vector3(1, 1, 1);
			canMoveLeft = true;
		}
		
		if (velocity.x < 0){
			sprite.scale = new Vector3(-1, 1, 1);
			canMoveRight = true;
		}
		
		if (!grounded && controller.getL){
			sprite.scale = new Vector3(-1, 1, 1);
		}
		
		if (!grounded && controller.getR){
			sprite.scale = new Vector3(1, 1, 1);
		}
		
		
		
		
	}
	
	protected void ApplyMovement(){
		t.Translate(velocity);
		lastVelocity = new Vector2(velocity.x, velocity.y);
		
		t.rotation = Quaternion.identity;
	}
	
	
	protected void GetOutOfWall(BoxCollider detector, BoxCollider other){
		int starter = 1;
		int counter = 1;
		int modifier = (int) bc.bounds.size.x;
		if (Fitz.fitz.transform.position.x < t.position.x)
			starter = -1;
		int changeat = starter;
		while (other.bounds.Contains(t.position)){
			t.position += new Vector3(starter * counter * modifier, (counter - 1) * modifier, 0);
			
			starter *= -1;
			if (starter == changeat)
				counter ++;
			
			if (counter > 10){
				Debug.Log("It's gotten out of hand");
				break;
			}
		}
		
		//now check if I'm in the avatar or something
		
	}
}
