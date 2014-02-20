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
	
	private class NextJump {
		public float holdLength;
		public bool onEnter;
		public bool onExit;
		public bool onCentre;

		public BullyInstructionConfiguration instruction;


        public NextJump(BullyInstructionConfiguration instruction, float holdLength)
        {
			this.holdLength = holdLength;
			this.onCentre = true;
			this.onExit = false;
			this.onEnter = false;
			this.instruction = instruction;
		}

        public NextJump(BullyInstructionConfiguration instruction, float holdLength, bool onEnter)
        {
			this.holdLength = holdLength;
			this.onEnter = onEnter;
			this.onExit = !onEnter;
			this.onCentre = false;
			this.instruction = instruction;
		}

        public NextJump(BullyInstructionConfiguration instruction, float holdLength, bool onCentre, bool onEnter, bool onExit)
        {
			this.holdLength = holdLength;
			this.onEnter = onEnter;
			this.onExit = onExit;
			this.onCentre = onCentre;
			this.instruction = instruction;
		}
		
	}
	
	private NextJump nextJump;
	
	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		base.FixedUpdate();
		
		velocity = Move(velocity, controller.hAxis);
	}
	
	void OnTriggerEnter2D (Collider2D other){
		BullyInstruction instruction = other.GetComponent<BullyInstruction>();

		if (!instruction) return;

		Debug.Log("found an instruction");
		handleInstruction (instruction.configuration);
	}

    private void handleInstruction(BullyInstructionConfiguration config)
    {
        if (config.isAJump()) {
            if (config.getDirection() == controller.hAxis && nextJump == null) {
                handleJumpInstruction(config);
            }
        } else {
            controller.hAxis = config.getDirection();
            Debug.Log("Walk " + config.moveDirection);
        }
	}

    private void handleJumpInstruction(BullyInstructionConfiguration config)
    {
        float target = config.getTarget();
        int targetPercentile = config.getTargetPercentile();
        int roll = generateRoll(targetPercentile);
        int result;
        Debug.Log("My roll starts out as " + roll);
        if (roll == targetPercentile) {
            nextJump = new NextJump(config, target);
        } else {
            int multiplier = roll < targetPercentile ? 1 : -1;
            Debug.Log("Calculating modifier. Multiplier: " + multiplier + ", talent: " + talent + " and difficulty " + (int)config.jumpDifficulty);
            int modifier = multiplier * (talent - (int)config.jumpDifficulty);
            result = roll + modifier;

            if ((roll > targetPercentile && result < targetPercentile) || (roll < targetPercentile && result > targetPercentile)) {
                Debug.Log("perfect!");
                result = targetPercentile;
                nextJump = new NextJump(config, target);
            } else {
                Debug.Log("Result is " + result);
                float holdTime = Mathf.Lerp(minJump, maxJump, (float)result / 100);
                bool centre = result < 100 && result >= 0;
                Debug.Log("jump in centre: " + centre);
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
	
	protected override Vector2 Jump (Vector2 currentVelocity, float amount)
	{
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
		Debug.Log("No more jumper");
		controller.getJump = false;
	}
}
