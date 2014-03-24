using UnityEngine;
using System.Collections;

[System.Serializable]
public class RunToInstruction : Instruction {

    public float targetX;
    public bool arriveWithSpeedZero;

	public Direction direction;
    private float moveSpeed = 0;
	public float lastX;
    private float ranDistance = 0;
	public int stockCounter;

    public RunToInstruction(Agent agent, Vector3 targetLocation, bool arriveWithSpeedZero = false) : base(agent){
		this.targetX = targetLocation.x;
        this.arriveWithSpeedZero = arriveWithSpeedZero;
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
        float ranDistanceThisFrame = Mathf.Abs(lastX - agent.transform.position.x);
        /*if( ranDistanceThisFrame < 0.007){
            stockCounter--;
        }else{
            stockCounter = 10;
        }*/
        
		if (isInRange() || stockCounter <= 0) {
            agent.setMovingStrenght(0);
			if(stockCounter <= 0) Debug.Log ("Unstuck");
            agent.speedFactor = 1;
            isDone = true;
        }

        if (arriveWithSpeedZero && Mathf.Abs(this.agent.transform.position.x - targetX) < 7) {
            if (ranDistance < 7) {
                agent.speedFactor = (1 - ranDistance / 3f);
                if (agent.speedFactor < 0) agent.speedFactor = 0;
            } else {
                agent.speedFactor = 0.000000000001f;
            }
        }
        ranDistance += ranDistanceThisFrame;
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
        return "RunToInstruction (targetX=" + targetX + ", arriveWithSpeedZero=" + this.arriveWithSpeedZero + ")";
    }
}
