using UnityEngine;
using System.Collections;

public class AgentInstructionTrigger : MonoBehaviour {

	
	public enum InstructionType{Run, Jump, DropOff}
	
	public InstructionType instructionType;

	public float runBeforeInstruction = 0;

	public float jumpHoldingLenght = 13;
	public float moveHoldingLenght = 13;
	public Direction direction = Direction.right;

	public float distanceToStartRunningAgain;
	public Direction endDirection = Direction.right;
	public float totalDropOff;

	public Instruction getInstruction(Agent agent){
		Instruction instruction = null;
		switch (instructionType) {
		case InstructionType.Run: instruction = makeRunInstruction(agent); break;
		case InstructionType.Jump : instruction =  makeJumpInstruction(agent); break;
		case InstructionType.DropOff : instruction =  makeDropOffInstruction(agent); break;
		}
		if (runBeforeInstruction == 0) {
			return instruction;	
		} else {
			Vector3 runto = this.transform.position;
			runto.x += runBeforeInstruction;
			Instruction run = new RunToInstruction(agent, runto);
			run.nextInstruction = instruction;
			return run;
		}
	}

	public Instruction makeRunInstruction(Agent agent){
		Vector3 runto = this.transform.position;
		runto.x += ((int)direction) * (moveHoldingLenght + runBeforeInstruction);
		return new RunToInstruction(agent, runto);
	}

	public Instruction makeJumpInstruction(Agent agent){
		return new JumpInstruction (agent, direction, jumpHoldingLenght, moveHoldingLenght);
	}

	public Instruction makeDropOffInstruction(Agent agent){
		return new DropOffInstruction (agent, direction, moveHoldingLenght, totalDropOff, distanceToStartRunningAgain, endDirection);
	}


	void OnDrawGizmos(){
		Transform t = GetComponent<Transform> ();
		BoxCollider2D derCollider = GetComponent<BoxCollider2D> ();
		Color myColor;

		float alpha = 1;
		Vector3 size = Vector3.one;
		if (derCollider)
			size = (Vector3)derCollider.size;

		switch (instructionType) {
		case InstructionType.Run: 
			myColor = new Color (this.moveHoldingLenght / 13f , 1, 0, alpha);
			break;
		case InstructionType.Jump : 
			myColor = new Color (1, this.jumpHoldingLenght / 13f , 0, alpha);
			break;
		default : 
			myColor = new Color (0, 1, this.jumpHoldingLenght / 13f , alpha); 
			break;
		}

		Gizmos.color = myColor;
		Gizmos.DrawCube (t.position, size);
	}
}
