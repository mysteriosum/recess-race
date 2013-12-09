using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TennisBall : Movable {
	
	private bool bouncing;
	private float floorBounceModifier = 0.8f;
	private Vector2 pushVector = new Vector2(140, 0);
	public int MoveDirection {
		get { return bouncing? 0 : 1; }
	}
	
	protected override float Acceleration {
		get {
			return bouncing? acceleration : maxHorizontalSpeed;
		}
	}
	
	private bool harmless = false;
	
	public bool Harmless {
		get { return harmless; }
		set {
			harmless = value;
			sprite.scale = harmless? Vector3.one * 0.5f : Vector3.one;
		}
	}
	
	void Start () {
		base.Start();
		hasGravity = false;
		
		velocity = new Vector2(maxHorizontalSpeed, 0);
		
	}
	
	void Update () {
		velocity = Move(velocity, MoveDirection);	//always moving right (locally)
		if (bouncing){
			float ySpeed = velocity.y;
			base.Update();
			if (grounded){
				Jump(Mathf.Abs(ySpeed * floorBounceModifier));
			}
		}
	}
	
	void HitWall (RaycastHit hitInfo){
		
		if (bouncing){
			velocity = new Vector2(-velocity.x, velocity.y);
			return;
		}
		
		Vector2 direction = new Vector2(t.right.x, t.right.y);
		Vector2 normal = (Vector2) hitInfo.normal;
		
		Vector2 newDirection = VectorFunctions.Bounce(direction, normal);		//my vector function provides a bounce given an angle and a normal
		
		Vector3 forward = new Vector3(newDirection.x, newDirection.y, 0);
		
		t.rotation = VectorFunctions.Look2D(newDirection);		//my new vector function uses Quaternion.LookRotation to make an appropriate look rotation for 2d
	}
	
	void OnCollisionEnter (Collision collision){
		Character charScript = collision.gameObject.GetComponent<Character>();
		
		if (charScript && !bouncing){
			Vector2 push = pushVector * (t.position.x > collision.transform.position.x? -1 : 1);
			bool fitz = charScript.Hurt(this.gameObject, HurtDuration.Medium, push);
			//start bouncing and no longer be dangerous
			if (!fitz) return;	//but only if I hit Fitz and not a bully
			bouncing = true;
			hasGravity = true;
			Harmless = true;
			t.rotation = Quaternion.identity;
			Jump(initialJumpVelocity);
		}
	}
	
	protected override Vector2 Move (Vector2 curVel, float amount)
	{
		float xSpeed = velocity.x;
		Vector2 result = base.Move(curVel, amount);
		if (bouncing && (!canMoveLeft || !canMoveRight)){
			Debug.Log("xSpeed is " + xSpeed);
			result = new Vector2(-xSpeed, result.y);
			canMoveLeft = true;
			canMoveRight = true;
			Debug.Log(result);
		}
		return result;
	}
	
	
}
