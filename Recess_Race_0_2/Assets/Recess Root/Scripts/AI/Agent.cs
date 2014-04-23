using UnityEngine;
using System.Collections;

public class Agent : Movable {

	private AgentPlateformFinder agentPlateformFinder;

    private Instruction currentInstruction;
    private Plateform lastPlateform;
    public int currentWayPoint = 0;
    public float speedFactor = 1;
	public float jumpSkill = 1;
	public float jumpDecissionSkill = 1;

	private Vector3 lastPosition;
	public float distanceDone = 111;
	private int unstuckTickNumber = 60;
	
	protected override float SecondsToMax {
		get {
			return grounded? base.SecondsToMax : 8f;
		}
	}

	protected override void Start() {
		base.Start();
		secondsToMax = 0.3f;
		agentPlateformFinder = new AgentPlateformFinder (this);
	}
	
    void Update() {
        if (currentInstruction != null) {
            currentInstruction.update();
            if (currentInstruction.isDone) {
                switchTo(currentInstruction.nextInstruction);
            }
        }
    }

    private void switchTo(Instruction instruction) {
        if (instruction != null) {
            instruction.start();
			debugLog(instruction.ToString());
        }
        this.currentInstruction = instruction;
		if (instruction == null) {
			this.setMovingStrenght (1);		
		}
    }

    protected override void FixedUpdate() {
		if (!activated) return;
        base.FixedUpdate();
        velocity = Move(velocity, controller.hAxis * speedFactor);
		antiStuck ();
    }

	private void antiStuck(){
		if (++unstuckTickNumber >= 30) {
			this.lastPosition = this.transform.position;
			unstuckTickNumber = 0;
			if(distanceDone < 0.1){
				if (debug)
					Debug.Log ("AGENT UNSTUCK ");
				if(this.currentInstruction == null){
					switchTo(null);
				}else{
					switchTo(this.currentInstruction.nextInstruction);
				}
			}
			distanceDone = 0;
		} else {
			distanceDone += (this.transform.position - this.lastPosition).magnitude;
			this.lastPosition = this.transform.position;
		}
	}

    void OnTriggerEnter2D(Collider2D other) {
		AgentInstructionTrigger instruction = other.GetComponent<AgentInstructionTrigger>();
        Plateform plateform = other.GetComponent<Plateform>();
        if (instruction) {
            if (currentInstruction == null) {
                switchTo(instruction.getInstruction(this));
            }
        } else if (plateform) {
			if (isCurrentPlateform(plateform)) return;
			if(plateform.isLastWayPoint){
				this.setMovingStrenght(1);
			}else{
				lastPlateform = plateform;
				if (plateform.waypointId > 0) {
					this.currentWayPoint = plateform.waypointId;
				}
				handlePlateform(plateform);
				Debug.Log("HANDLE");
			}
			
		}
		
	}
	
	private bool isCurrentPlateform(Plateform plateform){
		return currentInstruction != null && lastPlateform != null && lastPlateform.id == plateform.id;
	}


    private void handlePlateform(Plateform plateform) {
		PreciseJumpConfig preciseJump = this.agentPlateformFinder.generateMove(plateform);
        if (preciseJump == null) {
            Debug.LogError("No More jump Possible for agent : " + this.name);
        } else {
			Instruction instructionsToGetThere = InstructionFactory.makeInstruction(this,preciseJump.instruction);
			//Debug.Log("test" + linkedPlateform.startLocation);
			if(preciseJump.instruction.needRunCharge){
				makeRunCharge(preciseJump,instructionsToGetThere);
			}else{
                makeJump(preciseJump, instructionsToGetThere);
			}
        }
    }

	private void makeJump(PreciseJumpConfig preciseJump, Instruction instructionsToGetThere) {
        if (Mathf.Abs(this.transform.position.x - preciseJump.startLocation.x) < 0.1) {
            switchTo(instructionsToGetThere);
        } else {
            if ((this.transform.position.x < preciseJump.startLocation.x && preciseJump.startingDirection.Equals(Direction.left))
               || (this.transform.position.x > preciseJump.startLocation.x && preciseJump.startingDirection.Equals(Direction.right))) {
				Instruction run = new RunToInstruction(this, getXOffsetedPosition(preciseJump.startLocation), true);
                Instruction wait = new WaitInstruction(this, 0.4f);
                run.nextInstruction = wait;
				wait.nextInstruction = instructionsToGetThere;
				//Debug.Log(instructionsToGetThere.ToString());
                switchTo(run);
            } else {
				//debugLog("On attend pas");
				Instruction run = new RunToInstruction(this, getXOffsetedPosition(preciseJump.startLocation));
                run.nextInstruction = instructionsToGetThere;
                switchTo(run);
            }

        }
    }

	private void makeRunCharge(PreciseJumpConfig preciseJump, Instruction instructionsToGetThere){
		float xToGo = AgentPlateformFinder.getXToGetToMakeTheJump(this, preciseJump);
		if (xToGo != preciseJump.startLocation.x) {
			//debugLog("Making a run charge prepositioning");
			Instruction overRun = new RunToInstruction(this, getXOffsetedPosition(new Vector3(xToGo, preciseJump.startLocation.y, 0)));
			Instruction run = new RunToInstruction(this, getXOffsetedPosition(preciseJump.startLocation));
			overRun.nextInstruction = run;
			run.nextInstruction = instructionsToGetThere;
			switchTo(overRun);
		} else {
			//debugLog("RUN");
			Instruction run = new RunToInstruction(this, getXOffsetedPosition(preciseJump.startLocation));
			run.nextInstruction = instructionsToGetThere;
			switchTo(run);
		}

	}

	private void setInstructionAgentToThis(Instruction instruction){
		if (instruction == null) return;
		instruction.agent = this;
		setInstructionAgentToThis (instruction.nextInstruction);
	}

	public Vector3 getXOffsetedPosition(Vector3 v){
		return new Vector3 (v.x + (getRandomSkillFactor()-1), v.y, v.z);
	}

	public float getRandomSkillFactor(){
		return Random.Range (this.jumpSkill , 2 - this.jumpSkill);
	}

    void debugLog(string message) {
        if (this.debug) {
            Debug.Log(message);
        }
    }

    internal void setMovingStrenght(float strenght) {
        controller.hAxis = strenght;
    }

    public void jump(/*float jumpStrenght*/) {
        controller.getJump = true;
        this.velocity = base.Jump(velocity, JumpImpulse);
    }

    public void stopJumping() {
        controller.getJump = false;
    }
    public float getXSpeed() {
        return this.velocity.x;
    }

    public float getMaxXSpeed() {
        return this.maxSpeed;
    }
	
    internal bool isGrounded() {
        return this.grounded;
    }
}

