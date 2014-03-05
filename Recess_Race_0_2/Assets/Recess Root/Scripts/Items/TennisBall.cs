using UnityEngine;
using System.Collections;

public enum DirectionsEnum{
	up, left, down, right,
}

public class TennisBall : Movable {
	
	protected override float Gravity {
		get {
			return hasGravity? defaultGravity * gravityScale : 0;
		}
	}
	
	protected override float Deceleration {
		get {
			return deceleration;
		}
	}
	
	private bool hasGravity = false;
	
	//public inspector fields
	public float speed = 3f;
	public DirectionsEnum direction = DirectionsEnum.up;
	
	private CircleCollider2D circle;
	
	private Vector2 collideVelocity = new Vector2(80, 150)/TileProperties.tileDimension;
	private float bounceRetentionPercentage = 0.4f;
	private float MinimumBounce{
		get { return bounceRetentionPercentage * 2; }
	}
	private float deceleration = 0.0125f;
	private float gravityScale = 0.75f;
	
	protected override void Start () {
		base.Start();
		
		switch (direction){
		case DirectionsEnum.up:
			velocity = Vector2.up * speed;
			break;
		case DirectionsEnum.down:
			velocity = -Vector2.up * speed;
			break;
		case DirectionsEnum.left:
			velocity = -Vector2.right * speed;
			break;
		case DirectionsEnum.right:
			velocity = Vector2.right * speed;
			break;
			
		}
		
		circle = GetComponent<CircleCollider2D>();
		if (!circle){
			Debug.LogWarning("There's no circle collider on this tennis ball, wtf!");
		}
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		if (!hasGravity){ 		//move if I'm still being a hazard
			
			float rayLength = circle.radius + velocity.magnitude * Time.deltaTime;
			
			Vector2 rayDirection = velocity.normalized;
			float angle = Vector2.Angle(Vector2.right, rayDirection);
			Vector2[] origins = new Vector2[3] {
				t.position, 
				(Vector2) t.position + new Vector2(Mathf.Cos(Mathf.Deg2Rad * (angle - 90)), Mathf.Sin(Mathf.Deg2Rad * (angle - 90))) * circle.radius,
				(Vector2) t.position + new Vector2(Mathf.Cos(Mathf.Deg2Rad * (angle + 90)), Mathf.Sin(Mathf.Deg2Rad * (angle + 90))) * circle.radius,
			};
			
			RaycastHit2D hitInfo;
			
			for (int i = 0; i < 2; i++) {
				
				hitInfo = Physics2D.Raycast(origins[i], rayDirection, rayLength, Raylayers.onlyCollisions);
				
				if (hitInfo.fraction > 0){
					extraMove = VectorFunctions.Bounce(velocity, hitInfo.normal);
					t.Translate(velocity * hitInfo.fraction * Time.deltaTime);
					velocity = Vector2.zero;
					break;
				}
			}
		}
		else{
			float lastYVelocity = velocity.y;
			float lastXVelocity = velocity.x;
			base.FixedUpdate();
			
			if (velocity.x != 0){
				velocity = Move(velocity, 0);
				if (velocity.x == 0 && Mathf.Abs(lastXVelocity) > deceleration){
					velocity = new Vector2(-lastXVelocity * bounceRetentionPercentage, velocity.y);
				}
			}
			
			
			if (grounded && lastYVelocity < 0 && Mathf.Abs(lastYVelocity) > bounceRetentionPercentage){
				float amount = Mathf.Abs(lastYVelocity) * bounceRetentionPercentage;
				if (amount > MinimumBounce){
					velocity = Jump(velocity, Mathf.Abs(lastYVelocity) * bounceRetentionPercentage);
				}
			}
		}
		
		t.Translate(velocity * Time.deltaTime, Space.World);
		if (extraMove != Vector2.zero){
			velocity += extraMove;
			extraMove = Vector2.zero;
		}
	}
	protected override float Accelerate (float input)
	{
		
		return velocity.x;
	}
	 
	protected override void UpdatePosAndBox ()
	{
		pos = (Vector2) t.position;
		box = new Rect(t.position.x - circle.radius, t.position.y - circle.radius, circle.radius * 2, circle.radius * 2);
	}
	
	void CollideWithFitz(){
		Transform fitz = Fitz.fitz.transform;
		hasGravity = true;
		velocity = new Vector2(collideVelocity.x * (fitz.position.x > t.position.x? -1 : 1), collideVelocity.y);
		DamageScript dmg = GetComponent<DamageScript>();
		dmg.enabled = false;
	}
}
