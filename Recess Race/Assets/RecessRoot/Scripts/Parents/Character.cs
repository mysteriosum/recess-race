using UnityEngine;
using System.Collections;

public class Character : Movable {
	
	
	//LOTS OF DELEGATES! YEAAAAAAAAAAAAAAAAAAAAAAAH
	public delegate void InputDelegate();
	public delegate void FloatyDelegate (float para);
	
	public InputDelegate run;
	public InputDelegate runUp;
	public InputDelegate runDown;
	
	public InputDelegate jump;
	public InputDelegate jumpUp;
	public InputDelegate jumpDown;
	
	public InputDelegate doubleTap;
	public InputDelegate aboutFace;
	
	public InputDelegate downDown;
	public InputDelegate downUp;
	
	public InputDelegate directionDown;
	public FloatyDelegate direction;
	public InputDelegate directionUp;
	
	public InputDelegate fall;
	public InputDelegate gravity;
	
	
	
	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		float input = (Input.GetKey(KeyCode.LeftArrow)? -1 : 0) + (Input.GetKey(KeyCode.RightArrow)? 1 : 0);
		
		velocity = Move(velocity, input);
		
		if (grounded){
			if (Input.GetKey(KeyCode.Z)){
				//do the jump! Add the last part there to give myself a running jump
				this.Jump(initialJumpVelocity + Mathf.Abs(velocity.x/runningJumpModifier));
			}
		}
		base.Update();
	}
	
	
}
