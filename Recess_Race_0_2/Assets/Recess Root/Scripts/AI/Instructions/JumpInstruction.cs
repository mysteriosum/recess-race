using UnityEngine;
using System.Collections;

[SerializeField]
public enum Direction { left = -1 , right = 1}

public class JumpInstruction : Instruction {

    private float distanceToGetBy;
    private Direction direction;
	private float holdLenght;
	private bool holding;
	private float moveLenght;
	private bool moving;
	private float startX;
	
	private float lastX;
	private int stockCounter;

	public JumpInstruction(Agent agent, Direction direction, float holdLenght = 13, float moveLenght = 13, float distanceToGetBy = 1f)
        : base(agent) {
        this.distanceToGetBy = distanceToGetBy;
		this.direction = direction;
		this.holdLenght = holdLenght;
		this.moveLenght = moveLenght;
		stockCounter = 10;
    }


    public override void start() {
        agent.setMovingStrenght((float)direction);
        agent.jump();
		this.startX = agent.transform.position.x;
		this.moving = true;
		this.holding = true;
    }


    public override void update() {
		if(Mathf.Abs(lastX - agent.transform.position.x) < 0.007){
			stockCounter--;
		}else{
			stockCounter = 10;
		}
		
		this.lastX = agent.transform.position.x;
		if (stockCounter <= 0) {
			moving = false;
			this.isDone = true;
			agent.setMovingStrenght(0);	
			agent.stopJumping();
		}

		 
		if (moving && Mathf.Abs (startX - agent.transform.position.x) >= moveLenght) {
			moving = false;
			agent.setMovingStrenght(0);
		}
		if (holding && Mathf.Abs (startX - agent.transform.position.x) >= holdLenght) {
			holding = false;
			agent.stopJumping();
		}
        if ((!moving && !holding)) {
			this.isDone = true;
			agent.stopJumping();
        }
    }

    public override string ToString() {
        return "JumpInstruction, Jump )";
    }
}
