using UnityEngine;
using System.Collections;

public class Bully : Character {
	
	private float input = 0;
	//private HeightEnum jumpHeight = HeightEnum.none;
	
	public float mediumJumpMod = 1.5f;
	public float highJumpMod = 2.0f;
	
	void Start(){
		base.Start ();
	}
	
	void Update() {
		base.Update ();
	}
	
	protected override void DoInputs ()
	{
		velocity = Move (velocity, input);
		Debug.Log("MY input is " + input);
	}
	
	void OnTriggerStay (Collider other){
		BullyInstruction instruction = other.gameObject.GetComponent<BullyInstruction>();
		
		if (instruction){
			input = instruction.GetDirection();
			HeightEnum jumpHeight = instruction.GetImpulse ();
			if (grounded && jumpHeight != HeightEnum.none){
				Jump (ImpulseFromHeightEnum(jumpHeight));
			}
		}
	}
	
	float ImpulseFromHeightEnum(HeightEnum he){
		switch (he){
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
	
}