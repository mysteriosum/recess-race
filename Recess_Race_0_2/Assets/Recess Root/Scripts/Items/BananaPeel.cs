using UnityEngine;
using System.Collections;

public class BananaPeel : Movable {
	
	private bool inert = false;
	protected override float Deceleration {
		get {
			return base.Deceleration * 0.5f;
		}
	}
	
	protected override float Accelerate (float input)
	{
		return velocity.x;
	}
	
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		activated = true;
		velocity = new Vector2(-MaxSpeed, jumpImpulse);
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		if (!inert){
			base.FixedUpdate();
			velocity = Move(velocity, 0);
			if (grounded){
				inert = true;
			}
		}
	}
	
}
