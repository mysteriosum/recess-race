using UnityEngine;
using System.Collections;

public enum DirectionsEnum{
	up, left, down, right,
}

public class TennisBall : Movable {
	
	protected override float Gravity {
		get {
			return hasGravity? base.Gravity : 0;
		}
	}
	
	private bool hasGravity = false;
	
	//public inspector fields
	public float speed;
	public DirectionsEnum direction = DirectionsEnum.up;
	
	private CircleCollider2D circle;
	
	
	void Start () {
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
	void FixedUpdate () {
		
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
			base.FixedUpdate();
			
		}
		
		t.Translate(velocity * Time.deltaTime, Space.World);
		if (extraMove != Vector2.zero){
			velocity += extraMove;
			extraMove = Vector2.zero;
		}
	}
	 
	protected override void UpdatePosAndBox ()
	{
		pos = (Vector2) t.position;
		box = new Rect(t.position.x - circle.radius, t.position.y - circle.radius, circle.radius * 2, circle.radius * 2);
	}
}
