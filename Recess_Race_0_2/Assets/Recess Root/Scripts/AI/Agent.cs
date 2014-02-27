using UnityEngine;
using System.Collections;

public class Agent : Movable {

    private Instruction currentInstruction;


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
        Debug.Log(currentInstruction);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        velocity = Move(velocity, controller.hAxis);
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (currentInstruction != null) return;
        BullyInstruction instruction = other.GetComponent<BullyInstruction>();

        Plateform plateform = other.GetComponent<Plateform>();
        if (plateform) {
            handlePlateform(plateform);
        }

       /* if (instruction) {
            debugLog("found an instruction");
            handleInstruction(instruction.configuration);
        }

       */

    }


    private void handlePlateform(Plateform plateform) {
        debugLog("found an instruction");
        switchTo( AgentAi.generateMove(this, plateform) );
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
