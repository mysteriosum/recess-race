using UnityEngine;
using System.Collections;

[System.Serializable]
public class RunToInstruction : Instruction {

	public float targetX;
	public Direction direction;
	public float lastX;
	public int stockCounter;

    public RunToInstruction(Agent agent, Vector3 targetLocation) : base(agent){
		this.targetX = targetLocation.x;
		stockCounter = 10;
    }


    public override void start() {
		if (targetX > agent.transform.position.x) {
			direction = Direction.right;
		} else {
			direction = Direction.left;
		}
        agent.setMovingStrenght((int)direction);
		this.lastX = agent.transform.position.x;
    }


	public override void update() {
		if(Mathf.Abs(lastX - agent.transform.position.x) < 0.007){
			stockCounter--;
		}else{
			stockCounter = 10;
		}
		if (isInRange() || stockCounter <= 0) {
            agent.setMovingStrenght(0);
			if(stockCounter <= 0) Debug.Log ("Unstuck");
            isDone = true;
        }
		this.lastX = agent.transform.position.x;
    }

    private bool isInRange() {
        if (direction == Direction.left) {
            return this.agent.transform.position.x < targetX;
        } else {
            return this.agent.transform.position.x > targetX;
        }
    }

	public override Direction getStartingDirection (){
		return this.direction;	
	}

    public override string ToString() {
        return "RunToInstruction, run to (x=" + targetX + ")";
    }
}
