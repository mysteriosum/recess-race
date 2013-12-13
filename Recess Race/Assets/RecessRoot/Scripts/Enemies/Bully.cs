using UnityEngine;
using System.Collections;

public class Bully : Character {
	
	private float input = 0;
	//private HeightEnum jumpHeight = HeightEnum.none;
	
	public float mediumJumpMod = 1.5f;
	public float highJumpMod = 2.0f;
	
	GameObject justJumpedFrom;
	
	protected int skillPenalty = 0;
	public int SkillPenalty {
		get { return skillPenalty; }
		set { skillPenalty = value; }
	}
	
	private bool beingWatched = false;
	
	public bool BeingWatched {
		get { return beingWatched; }
	}
	
	public delegate void BullyDelegate(Character fitz);
	public BullyDelegate bullyMethod;
	
	protected void Start(){
		base.Start ();
		
		//TEST to see if a lower skill modifier is significant
		/*defaultGravity = 8.55f;
		defaultMaxFallSpeed = -200;
		acceleration = 10;
		maxHorizontalSpeed = 130;
		skidMultiplier = 2;
		initialJumpVelocity = 194.2;
		runningJumpModifier = 3;
		mediumJumpMod = 1.5f;
		highJumpMod = 2f;*/
	}
	
	protected void Update() {
		base.Update ();
	}
	
	protected override void DoInputs ()
	{
		velocity = Move (velocity, input);
	}
	
	void OnCollisionEnter (Collision other){
		
		if (other.gameObject.GetComponent<Fitz>()){
			Character c = other.gameObject.GetComponent<Character>();
			Debug.Log("Don't touch me");
			if (bullyMethod != null)
				bullyMethod(c);
			else
				Debug.LogWarning ("Your bully has no method...");
		}
	}
	
	protected virtual void OnTriggerStay (Collider other){
		BullyInstruction instruction = other.gameObject.GetComponent<BullyInstruction>();
		
		if (instruction){
			float tempInput = instruction.GetDirection();
			HeightEnum jumpHeight = instruction.GetImpulse ();
			
			if (tempInput != input && jumpHeight != HeightEnum.none){
				return;	//I don't want to jump if I'm not going in the right direction already
			}
			else{
				input = tempInput;
			}
			
			if (grounded && jumpHeight != HeightEnum.none && other.gameObject != justJumpedFrom){
				Jump (ImpulseFromHeightEnum(jumpHeight, instruction.jumpDifficulty));
				justJumpedFrom = instruction.gameObject;
			}
		}
	}
	
	protected virtual void OnTriggerExit (Collider other){
		if (other.gameObject == justJumpedFrom){
			justJumpedFrom = null;
		}
		
		if (other.GetComponent<ObservedZone>() != null){
			beingWatched = false;
		}
	}
	
	protected override void OnTriggerEnter (Collider other){
		base.OnTriggerEnter(other);
		if (other.GetComponent<ObservedZone>() != null){
			beingWatched = true;
		}
	}
	
	private float ImpulseFromHeightEnum(HeightEnum requiredHeight, DifficultyEnum difficulty){
		
		int roll = (int) (Random.value * 100) + skillPenalty;
		bool failed = roll > (int) difficulty;
		
		//Debug.Log("roll is " + roll + " so I " + (failed? "failed" : "succeeded") + " because " + difficulty.ToString());
		HeightEnum actualHeight = requiredHeight;
		
		if (failed){
			
			actualHeight = /*requiredHeight == HeightEnum.low? requiredHeight + 1 :*/ requiredHeight - 1;
		}
		
		
		switch (actualHeight){
		case HeightEnum.low:
			return initialJumpVelocity;
		case HeightEnum.medium:
			return initialJumpVelocity * mediumJumpMod;
		case HeightEnum.high:
			return initialJumpVelocity * highJumpMod;
		default:
			return 0;
		}
	}
	public override bool Hurt (GameObject caller, float duration, Vector2 newVelocity)
	{
		if (caller.GetComponent<TennisBall>()){
			return false;
		}
		return base.Hurt (caller);
	}
	
	
} 