using UnityEngine;
using System.Collections;

public class RunToInstruction : Instruction {
	    
	private float targetX;
    private float direction;

    public RunToInstruction(Agent agent, Vector3 targetLocation) : base(agent){
		this.targetX = targetLocation.x;
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
        if (direction == -1) {
            return this.agent.transform.position.x < targetX;
        } else {
            return this.agent.transform.position.x > targetX;
        }
    }

    public override string ToString() {
        return "RunToInstruction, run to (x=" + targetX + ")";
    }
}
