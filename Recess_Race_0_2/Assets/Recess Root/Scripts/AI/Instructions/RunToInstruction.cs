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
			float distance = Mathf.Abs(this.agent.transform.position.x - targetX);
			float force = 1;
			if(distance < 0.5){
				force = (ranDistance > 1 )? 0.03f : 0.03f ;
				Debug.Log("near" + force);
			} else if(distance < 0.9){
				force = (ranDistance > 1 )? -1f : 0.4f ;
				Debug.Log("getting" + force);
			} else if(distance < 1.2){
				force = (ranDistance > 1 )? -1f : 0.7f ;
				Debug.Log("getting" + force);
			} else if(distance < 1.5){
				force = (ranDistance > 1 )? 0.2f : 0.9f ;
				Debug.Log("far" + force);
			}
			agent.setMovingStrenght( (int)this.direction * force);
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
