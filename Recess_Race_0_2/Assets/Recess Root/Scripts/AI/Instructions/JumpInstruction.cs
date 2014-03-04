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

	public JumpInstruction(Agent agent, Direction direction, float holdLenght = 13, float moveLenght = 13, float distanceToGetBy = 1f)
        : base(agent) {
        this.distanceToGetBy = distanceToGetBy;
		this.direction = direction;
		this.holdLenght = holdLenght;
		this.moveLenght = moveLenght;
    }


    public override void start() {
        agent.setMovingStrenght((float)direction);
        agent.jump();
		this.startX = agent.transform.position.x;
		this.moving = true;
		this.holding = true;
    }


    public override void update() {
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
