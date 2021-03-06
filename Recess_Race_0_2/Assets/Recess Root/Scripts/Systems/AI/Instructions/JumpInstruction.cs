using UnityEngine;
using System.Collections;

public enum Direction { left = -1 , right = 1}

[System.Serializable]
public class JumpInstruction : Instruction {

    public Direction direction;
	public float holdLenght;
	public float moveLenght;
	public float moveAgainAfterYMoved;
	public Direction moveAgainDirection;
	public float moveAgainMoveLenght;

	public bool holding;
	public bool moving;
	public bool removing;
	public bool doneMoving;
	public float startX;
	
	private float lastX;
	private float lastY;
	private float endX;
	private float yDistance;
	private int stockCounter;

	public JumpInstruction(Agent agent, Direction direction, float holdLenght, float moveLenght
	                       , float moveAgainAfterYMoved , Direction moveAgainDirection,float moveAgainMoveLenght): base(agent) {
		this.direction = direction;
		this.holdLenght = holdLenght;
		this.moveLenght = moveLenght;
		this.moveAgainAfterYMoved = moveAgainAfterYMoved;
		this.moveAgainDirection = moveAgainDirection;
		this.moveAgainMoveLenght = moveAgainMoveLenght;
		stockCounter = 10;
    }


    public override void start() {
        agent.setMovingStrenght((float)direction);
        agent.jump();
		this.startX = agent.transform.position.x;
		this.moving = true;
		this.holding = true;
		this.lastY = this.agent.transform.position.y;
    }


    public override void update() {
		yDistance += Mathf.Abs(this.agent.transform.position.y - lastY);
		antiStuck ();
		this.lastY = this.agent.transform.position.y;

		if (this.isDone) return;
		
		if (holding && Mathf.Abs (startX - agent.transform.position.x) >= holdLenght) {
			holding = false;
			agent.stopJumping();
		}
		if (doneMoving) {
			this.isDone = true;
			agent.stopJumping();
		}else if (moving) {
			if (Mathf.Abs (startX - agent.transform.position.x) >= moveLenght) {
				moving = false;
				agent.setMovingStrenght (0);
			}
		} else {
			if(removing || moveAgainAfterYMoved == 0){
				if (Mathf.Abs (startX - agent.transform.position.x) >= moveAgainMoveLenght || agent.isGrounded()) {
					removing = false;
					//agent.setMovingStrenght ((float)direction);
					agent.setMovingStrenght (0);
					doneMoving = true;
				}
			}else if(yDistance > this.moveAgainAfterYMoved){
				removing = true;
				agent.setMovingStrenght ((int)this.moveAgainDirection);
				startX = this.agent.transform.position.x;
			}
		}
        
    }

	private void antiStuck(){
		//Debug.Log (Mathf.Abs(lastX - agent.transform.position.x) + " - " +  Mathf.Abs(lastY - agent.transform.position.y));
		if(Mathf.Abs(lastX - agent.transform.position.x) < 0.007 && Mathf.Abs(lastY - agent.transform.position.y) < 0.007){
			stockCounter--;
		}else{
			stockCounter = 10;
		}
		this.lastX = agent.transform.position.x;

        if (stockCounter <= 0 && this.agent.isGrounded()) {
            moving = false;
            this.isDone = true;
			agent.setMovingStrenght((float)direction);
            agent.stopJumping();
			Debug.Log ("UnStuck");
        }
	}

	public override Direction getStartingDirection (){
		return this.direction;
	}

    public override string ToString() {
        return "JumpInstruction, Jump";
    }
}
