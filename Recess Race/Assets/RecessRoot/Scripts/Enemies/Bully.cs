using UnityEngine;
using System.Collections;

public class Bully : Platformer {
	
	public MovementVariables movement = new MovementVariables
		(
			2.25f, 				//wSpeed
			1.25f,  			//rSpeed
			3f,  				//sSpeed
			
			0.18375f,  			//accel
			0.065f,  			//decel
			0.15625f,  			//skidDecel
			0.3125f,  			//running SkidDecel
			64f,  				//jHeight
			32f,  				//jExtraHeight
			1.25f,  			//airSpeedH
			6.3125f,  			//airSpeedInit
			4f,  				//fallSpeedMax
			0.1875f,  			//gravity
			0.375f 				//gravityPlus
		);
	
	protected Transform currentNode;
	protected Transform goalNode;
	protected int currentFloor = 0;
	public int CurrentFloor {
		get { return currentFloor; }
	}
	
	private float[] jumpHeights = new float[3]{4.2f, 5.8f, 6.3125f};
	
	public int successChance;
	public float speed;
	
	public static Bully brandon;
	public static Bully ashley;
	public static Bully romney;
	
	protected delegate void HarassDelegate ();
	protected HarassDelegate Harrass;
	
	protected string message;
	protected string warningMessage;
	protected string otherMessage;
	
	private delegate void JumpDelegate(int height);
	private JumpDelegate JumpDown;
	
	protected void BrandonHarrass () {
		if (currentFloor == 0){
			TextBox.bubble.Open("Brandon", warningMessage);
		}
		else {
			Fitz.fitz.Dying = true;
			TextBox.bubble.Open("Brandon", message);
		}
		
	}
	protected void AshleyHarrass () {
		TextBox.bubble.Open("Ashley", message);
	}
	
	protected void RomneyHarrass () {
		
		if (currentFloor == Bully.brandon.CurrentFloor && currentFloor != 0){
			Fitz.fitz.Dying = true;
			TextBox.bubble.Open("Brandon", message);
		}
		else if (currentFloor == 0 && currentFloor == Bully.ashley.CurrentFloor){
			
			TextBox.bubble.Open("Romney", otherMessage);
		}
		else{
			
			TextBox.bubble.Open("Romney", warningMessage);
		}
		
	}
	
	// Use this for initialization
	void Start () {
		Setup();
	}
	
	protected override void Setup () {
		base.Setup();
		anim = GetComponentInChildren<tk2dSpriteAnimator>();
		JumpDown = delegate(int height) {
			if (!grounded) return;
			velocity = new Vector2(velocity.x, jumpHeights[height]);
			anim.Play(a_jump);
			jumping = true;
			grounded = false;
		};
		
		JumpUp = delegate() {
			velocity = new Vector2(velocity.x, 0);
		};
		
		Gravity = delegate() {
			if (grounded) return;
			velocity += new Vector2(0, -movement.gravity);
		};
		
		OnLand += delegate() {
			velocity = new Vector2(velocity.x, 0);
			anim.Play(a_walk);
		};
		
	}
	
	// Update is called once per frame
	void Update () {
		
		FitDetectors();
		CheckStates();
		
		if (controller.getL && canMoveLeft){
			velocity = new Vector2(Mathf.Max(-movement.wSpeed, velocity.x - movement.accel), velocity.y);
			FacingRight = false;
		}
		
		if (controller.getR && canMoveRight){
			velocity = new Vector2(Mathf.Min(movement.wSpeed, velocity.x + movement.accel), velocity.y);
			FacingRight = true;
		}
		
		if (!controller.getR && !controller.getL){
			velocity += new Vector2(velocity.x < 0? movement.accel : -movement.accel, 0);
			if (velocity.x > -movement.accel * 2 && velocity.x < movement.accel){
				velocity = new Vector2(0, velocity.y);
			}
		}
		
		ApplyMovement();
	}
	
	void OnTriggerEnter (Collider other){
		if (other.tag == "moveLeft"){
			controller.getL = true;
			controller.getR = false;
		}
		else if (other.tag == "moveRight"){
			controller.getR = true;
			controller.getL = false;
		}
		else if (other.tag == "stop" || other.tag == "jumpStraight"){
			controller.getR = false;
			controller.getL = false;
		}
		else if (other.tag == "fall"){
			JumpUp();
		}
		
		
	}
	
	void OnCollisionEnter (Collision other){
		if (other.gameObject.tag == "fitzwilliam"){
			try {
				Harrass();
			}
			catch (System.NullReferenceException exception){
				Debug.Log("There's no Harass delegate");
			}
		}
		else if (other.gameObject.tag == "checkpoint"){
			Checkpoint cp = other.gameObject.GetComponent<Checkpoint>();
			if (cp){
				if (currentFloor == cp.Index){
					currentFloor ++;
				}
			}
		}
	}
	
	void OnTriggerStay (Collider other){
		BullyInstruction inst = other.GetComponent<BullyInstruction>();
		
		if (other.tag == "jumpLeft" && (velocity.x <= -movement.wSpeed || !canMoveLeft)){
			JumpDown((int)inst.jumpHeight);
		}
		else if (other.tag == "jumpRight" && (velocity.x >= movement.wSpeed || !canMoveRight)){
			JumpDown((int)inst.jumpHeight);
		}
		else if (other.tag == "jumpStraight" && velocity.x == 0){
			JumpDown((int)inst.jumpHeight);
		}
	}
}