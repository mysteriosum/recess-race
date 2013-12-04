using UnityEngine;
using System.Collections;

public class Character : Movable {
	
	protected float minimumAxisInput = 0.15f;
	
	private float maxPress2Jump = 0.25f;
	private float jumpTimer = 0;
	
	protected virtual float HorizontalInput {
		get {
			float input = (Input.GetKey(KeyCode.LeftArrow)? -1 : 0) + (Input.GetKey(KeyCode.RightArrow)? 1 : 0);
			float hAxis = Input.GetAxis("Horizontal");
			if (input == 0 && hAxis != 0){
				bool right = hAxis > minimumAxisInput;
				 
				input = right? 1 : -1;
			}
			return input;
		}
	}
	
	protected virtual bool JumpInput {
		get{
			return jumpTimer < maxPress2Jump;
		}
	}
	
	private int lastDirection;
	private int lastInput;
	private float lastDirectionTime;
	private float doubleTapLeeway = 0.2f;
	
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
	protected void Start () {
		base.Start();
		
		if (anim == null)
			Debug.Log ("There's no animator on this character, just fyi. Name: " + name);
		else
			anim.Play (A_fall);
	}
	
	// Update is called once per frame
	protected void Update () {
		DoInputs ();
		
		if (velocity.y < 0 && !anim.IsPlaying (A_fall)){
			anim.Play (A_fall);
		}
		
		base.Update();
	}
	
	protected virtual void DoInputs() {
		float input = HorizontalInput;
		
		velocity = Move(velocity, input);
		
		int nowInput = (int) input;
		
		if (lastInput == 0 && nowInput != 0){
			
			if (nowInput == lastDirection && Time.time - lastDirectionTime < doubleTapLeeway){
				Debug.Log ("Double tap!");
			}
			
			lastDirection = nowInput;
			lastDirectionTime = Time.time;
		}
		lastInput = nowInput;
		
		if (Input.GetButton("Jump")){		//calculate a jump press timer (so that if they press jump right before landing they still jump)
			if (jumpTimer == float.MaxValue)
				jumpTimer = 0;
			jumpTimer += Time.deltaTime;
		}
		else{
			jumpTimer = float.MaxValue;
		}
		
		if (grounded){						//whether I should jump
			if (JumpInput){
				//do the jump! Add the last part there to give myself a running jump
				this.Jump(initialJumpVelocity + Mathf.Abs(velocity.x/runningJumpModifier));
				anim.Play (A_jump);
			}
		}
		
	}
	
}
