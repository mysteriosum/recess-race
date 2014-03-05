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
			Debug.Log (instruction.ToString());
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
        LinkedJumpPlateform getToPlateform = AgentPlateformFinder.generateMove(this, plateform);
        if (getToPlateform == null) {
            Debug.LogError("No More jump Possible for agent : " + this.name);
        } else {
			float xToGo = AgentPlateformFinder.getXToGetToMakeTheJump(this, getToPlateform);

			Instruction instructionsToGetThere;
            JumpRunCreationData data = getToPlateform.data;
            if (data.jump){ 
                instructionsToGetThere = InstructionFactory.makeRunJump(this, getToPlateform.jumpStart, getToPlateform.data);
            } else {
                Vector3 runto = getToPlateform.jumpStart;
                runto.x += ((int)data.direction * data.moveHoldingLenght);
                instructionsToGetThere = new RunToInstruction(this, runto);
                
            }

			if(xToGo !=getToPlateform.jumpStart.x){
				//Debug.Log("Must overRun to get there :)  " + xToGo );
				Instruction overRun = new RunToInstruction(this, new Vector3(xToGo, getToPlateform.jumpStart.y, 0));
				overRun.nextInstruction = instructionsToGetThere;
				switchTo(overRun);
			}else{
				switchTo(instructionsToGetThere);
			}
        }
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
