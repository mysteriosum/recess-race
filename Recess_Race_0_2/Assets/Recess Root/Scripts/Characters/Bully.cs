using UnityEngine;
using System.Collections;

public class Bully : Movable {
	
	//--------------------------------------------------------------------------\\
	//-------------------------------AI variables-------------------------------\\
	//--------------------------------------------------------------------------\\
	
	float minJump = 0;
	float maxJump = 1.52f;
	public float myMaxSpeed;
	protected override float MaxSpeed {
		get {
			return myMaxSpeed;
		}
	}


	public int talent = 10;
		
	private NextJump nextJump;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		if(myMaxSpeed == 0){
			myMaxSpeed = base.maxSpeed;
		}
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		
		base.FixedUpdate();
		
		velocity = Move(velocity, controller.hAxis);
	}
	
	void OnTriggerEnter2D (Collider2D other){
		BullyInstruction instruction = other.GetComponent<BullyInstruction>();

		if (instruction) {
			debugLog("found an instruction");
			handleInstruction (instruction.configuration);		
		}

		Plateform plateform = other.GetComponent<Plateform>();
		if (plateform) {
			handlePlateform(plateform);
		}

	}

	private void handlePlateform(Plateform plateform){
		//NextJump nextJumpConfig = BullyAi.generateMove (this, plateform);
		/*this.nextJump = nextJumpConfig;
		if (nextJumpConfig == null) {
			Debug.LogError("No jump config found for " + this.name);		
		}*/
	}

    private void handleInstruction(BullyInstructionConfiguration config){
        if (config.isAJump()) {
            if (config.getDirection() == controller.hAxis && nextJump == null) {
                handleJumpInstruction(config);
            }
        } else {
            controller.hAxis = config.getDirection();
			debugLog("Walk " + config.moveDirection);
        }
	}

    private void handleJumpInstruction(BullyInstructionConfiguration config)
    {
        float target = config.getTarget();
        int targetPercentile = config.getTargetPercentile();
        int roll = generateRoll(targetPercentile);
        int result;
		debugLog("My roll starts out as " + roll);
        if (roll == targetPercentile) {
            nextJump = new NextJump(config, target);
        } else {
            int multiplier = roll < targetPercentile ? 1 : -1;
			debugLog("Calculating modifier. Multiplier: " + multiplier + ", talent: " + talent + " and difficulty " + (int)config.jumpDifficulty);
            int modifier = multiplier * (talent - (int)config.jumpDifficulty);
            result = roll + modifier;

            if ((roll > targetPercentile && result < targetPercentile) || (roll < targetPercentile && result > targetPercentile)) {
				debugLog("perfect!");
                result = targetPercentile;
                nextJump = new NextJump(config, target);
            } else {
				debugLog("Result is " + result);
                float holdTime = Mathf.Lerp(minJump, maxJump, (float)result / 100);
                bool centre = result < 100 && result >= 0;
				debugLog("jump in centre: " + centre);
                bool onExit = result >= 100;
                bool onEnter = result < 0;

                if (config.moveDirection == CommandEnum.middle && onExit) {
                    onExit = false;
                    centre = true;
                }

                nextJump = new NextJump(config, holdTime, centre, onEnter, onExit);
                if (nextJump.onEnter && grounded) {
                    velocity = Jump(velocity, JumpImpulse, nextJump.holdLength);

                }
            }
        }
    }

    private int generateRoll(int targetPercentile)
    {
        int minPercentile = targetPercentile - 50;
        int maxPercentile = targetPercentile + 50;
        return Random.Range(minPercentile, maxPercentile);
    }
	
	void OnTriggerStay2D (Collider2D other){
		BullyInstruction instruction = other.GetComponent<BullyInstruction>();
        if (instruction && nextJump != null && grounded) {
            BullyInstructionConfiguration config = instruction.configuration;
            bool goingTheRightWay = (velocity.x >= 0 && config.moveDirection == CommandEnum.right)
                                     || (velocity.x <= 0 && config.moveDirection == CommandEnum.left);
			
			if (nextJump.onEnter && goingTheRightWay){
				velocity = Jump(velocity, JumpImpulse, nextJump.holdLength);
			}
			else if (nextJump.onCentre && goingTheRightWay){
				bool passedCentre = instruction.Direction > 0? (t.position.x >= other.transform.position.x) : (t.position.x <= other.transform.position.x);
                if (config.moveDirection == CommandEnum.middle)
					passedCentre = true;		//special case for middle-jumpers, because they'll never move on 'em
				if (instruction.IsAJumpCommand && passedCentre){
					velocity = Jump(velocity, JumpImpulse, nextJump.holdLength);
				}
			}
		}
		
		if (instruction && nextJump == null){
			if (instruction.IsAJumpCommand){
				OnTriggerEnter2D(other);
			}
		}
	}
	
	void OnTriggerExit2D (Collider2D other){
		BullyInstruction instruction = other.GetComponent<BullyInstruction>();
		if (instruction & nextJump != null){
			if (nextJump.instruction == instruction.configuration){
				if (grounded){
					velocity = Jump(velocity, JumpImpulse, nextJump.holdLength);
				}
				else{
					nextJump = null;
					return;
				}
			}
		}
	}
	
	protected override Vector2 Jump (Vector2 currentVelocity, float amount){
		controller.getJump = true;
		return base.Jump (currentVelocity, amount);
	}
	
	protected Vector2 Jump(Vector2 currentVelocity, float amount, float delay){
		CancelInvoke("ResetJumpInput");
		Invoke("ResetJumpInput", delay);
		nextJump = null;
		return Jump(currentVelocity, amount);
	}
	
	void ResetJumpInput(){
		debugLog("No more jumper");
		controller.getJump = false;
	}

	void debugLog(string message){
		if(this.debug){
			Debug.Log(message);
		}
	}
}
