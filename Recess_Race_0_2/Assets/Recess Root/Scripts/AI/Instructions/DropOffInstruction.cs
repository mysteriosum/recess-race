using UnityEngine;
using System.Collections;

[System.Serializable]
public class DropOffInstruction : Instruction {

	[System.Serializable]
	public class CreationData : InstructionCreationData{
		public Direction firstDirection;
		public float moveXLenght;
		public float totalDrop;
		public float moveAgainAfterYMoved;
		public Direction endDropDirection;
	}

	public Direction firstDirection;
	public float moveXLenght;
	public float totalDrop;
	public float dropLenghtToStartMovingAgain;
	public Direction endDropDirection;

	public float startX;
	public float targetX;
	public float startY;
	public float removeTargetY;
	public float finalY;
	public bool moving;
	public bool droping;
	public bool removing;

	public DropOffInstruction(Agent agent, Direction firstDirection, float moveXLenght, float totalDrop, float dropLenghtToStartMovingAgain, Direction endDropDirection)
	: base(agent) {
		this.firstDirection = firstDirection;
		this.moveXLenght = moveXLenght;
		this.totalDrop = totalDrop;
		this.dropLenghtToStartMovingAgain = dropLenghtToStartMovingAgain;
		this.endDropDirection = endDropDirection;
	}
	
	
	public override void start() {
		agent.setMovingStrenght((float)firstDirection);
		this.startX = agent.transform.position.x;
		this.targetX = startX + (float)firstDirection * moveXLenght;
		this.startY = agent.transform.position.y;
		this.removeTargetY = startY - dropLenghtToStartMovingAgain;
		this.finalY = startY - totalDrop;
		this.moving = true;
		this.droping = false;
		this.removing = false;
	}

	public override void update() {
		if (isDone) {
			return;		
		}else if (moving) {
			if(isInTargetXRange()){
				this.moving = false;
				this.droping = true;
				this.agent.setMovingStrenght(0);
			}		
		}else if (droping) {
			if(isInTargetYRange()){
				this.droping = false;
				this.removing = true;
				this.agent.setMovingStrenght((float)endDropDirection);
			}		
		}else if (removing) {
			if(isInFinalYRange()){
				this.removing = false;
				this.agent.setMovingStrenght(0);
				this.isDone = true;
			}		
		}
	}

	private bool isInTargetXRange() {
		if ((int)firstDirection == -1) {
			return this.agent.transform.position.x < targetX;
		} else {
			return this.agent.transform.position.x > targetX;
		}
	}

	private bool isInTargetYRange() {
		return this.agent.transform.position.y < this.removeTargetY;
	}

	private bool isInFinalYRange() {
		return this.agent.transform.position.y < this.finalY;
	}

	public override Direction getStartingDirection (){
		return this.firstDirection;
	}
}
