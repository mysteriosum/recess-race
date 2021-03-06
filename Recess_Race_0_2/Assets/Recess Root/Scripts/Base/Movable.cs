﻿using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {

	protected Controller controller;

	//------------------------------------------------------\\
	//--------------Movement variables and such-------------\\
	//------------------------------------------------------\\
	//evething is in pixels/second


	protected float defaultGravity						= 7.8f/TileProperties.tileDimension;
	protected float holdGravityModifier					= 0.74f;
	protected float maxFallSpeed						= -250f/TileProperties.tileDimension;
	
	protected float lerpAccel							= 0.0375f;
	protected float lerpTargetAdd						= 4f/TileProperties.tileDimension;
	protected float baseDecel							= 6f/TileProperties.tileDimension;
	protected float secondsToMax						= 0.6f;
	
	protected float maxSpeed							= 140f/TileProperties.tileDimension;
	protected float jumpImpulse							= 196f/TileProperties.tileDimension;
	protected float extraImpulseFromRun					= 10f/TileProperties.tileDimension;
	
	protected float headHitVelocityMod					= 0.33f;
	
	protected float maxWalkAnimMultiplier				= 1.5f;
	
	//angles and slopes
	float angleLeeway = 5f;
	
	//------------------------------------------------------\\
	//--------------------Rays and such---------------------\\
	//------------------------------------------------------\\

	protected int horizontalRays						= 5;
	protected int verticalRays							= 4;
	protected float margin								= 3.5f/TileProperties.tileDimension;
	#region declaringRays
	protected RaycastHit2D[] downRays = new RaycastHit2D[] {
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
	};

	protected RaycastHit2D[] upRays = new RaycastHit2D[] {
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
	};
	protected RaycastHit2D[] sideRays = new RaycastHit2D[] {
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
		new RaycastHit2D(),
	};
	#endregion
	//------------------------------------------------------\\
	//----------------------Components----------------------\\
	//------------------------------------------------------\\
	
	protected Transform t;
	protected Rigidbody2D rb;
	protected BoxCollider2D bc;
	protected SpriteRenderer r;
	protected Sprite sprite;
	protected Animator anim;
	protected bool animated = true;
	protected AudioSource source;
	protected Sounds sounds;

	//------------------------------------------------------\\
	//------------------other properties--------------------\\
	//------------------------------------------------------\\
	
	protected Vector2 velocity;
	protected Vector2 pos;
	protected Vector2 extraMove							= Vector2.zero;
	
	protected Rect box;

	protected bool grounded								= false;
	protected bool falling								= false;
	protected bool hurt									= false;

	protected bool activated							= false;

	//------------------------------------------------------\\
	//-----------------Getters and setters------------------\\
	//------------------------------------------------------\\
	
	protected virtual float Gravity {
		get { return defaultGravity * (controller.getJump? holdGravityModifier : 1); }
	}
	protected virtual float MaxFallSpeed {
		get { return maxFallSpeed; }
	}
	protected virtual float MaxSpeed {
		get { return maxSpeed; }
	}
	protected virtual float Deceleration {
		get { return baseDecel; }
	}
	protected virtual float LerpAccel {
		get { return lerpAccel; }
	}
	protected virtual float JumpImpulse {
		get {
			float extra = extraImpulseFromRun * (Mathf.Abs(velocity.x) / MaxSpeed);
			return jumpImpulse + extra; 
		}
	}
	protected virtual float HeadHitMod {	//when I hit my head on something I get a little bounce (-velocity.y * this value)
		get {
			return headHitVelocityMod;
		}
	}
	protected virtual float SecondsToMax {
		get { return secondsToMax; }
	}
	
	
	//------------------------------------------------------\\
	//-------------------Animation names--------------------\\
	//------------------------------------------------------\\
	public class AnimationNames{
		public string walk = "Walk";
		public string idle = "Idle";
		public string jump = "Jump";
		public string land = "Land";
		public string fall = "Fall";
		public string hurt = "Hurt";
		public string rest = "Rest";
		public string roll = "Roll";
		public string down = "Down";
		public string hang = "Hang";
		public string skid = "Skid";
	}
	
	public AnimationNames a = new AnimationNames();
	
	protected virtual string WalkAnimation {
		get{
			return a.walk;
		}
	}
	protected virtual string JumpAnimation {
		get{
			return a.jump;
		}
	}
	protected virtual string FallAnimation {
		get{
			return a.fall;
		}
	}
	protected virtual string LandAnimation {
		get{
			return a.land;
		}
	}
	//------------------------------------------------------\\
	//----------------------Debugging-----------------------\\
	//------------------------------------------------------\\

	bool running = false;
	public bool debug = false;

	protected virtual void Start () {
		t = transform;
		rb = rigidbody2D;
		r = GetComponent<SpriteRenderer>();
		bc = GetComponent<BoxCollider2D>();
		sprite = r.sprite;
		anim = GetComponent<Animator>();
		source = gameObject.AddComponent<AudioSource>();
		sounds = new Sounds();
		controller = new Controller();
		if (!anim){
			if (debug){
				Debug.LogWarning("No animations on this Movable. Disabling animations!");
			}
			animated = false;
		}
		
		running = true;			//DEV
		
	}
	
	protected virtual void FixedUpdate () {
	
		
		if (!activated){	//so I don't fall a million metres in the first frame because it took so long to load v_v;
			return;
		}
		UpdatePosAndBox();

	//------------------------------------------------------\\
	//------------------------Gravity-----------------------\\
	//------------------------------------------------------\\
		float downRayLength = margin;

		if (!grounded){
			velocity = new Vector2(velocity.x, Mathf.Max(velocity.y - Gravity, MaxFallSpeed));
			downRayLength = -velocity.y * Time.deltaTime;
			
			if (velocity.y < 0 && !falling){
				falling = true;
				
				SendMessage("OnFall", SendMessageOptions.DontRequireReceiver);
			}
			if (falling && animated && !hurt){
				anim.Play(FallAnimation);
			}
		}

		if (downRayLength > 0){	//only check down if I'm grounded, or if I'm falling


			bool connectedDown = false;
			int lastConnection = 0;
			Vector2 min = new Vector2(box.xMin, box.center.y);
			Vector2 max = new Vector2(box.xMax, box.center.y);

			for (int i = 0; i < verticalRays; i ++){
				Vector2 start = Vector2.Lerp(min, max, (float)i / (float) (verticalRays-1));
				Vector2 end = start + -Vector2.up * (downRayLength + box.height/2);
				downRays[i] = Physics2D.Linecast(start, end, Raylayers.downRay);
				if (downRays[i].fraction > 0){
					float angle = Mathf.Abs(Vector2.Angle(Vector2.right, downRays[i].normal));
					if (Mathf.Abs(angle - 180) < angleLeeway || angle < angleLeeway){
						Debug.Log("Yeah dude");
						continue;
					}
					connectedDown = true;
					lastConnection = i;
				}
			}
			
			if (connectedDown && !grounded){
				//t.Translate(-Vector2.up * downRays[lastConnection].point * downRays[lastConnection].fraction);
				//t.position = Vector2.Lerp (downRays[lastConnection].point, pos, downRays[lastConnection].fraction);
				//t.position = new Vector2(t.position.x, downRays[lastConnection].point.y + box.height/2);
				velocity = new Vector2(velocity.x, 0);
				extraMove += new Vector2(0, downRays[lastConnection].point.y - box.yMin);
//				extraMove += new Vector2(0, (downRayLength + box.height/2) * -(1 - downRays[lastConnection].fraction));
				//t.position = new Vector3(t.position.x + bc.center.x, downRays[lastConnection].point.y + bc.center.y - bc.size.y/2, t.position.z);
				grounded = true;
				falling = false;
				SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
				if (debug){
					//Debug.Log ("I'm grounded now. This is what I hit: " + downRays[lastConnection].collider.name);
				}
				if (animated){
					anim.Play(LandAnimation);
				}
			}
			else if (!connectedDown){
				grounded = false;
			}
			
		}
		
	//------------------------------------------------------\\
	//---------------------Head banging---------------------\\
	//------------------------------------------------------\\
		
		if (grounded || velocity.y > 0){
			float upRayLength = grounded? margin : velocity.y * Time.deltaTime;
			
			bool connection = false;
			int lastConnection = 0;
			Vector2 min = new Vector2(box.xMin + margin, box.center.y);
			Vector2 max = new Vector2(box.xMax - margin, box.center.y);
			
			for (int i = 0; i < verticalRays; i ++){
				Vector2 start = Vector2.Lerp(min, max, (float)i / (float) verticalRays);
				Vector2 end = start + Vector2.up * (upRayLength + box.height/2);
				upRays[i] = Physics2D.Linecast(start, end, Raylayers.upRay);
				if (upRays[i].fraction > 0){
					connection = true;
					lastConnection = i;
				}
			}
			
			if (connection){
				velocity = new Vector2(velocity.x, -velocity.y * HeadHitMod);
				extraMove += new Vector2(0, upRays[lastConnection].point.y - box.yMax);
				SendMessage("OnHeadHit", SendMessageOptions.DontRequireReceiver);
			}
		}
		
		
		t.Translate(velocity * Time.deltaTime + extraMove);
		extraMove = Vector2.zero;
	}
	
	protected virtual void UpdatePosAndBox (){
		pos = (Vector2) t.position;
		box = new Rect(t.position.x + bc.center.x - bc.size.x/2, t.position.y + bc.center.y	 - bc.size.y/2, bc.size.x, bc.size.y);
	}

	//------------------------------------------------------\\
	//------------Virtual displacement functions------------\\
	//------------------------------------------------------\\
	

	protected virtual Vector2 Move(Vector2 currentVelocity, float input){
		Vector2 vel = currentVelocity;
		float newX = vel.x;
		
		if (input != 0){
			newX = Accelerate(input);
			t.localScale = new Vector3(input > 0? 1 : -1, 1, 1);
			if (animated && grounded){
				anim.Play(WalkAnimation);
				anim.speed = Mathf.Lerp(MaxSpeed * maxWalkAnimMultiplier, MaxSpeed, Mathf.Abs(newX) / MaxSpeed)/ MaxSpeed;
			}
		} else if (animated && grounded && !hurt){
			anim.Play(a.idle);
		}
		
		//additional deceleration if the input doesn't match current speed
		if ((vel.x > 0 && input <= 0) || (vel.x < 0 && input >= 0)){
			
			int modifier = vel.x > 0? 1 : -1;
			newX -= Deceleration * modifier;
			if (Mathf.Abs(newX) < Deceleration){
				newX = 0;
			}
			
		}
		
		if (newX != 0){
			//check for collisions:
			int modifier = newX > 0? 1 : -1;
			//float checkAmount = box.width/2 * modifier + newX; //neverUSed
			Vector2 Min = new Vector2(box.center.x, box.yMin + margin);
			Vector2 Max = new Vector2(box.center.x, box.yMax - margin);
			bool connected = false;
			int lastConnection = 0;
			
			for (int i = 0; i < horizontalRays; i ++){
				Vector2 start = Vector2.Lerp(Min, Max, (float) i / (float) horizontalRays);
				Vector2 end = start + Vector2.right * modifier * (Mathf.Abs(newX * Time.deltaTime) + box.width/2);
				sideRays[i] = Physics2D.Linecast(start, end, Raylayers.onlyCollisions);
				
				if (sideRays[i].fraction > 0){
					connected = true;
					lastConnection = i;
				}
			}
			
			if (connected){
				//t.position = new Vector2(sideRays[lastConnection].point.x - (box.width/2 * modifier), t.position.y);
				extraMove += new Vector2(sideRays[lastConnection].point.x - (t.position.x + box.width/2 * modifier), 0);
				newX = 0;
				//Debug.Log(name + " connected!");
			}
			
		}
		//Debug.Log("newX = " + newX);
		return new Vector2(newX, currentVelocity.y);
	}
	
	public void setActivated(){
		activated = true;
	}
	
	protected virtual float Accelerate(float input){
		float newX = velocity.x + input * (MaxSpeed / SecondsToMax) * Time.deltaTime;
		newX = Mathf.Clamp(newX, -MaxSpeed, MaxSpeed);
		return newX;
	}
	
	protected virtual Vector2 Jump (Vector2 currentVelocity, float amount){
		if (animated){
			anim.Play(JumpAnimation);
		}
		Vector2 newVel = new Vector2(currentVelocity.x, amount);
		
		Invoke("PlayJumpSound", 0.03f);
		return newVel;
	}
	
	void PlayJumpSound(){
		source.clip = sounds.jump;
		source.Play();
	}
	
	
	public void SetVelocity (Vector2 newVelocity){
		velocity = newVelocity;
	}
	
	void OnDrawGizmos(){
		if (running && debug){
			Gizmos.DrawLine(box.center, box.center + new Vector2(0, box.height / -2 + velocity.y * Time.deltaTime));
			Gizmos.DrawCube (box.center, new Vector3(box.width, box.height, 1));
		}
	}
	
	public void GarbagePickup(){
		source.clip = sounds.cameraSound;
		source.Play();
	}
}
