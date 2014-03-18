using UnityEngine;
using System.Collections;

public class Agent : Movable {

    private Instruction currentInstruction;
    private Plateform lastPlateform;
    public int currentWayPoint = 0;


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
        }
        this.currentInstruction = instruction;
    }

    protected override void FixedUpdate() {
		if (!activated) return;
        base.FixedUpdate();
        velocity = Move(velocity, controller.hAxis);
    }


    void OnTriggerEnter2D(Collider2D other) {
		AgentInstructionTrigger instruction = other.GetComponent<AgentInstructionTrigger>();
        Plateform plateform = other.GetComponent<Plateform>();
        if (instruction) {
            if (currentInstruction == null) {
                switchTo(instruction.getInstruction(this));
            }
        } else if (plateform) {
			if (currentInstruction != null && lastPlateform != null && lastPlateform.id == plateform.id) return;
            lastPlateform = plateform;
            if (plateform.waypointId > 0) {
               this.currentWayPoint = plateform.waypointId;
            }
            handlePlateform(plateform);
        }

    }


    private void handlePlateform(Plateform plateform) {
        LinkedPlateform linkedPlateform = AgentPlateformFinder.generateMove(this, plateform);
        if (linkedPlateform == null) {
            Debug.LogError("No More jump Possible for agent : " + this.name);
        } else {
			Instruction instructionsToGetThere = InstructionFactory.makeInstruction(this,linkedPlateform.instruction);
			if(linkedPlateform.instruction.needRunCharge){
				makeRunCharge(linkedPlateform,instructionsToGetThere);
			}else{
				switchTo(instructionsToGetThere);
			}
        }
    }

	private void makeRunCharge(LinkedPlateform linkedPlateform, Instruction instructionsToGetThere){
		float xToGo = AgentPlateformFinder.getXToGetToMakeTheJump(this, linkedPlateform);
		if (xToGo != linkedPlateform.startLocation.x) {
			Debug.Log("Making a run charge prepositioning");
			Instruction overRun = new RunToInstruction(this, new Vector3(xToGo, linkedPlateform.startLocation.y, 0));
			Instruction run = new RunToInstruction(this, linkedPlateform.startLocation);
			overRun.nextInstruction = run;
			run.nextInstruction = instructionsToGetThere;
			switchTo(overRun);
		} else {
			Debug.Log("RUN");
			Instruction run = new RunToInstruction(this, linkedPlateform.startLocation);
			run.nextInstruction = instructionsToGetThere;
			switchTo(run);
		}

	}

	private void setInstructionAgentToThis(Instruction instruction){
		if (instruction == null) return;
		instruction.agent = this;
		setInstructionAgentToThis (instruction.nextInstruction);
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

   /* protected override Vector2 Jump(Vector2 currentVelocity, float amount) {
        controller.getJump = true;
        return base.Jump(currentVelocity, amount);
    }

    protected Vector2 Jump(Vector2 currentVelocity, float amount, float delay) {
        CancelInvoke("JumpDone");
        Invoke("JumpDone", delay);
        nextJump = null;
        return Jump(currentVelocity, amount);
    }

    void JumpDone() {
        debugLog("Stop The Jump!");
        controller.getJump = false;
    }*/

}
