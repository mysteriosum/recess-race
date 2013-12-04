using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movable : MonoBehaviour {
	
	protected Transform t;
	protected Rigidbody rb;
	protected BoxCollider bc;
	protected GameObject go;
	
	public bool hasGravity = true;
	public float defaultGravity = 2.5f;
	public float defaultMaxFallSpeed = -60f;
	
	public float acceleration = 4f;
	public float maxHorizontalSpeed = 25f;
	public float skidMultiplier = 2f;
	
	public float initialJumpVelocity = 6f;
	public float runningJumpModifier = 3f;
	
	
	
				// Overridable Getters & Setters
	protected virtual float Gravity {
		get { return defaultGravity; }
	}
	
	protected virtual float MaxFallSpeed {
		get { return defaultMaxFallSpeed; }
	}
	
	protected virtual float Acceleration {
		get { return acceleration; }
	}
	
	protected virtual float MaxSpeed {
		get { return maxHorizontalSpeed; }
	}
	
	protected virtual bool FacingRight {
		get {
			if (!anim)
				return false;
			return anim.Sprite.scale.x == 1;
		}
	}
	
	protected virtual string A_jump {
		get { return "jump"; }
	}
	protected virtual string A_fall {
		get { return "fall"; }
	}
	protected virtual string A_walk {
		get { return "walk"; }
	}
	protected virtual string A_land {
		get { return "land"; }
	}
	
	protected virtual string A_idle {
		get { return "idle"; }
	}
	
	
	protected Vector2 velocity;
	protected bool grounded;
	
	
				//vars for rays and stuff
	protected int margin = 2;
	protected int frequency = 8;
	protected int minimumRays = 2;
	protected int myLayerMask;
	
	
	public bool canMoveLeft;
	public bool canMoveRight;
	private bool canJump;
	//singleton
	
				//events & sheeeit
	protected delegate bool CollisionEvent(RaycastHit hitInfo);
	protected event CollisionEvent rightEvent;
	protected event CollisionEvent leftEvent;
	protected event CollisionEvent upEvent;
	protected event CollisionEvent downEvent;
	
	private int rayNumberH;
	private int rayNumberV;
	private Vector2 box;
	private float minInput = 0.01f;
	private bool started = false;
	
	private Vector3 initPos;
	protected tk2dSpriteAnimator anim;
	
	
	public class Raylayers{
		public readonly int onlyCollisions;
		
		public Raylayers(){
			onlyCollisions = 1 << LayerMask.NameToLayer("Collisions");
		}
	}
	
	public Raylayers layers;
	
	// Use this for initialization
	void Awake () {
		layers = new Raylayers();
	}
	
	virtual protected void Start() {
		t = transform;
		go = gameObject;
		bc = GetComponent<BoxCollider>();
		rb = rigidbody;
		anim = GetComponent<tk2dSpriteAnimator>();
		
		if (bc == null){
			Debug.LogWarning("There's no box collider on " + name);
		}
		if (rb == null){
			Debug.LogWarning("There's no rigidbody on " + name);
		}
		
		box = new Vector2(bc.size.x * t.localScale.x, bc.size.y * t.localScale.y);
		rayNumberH = Mathf.Max((int) (box.y - margin) / frequency, minimumRays);
		rayNumberV = Mathf.Max((int) (box.x - margin) / frequency, minimumRays);
		
		initPos = t.position;
		
	}
	
	// Update is called once per frame
	virtual protected void Update () {
		
		
		
		
				//get the right input amount
			
		
		if (!hasGravity)
			return;
		
		if(!grounded){
			velocity = new Vector2(velocity.x, Mathf.Max(MaxFallSpeed, velocity.y - Gravity));
		}
		#region checkDown
		//check my bottom!
		Vector2 pos = new Vector2(bc.bounds.center.x, bc.bounds.center.y);
		//Vector2 pos = new Vector2(t.position.x, t.position.y);
		//float rayLengthV = box.y / 2 + Mathf.Abs(velocity.y * Time.deltaTime);
		float rayLengthV = box.y / 2 + Mathf.Max(velocity.y * Time.deltaTime * -1, margin);
		List<Ray> downRays = new List<Ray>();
		RaycastHit downHit;
		
		for (int i = 0; i < rayNumberV; i ++){
			//offset: how far along the y axis of my collision box am I at this stage of the loop?
			int offset = i == 0? margin : i == rayNumberV - 1? (int)box.x - margin : i * frequency + margin; 
			
			//start position of my ray
			Vector3 start = new Vector3(pos.x - box.x/2 + offset, pos.y, t.position.z);
			
			downRays.Add (new Ray(start, Vector3.down));
		}
		
		bool somethingBottom = false;
		
		foreach (Ray ray in downRays){
			somethingBottom = Physics.Raycast(ray, out downHit, rayLengthV, layers.onlyCollisions);
			
			if (somethingBottom){
				if (grounded) break;
				else {
					SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
					grounded = true;
					velocity = new Vector2(velocity.x, 0);
					t.Translate(Vector3.down * (downHit.distance - box.y/2));
					break;
				}
			}
		}
		
		if (!somethingBottom){
			grounded = false;
		}
		#endregion
		
		#region checkUp
		float upRayLength = 0;
		if (grounded)
			upRayLength = box.y / 2 + margin;
		else
			upRayLength = box.y / 2 + (velocity.y > 0? velocity.y : 0) * Time.deltaTime;
		List<Ray> upRays = new List<Ray>();
		RaycastHit upHit;
		
		for (int i = 0; i < rayNumberV; i ++){
			//offset: how far along the y axis of my collision box am I at this stage of the loop?
			float offset = i == 0? margin : i == rayNumberV - 1? box.x - margin : i * frequency + margin; 
			
			//start position of my ray
			Vector3 start = new Vector3(pos.x - box.x/2 + offset, pos.y, t.position.z);
			
			upRays.Add (new Ray(start, Vector3.up));
		}
		
		bool somethingTop = false;
		
		foreach (Ray ray in upRays){
			somethingTop = Physics.Raycast(ray, out upHit, upRayLength, layers.onlyCollisions);
			
			if (somethingTop){
				if (grounded) break;
				else {
					SendMessage("HitHead", SendMessageOptions.DontRequireReceiver);
					velocity = new Vector2(velocity.x, -margin);
					t.Translate(Vector3.up * (upHit.distance - box.y/2));
					break;
				}
			}
		}
		if (somethingTop && grounded){
			canJump = false;
		}
		else if (!somethingTop && grounded){
			canJump = true;
		}
		#endregion
	}
	
	
	protected virtual void Jump(float speed){
		velocity = new Vector3(velocity.x, speed, 0);
	}
	
	virtual protected Vector2 Move (Vector2 curVel, float amount){
		
		
		//Vector2 pos = new Vector2(t.position.x, t.position.y);
		Vector2 pos = new Vector2(bc.bounds.center.x, bc.bounds.center.y);
		Vector2 vel = curVel;
		float mod = (Vector3.right * amount).normalized.x;
		
		if (amount * curVel.x < 0){
			mod *= skidMultiplier;
		}
		
		if ((amount < 0 && canMoveLeft) || (amount > 0 && canMoveRight)){
			vel = new Vector2(Mathf.Clamp(vel.x + mod * Acceleration, -MaxSpeed, MaxSpeed), vel.y);
			if (anim){
				if (!anim.IsPlaying (anim.GetClipByName(A_walk)) && grounded){
					anim.Play (A_walk);
				}
			}
		}
		else{
			vel.x -= vel.x > 0? Acceleration : vel.x < 0? -Acceleration : 0;
			if (vel.x > -Acceleration && vel.x < Acceleration){		//so many ifs... but I mean like what else can I do (probably something better but who cares)
				vel = new Vector2(0, vel.y);
				if (anim){
					if (!anim.IsPlaying (anim.GetClipByName(A_idle)) && grounded){
						anim.Play (A_idle);
					}
				}
			}
		}
		
		if (anim){							//here: flip my sprite according to the input direction
			anim.Sprite.scale = new Vector3(amount > 0? 1 : amount < 0? -1 : anim.Sprite.scale.x, anim.Sprite.scale.y, anim.Sprite.scale.z);
		}
		
		float rayLengthH = box.x / 2 + Mathf.Abs(vel.x * Time.deltaTime);
		
		List<Ray> rays = new List<Ray>();
		RaycastHit hit;
		
		for (int i = 0; i < rayNumberH; i ++){
			//offset: how far along the y axis of my collision box am I at this stage of the loop?
			int offset = i == 0? margin : i == rayNumberH - 1? (int)box.y - margin : i * frequency + margin; 
			
			//start position of my ray
			Vector3 start = new Vector3(pos.x, pos.y - box.y/2 + offset, t.position.z);
			
			rays.Add (new Ray(start, Vector3.right * vel.x)); //multiplying the current speed by Vector3.right always gives me the right direction
		}
		
		bool connected = false;
		
		foreach (Ray ray in rays){
			connected = Physics.Raycast(ray, out hit, rayLengthH, layers.onlyCollisions);
			
			if (connected){		//what do I do when I hit a wall? You decide! ...not really.
				Vector2 compVec = new Vector2(vel.normalized.x, 0).normalized;
				vel = new Vector2(compVec.x * (hit.distance - box.x/2), vel.y);
				SendMessage("HitWall", SendMessageOptions.DontRequireReceiver);
				if (vel.x > 0){
					canMoveRight = false;
					SendMessage("HitRight", SendMessageOptions.DontRequireReceiver);
				}
				if (vel.x < 0){
					canMoveLeft = false;
					SendMessage("HitLeft", SendMessageOptions.DontRequireReceiver);
				}
				break;
			}
		}
		
		if (!connected){
			canMoveLeft = true;
			canMoveRight = true;
		}
		return vel;
	}
	
	
	
	virtual protected Vector2 Move (){
		return Move(this.velocity, Input.GetAxis("Horizontal"));
	}
	
	public void Restart () {
		this.t.position = initPos;
		velocity = Vector2.zero;
	}
	
	virtual protected void LateUpdate (){
		Vector3 before = t.position;
		t.Translate(velocity * Time.deltaTime);
		
		//rb.velocity = (Vector3)velocity;
	}
	
	
}
