using UnityEngine;
using System.Collections;

[System.Serializable]
public class RunToInstruction : Instruction {

    public float targetX;
    public bool arriveWithSpeedZero;

	public Direction direction;
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
		ranDistance = 0;
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
			//if(stockCounter <= 0) Debug.Log ("Unstuck");
            agent.speedFactor = 1;
            isDone = true;
        }

        if (!isDone && arriveWithSpeedZero) {
			if(Mathf.Abs(this.agent.transform.position.x - targetX) < 2.5){
				if (ranDistance > 2 ) {
					if(Mathf.Abs(this.agent.transform.position.x - targetX) < 0.5){
						float force = (targetX - this.agent.transform.position.x) / 16;
						agent.setMovingStrenght(force);
					}else{
						float force = (this.agent.transform.position.x - targetX) / 16;
						agent.setMovingStrenght(force);
					}
				} else {
					agent.setMovingStrenght(Mathf.Sign(targetX - this.agent.transform.position.x) / 8);
				}
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
