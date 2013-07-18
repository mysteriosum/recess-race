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
		
		public float hAxisLast = 0.0f;
		public float hAxisLastTime = 0.0f;
		
		public bool getD;
		public bool getDDown;
		public bool getDUp;
		
		public bool getU;
		public bool getUUp;
		public bool getUDown;
		
		public bool doubleTap;
		public bool aboutFace;
	
		public bool locked;
		
		public void GetInputs(){
			if (locked) return;
			
			getRun = Input.GetButton("Run");
			getRunDown = Input.GetButtonDown("Run");
			getRunUp = Input.GetButtonUp("Run");
			
			getJump = Input.GetButton("Jump");
			getJumpDown = Input.GetButtonDown("Jump");
			getJumpUp = Input.GetButtonUp("Jump");
			
			float hAxis = Input.GetAxis("Horizontal");
			
			getL = hAxis < -0.3f;
			getR = hAxis > 0.3f;
			
			getLDown = (hAxisLast < 0.3f && hAxisLast > -0.3f) && getL;
			getRDown = (hAxisLast < 0.3f && hAxisLast > -0.3f) && getR;
			
			getLUp = hAxisLast < -0.3f && !getL;
			getRUp = hAxisLast > 0.3f && !getR;
			
			hAxisLast = hAxis;
			hAxisLastTime = Time.time;
		}
		
	}
	
	protected Controller controller = new Controller();
	
	
	//[System.SerializableAttribute]
	public class MovementVariables {
		public float wSpeed;
		public float rSpeed;
		public float sSpeed;
		public float accel;
		public float decel;
		public float skidDecel;
		public float rSkidDecel;
		
		public float jHeight;
		public float jExtraHeight;
		public float airSpeedInit;
		public float airSpeedExtra = 0.15625f;
		public float runJumpIncrement = 0.5f;
		public float airAccel;
		public float fallSpeedMax;
		public float gravity;
		public float gravityPlus;
		
		public MovementVariables(float wSpeed, float rSpeed, float sSpeed, float accel, float decel, float skidDecel, float rSkidDecel, float jHeight, float jExtraHeight, 
			float airSpeedH, float airSpeedInit, float fallSpeedMax, float gravity, float gravityPlus){
			this.wSpeed = wSpeed;
			this.rSpeed = rSpeed;
			this.sSpeed = sSpeed;
			this.accel = accel;
			this.decel = decel;
			this.skidDecel = skidDecel;
			this.rSkidDecel = rSkidDecel;
			this.jHeight = jHeight;
			this.jExtraHeight = jExtraHeight;
			this.airSpeedInit = airSpeedInit;
			this.airAccel = airAccel;
			this.fallSpeedMax = fallSpeedMax;
			this.gravity = gravity;
			this.gravityPlus = gravityPlus;
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
	
	protected BoxCollider[] groundColliders;
	
	
	// Use this for initialization
	void Start () {
		Setup();
	}
	
	protected virtual void Setup(){
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
		
	}
	
	// Update is called once per frame
	void Update () {
		controller.GetInputs();
		
		CheckStates();
		
		//JumpmanUpdate();
		
		ApplyMovement();
	}
	
	protected virtual void Cleanup(){
		justLanded = false;
		startJump = false;
		controller.doubleTap = false;
		controller.aboutFace = false;
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
			Debug.Log(velocity);
		}
		
		if (headBump){
			velocity = new Vector2(velocity.x, 0);
			headBump = false;
		}
	}
	
	protected void CheckStates(){
	
		
		BoxCollider botCollider = null;
		BoxCollider leftCollider = null;
		BoxCollider rightCollider = null;
		BoxCollider topCollider = null;
		
		foreach (BoxCollider box in groundColliders){			//Check each of my colliders to see if I have something in them
			if (botDetector.bounds.Intersects(box.bounds)){
				if (botCollider == null){
					botCollider = box;
				}
				else if (bc.bounds.Intersects(box.bounds)){
						botCollider = box;
				}
			}
			if (leftDetector.bounds.Intersects(box.bounds)){
				if (leftCollider == null){
					leftCollider = box;
				}
				else if (bc.bounds.Intersects(box.bounds)){
						leftCollider = box;
				}
			}
			if (rightDetector.bounds.Intersects(box.bounds)){
				if (rightCollider == null){
					rightCollider = box;
				}
				else if (bc.bounds.Intersects(box.bounds)){
						rightCollider = box;
				}
			}
			if (topDetector.bounds.Intersects(box.bounds)){
				if (topCollider == null){
					topCollider = box;
				}
				else if (bc.bounds.Intersects(box.bounds)){
						topCollider = box;
				}
			}
			
		}
		
		if (botCollider != null){
			if (bc.bounds.Intersects(botCollider.bounds)){		
													//Check if I should be grounded
				if (!grounded && velocity.y <= 0){
					grounded = true;
					jumping = false;
					falling = false;
					justLanded = true;
					velocity = new Vector2(velocity.x, 0);
					t.position = new Vector3(t.position.x, botCollider.bounds.center.y + botCollider.size.y / 2 + bc.size.y / 2, 0);
				}
			}
			else {
				grounded = false;
				falling = true;
			}
			
		}
		else{
			if (!jumping){				//I didn't jump, so I'm falling
				falling = true;
				grounded = false;
			}
		}
		//no more checking bottom
		
		
		if (rightCollider != null){
			if (bc.bounds.Intersects(rightCollider.bounds)){
				canMoveRight = false;
				velocity = new Vector2(0, velocity.y);				//reset velocity and set myself to the correct position if I'm up against a wall
				t.position = new Vector3(rightCollider.collider.bounds.center.x - rightCollider.collider.bounds.size.x / 2 - bc.bounds.size.x / 2, t.position.y, t.position.z);
			}
			else{
				canMoveRight = true;
			}
		}
		else{
			canMoveRight = true;
		}
		
		if (leftCollider != null){
			if (bc.bounds.Intersects(leftCollider.bounds)){
				canMoveLeft = false;
				velocity = new Vector2(0, velocity.y);				//reset velocity and set myself to the correct position if I'm up against a wall
				t.position = new Vector3(leftCollider.collider.bounds.center.x + leftCollider.collider.bounds.size.x / 2 + bc.bounds.size.x / 2, t.position.y, t.position.z);
			}
			else{
				canMoveLeft = true;
			}
		}
		else{
			canMoveLeft = true;
		}
		
		if (velocity.x > 0){						//sprite directions. Check what direction I'm facing and flip sprite accordingly
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
		
		
		
		if (grounded && controller.getJumpDown){			//initiate the JUMP action!
			jumping = true;
			grounded = false;
			jumpStartY = t.position.y;
			startJump = true;
		}
		
		if (jumping){
			jumpCounter ++;
			if (topCollider != null){
				if (bc.bounds.Intersects(topCollider.bounds) && !rightDetector.bounds.Intersects(topCollider.bounds) && !leftDetector.bounds.Intersects(topCollider.bounds)){
					falling = true;
					jumping = false;
					headBump = true;
					velocity = new Vector2(velocity.x, 0);
					Debug.Log("Hahah yeah right");
				}
			}
		}
	}
	
	
	protected void ApplyMovement(){
		
		
		t.Translate(velocity);
		lastVelocity = new Vector2(velocity.x, velocity.y);
		//velocity = Vector2.zero;
	}
}
