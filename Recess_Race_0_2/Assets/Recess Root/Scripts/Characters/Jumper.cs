using UnityEngine;
using System.Collections;

public class Jumper : Movable {
	private Fitz fitz;
	
	private float detectRange;
	
	protected override float JumpImpulse {
		get {
			return base.JumpImpulse;
		}
	}
	
	protected override float Gravity {
		get {
			return defaultGravity * holdGravityModifier;
		}
	}
	// Use this for initialization
	protected override void Start () {
		base.Start();
		fitz = Fitz.fitz;
		
		detectRange = Camera.main.orthographicSize;
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		base.FixedUpdate();
		
		if (grounded){
			bool fitzGrounded = fitz.IsGrounded;
			float distance = Mathf.Abs(fitz.transform.position.x - t.position.x);
			if (!fitzGrounded && distance < detectRange){
				velocity = Jump(velocity, JumpImpulse);
			}
			
			if (fitz.transform.position.x > t.position.x){
				t.localScale = new Vector3(1, 1, 1);
			}else{
				t.localScale = new Vector3(-1, 1, 1);
			}
		}
		
	}
}
