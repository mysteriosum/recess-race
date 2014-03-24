using UnityEngine;
using System.Collections;

public class GarbagePhysics : Movable {
	
	private Vector3 targetPoint;
	private Transform ahrahObject;
	
	private float accelerationRate = 200f;
	private float maxVelocity = 50f;
	
	private float maxDistance = 5f;
	
	private Vector2 AccelVector {
		get{
			return (targetPoint - t.position).normalized * accelerationRate;
		}
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		activated = true;
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		if (activated){
			if (ahrahObject){
				targetPoint = ahrahObject.position;
			}
			velocity += AccelVector * Time.deltaTime;
			
			velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
			
			t.Translate(velocity * Time.deltaTime);
		}
		
		else{
			velocity = Move(velocity, 0);
			base.FixedUpdate();
			
			if (grounded){
				Destroy(this);
			}
		}
		
	}
	
	public void FollowTransform(Transform toFollow){
		ahrahObject = toFollow;
	}
	
	public void Deactivate(){
		activated = false;
	}
	
	
}
