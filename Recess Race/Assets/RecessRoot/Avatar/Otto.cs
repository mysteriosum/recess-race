using UnityEngine;
using System.Collections;

public class Otto : Character {
	
	public float runMaxSpeedModifier;
	public float sprintMaxspeedModifier;
	public float jumpHoldGravityMod;
	public float airAccelMod = 2.0f;
	
	protected override float Acceleration {
		get {
			return base.Acceleration * (grounded? 1 : airAccelMod);
		}
	}
	
	protected override float MaxSpeed {
		get {
			return maxHorizontalSpeed * (sprinting? sprintMaxspeedModifier : running? runMaxSpeedModifier : 1);
		}
	}
	
	protected override float Gravity {
		get {
			return Input.GetButton("Jump")? defaultGravity * jumpHoldGravityMod : defaultGravity;
		}
	}
	
	protected override string A_jump {
		get {
			return sprinting? "sprintJump" : "jump";
		}
	}
	
	protected override string A_walk {
		get {
			return sprinting? "dash" : "walk";
		}
	}
	
	protected override string A_fall {
		get {
			return sprinting? "sprintJump" : "fall";
		}
	}
	
	private bool running;
	private bool sprinting;
	
	private float pBar = 0;
	private float pBarRunAt = 0.65f;
	
	private float minFPS = 10;
	private float maxAnimSpeedMod = 2.0f;
	private float defaultFPS;
	
	void Awake () {
		base.Awake();
		this.name = "Fitzwilliam";
	}
	
	// Use this for initialization
	void Start () {
		base.Start ();
		
		defaultFPS = anim.CurrentClip.fps;
	}
	
	// Update is called once per frame
	void Update () {
		running = Input.GetButton ("Run");
		base.Update ();
		
		
		if (Mathf.Abs (velocity.x) >= maxHorizontalSpeed * runMaxSpeedModifier - Acceleration && running){
			if (grounded)
				pBar = Mathf.Min(pBar + Time.deltaTime, pBarRunAt);
			sprinting = pBar >= pBarRunAt;
		}
		else if (!running || (grounded &&velocity.x < MaxSpeed)){
			sprinting = false;
			pBar = Mathf.Max (pBar - Time.deltaTime, 0);
		}
		
	}
	
	void OnFall () {
		 anim.Play (A_walk);
	}
	
	protected override Vector2 Move (Vector2 curVel, float amount)
	{
		velocity = base.Move (curVel, amount);
		
		if (anim.IsPlaying (A_walk) && !sprinting){
			float percentage = Mathf.Abs (velocity.x) / (maxHorizontalSpeed * runMaxSpeedModifier);
			anim.CurrentClip.fps = Mathf.Lerp(minFPS, minFPS * maxAnimSpeedMod, percentage);
			
		}
		
		return velocity;
	}
	
	
}
