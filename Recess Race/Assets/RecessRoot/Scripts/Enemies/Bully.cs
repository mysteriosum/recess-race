using UnityEngine;
using System.Collections;

public class Bully : Character {
	
	private float input = 0;
	//private HeightEnum jumpHeight = HeightEnum.none;
	
	public float mediumJumpMod = 1.5f;
	public float highJumpMod = 2.0f;
	
	GameObject justJumpedFrom;
	
	protected int skillModifier = 0;
	
	void Start(){
		base.Start ();
		
		//TEST to see if a lower skill modifier is significant
		skillModifier = 20;
	}
	
	void Update() {
		base.Update ();
	}
	
	protected override void DoInputs ()
	{
		velocity = Move (velocity, input);
	}
	
	void OnTriggerStay (Collider other){
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
	
	void OnTriggerExit (Collider other){
		if (other.gameObject == justJumpedFrom){
			justJumpedFrom = null;
		}
	}
	
	private float ImpulseFromHeightEnum(HeightEnum requiredHeight, DifficultyEnum difficulty){
		
		int roll = (int) (Random.value * 100) + skillModifier;
		bool failed = roll > (int) difficulty;
		
		Debug.Log("roll is " + roll + " so I " + (failed? "failed" : "succeeded"));
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
	
	public override bool Hurt (GameObject caller)
	{
		if (caller.GetComponent<TennisBall>()){
			return false;
		}
		return base.Hurt (caller);
	}
	
} 