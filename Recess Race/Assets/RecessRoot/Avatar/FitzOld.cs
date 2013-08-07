 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class FitzOld : MonoBehaviour {
	
	private GameObject dummy;
	
	private BoxCollider leftWall;
	private BoxCollider rightWall;
	
	private Transform t;
	private CharacterMotor motor;
	private PlatformInputController controller;
	
	
	private tk2dAnimatedSprite anim;
	
	private string a_initWalk = "walk";
	private string a_idle = "idle";
	private string a_fall = "fall";
	private string a_jump = "jump";
	private string a_land = "land";
	private string a_dash = "dash";
	private string a_wallGrab = "wallGrab";
	
	private class InitMoveValues {
		public float groundAccel;
		public float walkSpeed;
		public float runSpeed;
		public float gravity;
		public float maxFallSpeed;
		
	}
	
	private InitMoveValues initMoveValues = new InitMoveValues();
	
	private int walkCounter = 0;
	private int airCounter = 0;
	
	private bool onGround = false;
	private bool canWallGrab = true;
	private int wallGrabSpeed = 28;
	private bool wallGrabbing = false;
	private bool WallGrabbing{
		get { return wallGrabbing; }
		set { wallGrabbing = value;
			//motor.movement.maxFallSpeed 
		}
	}
	
	private bool isDashing = false;
	private int dashTimer = 0;
	private int dashTiming = 30;
	private int dashAccel = 5000;
	private int dashSpeed = 250;
	private float doubleTapTiming = 0.2f;
	
	
	
	//inputs
	
	private bool doubleTap = false;
	private float lastAxisH = 0.0f;
	private float lastPressedH = 0.0f;
	
	private float lastY = 0f;
	
	private BoxCollider[] wallGrabTargets;
	
	// Use this for initialization
	void Start () {
		dummy = GameObject.Find("Fitzwilliam/dummy");
		t = transform;
		motor = GetComponent<CharacterMotor>();
		controller = GetComponent<PlatformInputController>();
		
		anim = dummy.GetComponent<tk2dAnimatedSprite>();
		
		leftWall = GameObject.Find(name + "/LeftWallDetector").GetComponent<BoxCollider>();
		rightWall = GameObject.Find(name + "/RightWallDetector").GetComponent<BoxCollider>();
		
		GameObject[] allWallGrabTargets = GameObject.FindGameObjectsWithTag("wallGrabTarget");		//getting all my wall grab targets!
		List<BoxCollider> boxes = new List<BoxCollider>();
		foreach (GameObject go in allWallGrabTargets){
			boxes.Add (go.GetComponent<BoxCollider>());
		}
		wallGrabTargets = boxes.ToArray();
		
				//storing my initial movement values!
		initMoveValues.gravity = motor.movement.gravity;
		initMoveValues.groundAccel = motor.movement.maxGroundAcceleration;
		initMoveValues.runSpeed = motor.movement.maxRunSpeed;
		initMoveValues.walkSpeed = motor.movement.maxWalkSpeed;
		initMoveValues.maxFallSpeed = motor.movement.maxFallSpeed;
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = motor.GetDirection();
		
		if (direction.x < 0){						//make sure the sprite is facing in the right direction
			anim.scale = new Vector3(1, 1, 1);
		}
		else if (direction.x > 0){
			anim.scale = new Vector3(-1, 1, 1);
		}
		//end
		
		//Inputs!
		doubleTap = false;
		
		float axisH = Input.GetAxis("Horizontal");
		if (axisH != 0 && lastAxisH == 0){
			if (Time.time - lastPressedH <= doubleTapTiming){
				doubleTap = true;
				Debug.Log("last time = " + lastPressedH + ", now time =" + Time.time);
				
			}
			lastPressedH = Time.time;
		}
		lastAxisH = axisH;
					//Reset my checks!
		
		if (!onGround)				//If I'm not on the ground I want to go to the 'inAir' tag
			goto inAir;
		
		
		if (direction.x != 0){						//animate the walk
			if (walkCounter == 0 && !isDashing)
				anim.Play(a_initWalk);
	
			walkCounter ++;
		}
		else{
			if (anim.CurrentClip.name != a_land && !isDashing){
				anim.Play(a_idle);
			}
			
			walkCounter = 0;
		}
		
													//<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^>
													//<===============DASHING!===============>
													//<vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv>
		if (doubleTap && !isDashing){		//init the dash
			doubleTap = false;
			isDashing = true;
			anim.Play(a_dash);
			motor.movement.maxGroundAcceleration = dashAccel;
			motor.movement.maxWalkSpeed = dashSpeed;
			motor.movement.maxRunSpeed = dashSpeed;
			controller.directionLocked = true;
		}
		
		if (isDashing){
			motor.inputMoveDirection = new Vector3(-anim.scale.x, 0, 0);
			dashTimer ++;
			if (dashTimer >= dashTiming){
				EndDash();
			}
		}
		
		return;
	inAir:
		airCounter ++;
		
		if (lastY > t.position.y && anim.CurrentClip.name != a_fall && !wallGrabbing){
			anim.Play(a_fall);
		}
		
		lastY = t.position.y;
		//check my wall detectors to see if I can wall grab
		foreach (BoxCollider bc in wallGrabTargets){
			if ((axisH > 0 && rightWall.bounds.Intersects(bc.bounds)) || (axisH < 0 && leftWall.bounds.Intersects(bc.bounds))){
				canWallGrab = true;
				break;
			}
			else{
				canWallGrab = false;
			}
		}
		if (!canWallGrab){
			motor.movement.maxFallSpeed = initMoveValues.maxFallSpeed;
			wallGrabbing = false;
		}
		return;
	}
	
	void OnControllerColliderHit (ControllerColliderHit other){
		if (other.collider.tag == "wallGrabTarget" && canWallGrab){
			motor.movement.maxFallSpeed = wallGrabSpeed;
			anim.Play(a_wallGrab);
			wallGrabbing = true;
		}
	}
	
	void OnFall () {
		if (!wallGrabbing)
			anim.Play(a_fall);
		
		onGround = false;
		Debug.Log("OnFall!");
	}
	
	void OnLand () {
		airCounter = 0;
		onGround = true;
		walkCounter = 0;
		if (motor.GetDirection().x == 0){
			anim.Play(a_land);
		}
		
		motor.movement.maxGroundAcceleration = initMoveValues.groundAccel;
		motor.movement.maxWalkSpeed = initMoveValues.walkSpeed;
		motor.movement.maxRunSpeed = initMoveValues.walkSpeed;
	}
	
	void OnJump () {
		anim.Play(a_jump);
		onGround = false;
		lastY = t.position.y;
		if (isDashing){
			EndDash();
			motor.movement.maxAirAcceleration = dashAccel;
			motor.movement.maxWalkSpeed = dashSpeed;
			motor.movement.maxRunSpeed = dashSpeed;
		}
	}
	
	void EndDash () {
		isDashing = false;
		dashTimer = 0;
		if ((Input.GetAxis("Horizontal") > 0.5f || Input.GetAxis("Horizontal") < -0.5f) && !Input.GetButton("Jump")){
			anim.Play(a_initWalk);
		}
		motor.movement.maxGroundAcceleration = initMoveValues.groundAccel;
		motor.movement.maxWalkSpeed = initMoveValues.walkSpeed;
		motor.movement.maxRunSpeed = initMoveValues.walkSpeed;
		controller.directionLocked = false;
		
	}
	
	void OnExternalVelocity () {
		
	}
}
