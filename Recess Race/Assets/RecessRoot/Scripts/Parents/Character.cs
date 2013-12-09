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
	
	private Vector3 initPosition;
	private Transform currentCheckpoint;
	
	private int lastDirection;
	private int lastInput;
	private float lastDirectionTime;
	private float doubleTapLeeway = 0.2f;
	
	private bool disabled = false;
	private float disableDuration = HurtDuration.Short;
	
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
		initPosition = t.position;
		if (anim == null)
			Debug.Log ("There's no animator on this character, just fyi. Name: " + name);
		else
			anim.Play (A_fall);
	}
	
	// Update is called once per frame
	protected void Update () {
		
		
		DoInputs ();
		
		if (velocity.y < 0 && !anim.IsPlaying (A_fall) && !falling){
			anim.Play (A_fall);
			falling = true;
		}
		
		base.Update();
	}
	
	protected virtual void DoInputs() {
		//if (disabled) return;
		
		float input = HorizontalInput;
		
		if (disabled){
			input = 0;
		}
		
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
			}
		}
	}
	
	public void SetCheckpoint (Transform checkpoint){
		currentCheckpoint = checkpoint;
	}
	/// <summary>
	/// Check if I'm going to respawn (depending on what's calling the function).
	/// </summary>
	/// <param name='caller'>
	/// The script calling the function
	/// </param>
	public virtual bool Hurt (GameObject caller, float duration, Vector2 newVelocity){
		disabled = true;
		Invoke("RegainControl", duration);
		blinking = true;
		velocity = newVelocity;
		return true;
	}
	
	public virtual bool Hurt (GameObject caller){
		return Hurt(caller, disableDuration, Vector2.zero);
	}
	
	public virtual bool Hurt (GameObject caller, float duration){
		return Hurt(caller, duration, Vector2.zero);
	}
	
	public virtual void RegainControl () {
		disabled = false;
		anim.Sprite.color = Color.white;
		blinking = false;
	}
	
}

public static class HurtDuration {
	public static float Long {
		get { return 4f; }
	}
	public static float Medium {
		get { return 2.5f; }
	}
	public static float Short {
		get { return 1.5f; }
	}
}