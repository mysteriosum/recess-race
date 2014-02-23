using UnityEngine;
using System.Collections;

public class RunToInstruction : Instruction {

    private Vector3 targetLocation;
    private float distanceToGetBy;
    private float direction;

    public RunToInstruction(Agent agent, Vector3 targetLocation, float distanceToGetBy = 1f) : base(agent){
        this.targetLocation = targetLocation;
        this.distanceToGetBy = distanceToGetBy;
        if (targetLocation.x > agent.transform.position.x) {
            direction = 1;
        } else {
            direction = -1;
        }
    }


    public override void start() {
        agent.setMovingStrenght(direction);
    }


    public override void update() {
        if (isInRange()) {
            agent.setMovingStrenght(0);
            isDone = true;
        }
    }

    private bool isInRange() {
        return (this.agent.transform.position - targetLocation).magnitude < distanceToGetBy;
    }

    public override string ToString() {
        return "RunToInstruction, run to (" + targetLocation.x + "," + targetLocation.y + ")";
    }
}
