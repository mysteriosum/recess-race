using UnityEngine;
using System.Collections;

public class JumpInstruction : Instruction {

    private Vector3 targetLocation;
    private float distanceToGetBy;
    private float direction;
   // private float jumpStrenght;

    public JumpInstruction(Agent agent, Vector3 targetLocation, float distanceToGetBy = 1f)
        : base(agent) {
        this.targetLocation = targetLocation;
        this.distanceToGetBy = distanceToGetBy;
        //this.jumpStrenght = jumpStrenght;
        if (targetLocation.x > agent.transform.position.x) {
            direction = 1;
        } else {
            direction = -1;
        }
    }


    public override void start() {
        agent.setMovingStrenght(direction);
        agent.jump();
    }


    public override void update() {
        if (isInRange()) {
            this.isDone = true;
        }
    }

    private bool isInRange() {
        return (this.agent.transform.position - targetLocation).magnitude < distanceToGetBy;
    }

    public override string ToString() {
        return "JumpInstruction, Jump to (" + targetLocation.x + "," + targetLocation.y + ")";
    }
}
