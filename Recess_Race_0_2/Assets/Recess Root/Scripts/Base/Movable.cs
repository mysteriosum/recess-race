using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {

	protected Controller controller = new Controller();

	//------------------------------------------------------\\
	//--------------Movement variables and such-------------\\
	//------------------------------------------------------\\
	//evething is in pixels/second

	protected float defaultGravity						= 9.8f;
	protected float holdGravityModifier					= 0.7f;
	protected float maxFallSpeed						= -220f;
	
	protected float lerpAccel							= 0.0375f;
	protected float lerpTargetAdd						= 4f;
	protected float baseDecel							= 11f;
	
	protected float maxSpeed							= 150f;
	protected float jumpImpulse							= 220f;
	protected float extraImpulseFromRun					= 36f;
	
	protected float headHitVelocityMod					= 0.33f;
	
	//------------------------------------------------------\\
	//--------------------Rays and such---------------------\\
	//------------------------------------------------------\\

	protected int horizontalRays						= 5;
	protected int verticalRays							= 4;
	protected float margin								= 3.5f;
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


	//------------------------------------------------------\\
	//------------------other properties--------------------\\
	//------------------------------------------------------\\
	
	protected Vector2 velocity;
	protected Vector2 pos;
	protected Vector2 extraMove							= Vector2.zero;
	
	protected Rect box;

	protected bool grounded								= false;
	protected bool falling								= false;

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

	//------------------------------------------------------\\
	//----------------------Debugging-----------------------\\
	//------------------------------------------------------\\

	bool running = false;

	protected virtual void Start () {
		t = transform;
		rb = rigidbody2D;
		r = GetComponent<SpriteRenderer>();
		bc = GetComponent<BoxCollider2D>();
		sprite = r.sprite;
		
		running = true;			//DEV
		
	}
	
	protected virtual void FixedUpdate () {
		if (!activated){	//so I don't fall a million metres in the first frame because it took so long to load v_v;
			activated = true;
			return;
		}
		pos = (Vector2) t.position;
		box = new Rect(t.position.x - bc.size.x/2, t.position.y - bc.size.y/2, bc.size.x, bc.size.y);

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
		}

		if (downRayLength > 0){	//only check down if I'm grounded, or if I'm falling


			bool connectedDown = false;
			int lastConnection = 0;
			Vector2 min = new Vector2(box.xMin + margin, box.center.y);
			Vector2 max = new Vector2(box.xMax - margin, box.center.y);

			for (int i = 0; i < verticalRays; i ++){
				Vector2 start = Vector2.Lerp(min, max, (float)i / (float) verticalRays);
				Vector2 end = start + -Vector2.up * (downRayLength + box.height/2);
				downRays[i] = Physics2D.Linecast(start, end, Raylayers.downRay);
				if (downRays[i].fraction > 0){
					connectedDown = true;
					lastConnection = i;
				}
			}
			
			if (connectedDown && !grounded){
				//t.Translate(-Vector2.up * downRays[lastConnection].point * downRays[lastConnection].fraction);
				//t.position = Vector2.Lerp (downRays[lastConnection].point, pos, downRays[lastConnection].fraction);
				//t.position = new Vector2(t.position.x, downRays[lastConnection].point.y + box.height/2);
				velocity = new Vector2(velocity.x, 0);
				extraMove += new Vector2(0, downRays[lastConnection].point.y - (t.position.y - box.height/2));
				grounded = true;
				falling = false;
				SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
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
				falling = true;
				extraMove += new Vector2(0, upRays[lastConnection].point.y - (t.position.y + box.height/2));
				SendMessage("OnHeadHit", SendMessageOptions.DontRequireReceiver);
			}
		}
		
	}

	//------------------------------------------------------\\
	//------------Virtual displacement functions------------\\
	//------------------------------------------------------\\
	

	protected virtual Vector2 Move(Vector2 currentVelocity, float input){
		Vector2 vel = currentVelocity;
		float newX = Accelerate(input);
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
			float checkAmount = box.width/2 * modifier + newX;
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
			}
		}
		//Debug.Log("newX = " + newX);
		return new Vector2(newX, currentVelocity.y);
	}
	
	protected virtual float Accelerate(float input){
		float newX = Mathf.Lerp(velocity.x, (MaxSpeed + lerpTargetAdd) * input, LerpAccel);
		newX = Mathf.Clamp(newX, -MaxSpeed, MaxSpeed);
		return newX;
	}
	
	protected virtual Vector2 Jump (Vector2 currentVelocity, float amount){
		Vector2 newVel = new Vector2(currentVelocity.x, amount);
		return newVel;
	}
	

	protected void LateUpdate() {
		t.Translate(velocity * Time.deltaTime + extraMove);
		extraMove = Vector2.zero;
	}
	
	
	void OnDrawGizmos(){
		if (running){
			Gizmos.DrawLine(box.center, box.center + new Vector2(0, box.height / -2 + velocity.y * Time.deltaTime));
		}
	}
}








