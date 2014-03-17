using UnityEngine;
using System.Collections;

public class AgentInstructionTrigger : MonoBehaviour {

	public float runBeforeInstruction = 0;

	public InstructionCreationData data = new InstructionCreationData();

	public Instruction getInstruction(Agent agent){
		Instruction instruction = null;
		switch (data.type) {
		case InstructionCreationData.InstructionType.Run: instruction = makeRunInstruction(agent); break;
		case InstructionCreationData.InstructionType.Jump : instruction =  makeJumpInstruction(agent); break;
		case InstructionCreationData.InstructionType.DropOff : instruction =  makeDropOffInstruction(agent); break;
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
		runto.x += ((int)data.direction) * (data.moveHoldingLenght + runBeforeInstruction);
		return new RunToInstruction(agent, runto);
	}

	public Instruction makeJumpInstruction(Agent agent){
		return new JumpInstruction (agent, data.direction, data.jumpHoldingLenght, data.moveHoldingLenght, this.data.distanceToStartRunningAgain, this.data.endDirection, this.data.totalDistanceAfterMoveAgain);
	}

	public Instruction makeDropOffInstruction(Agent agent){
		return new DropOffInstruction (agent, data.direction, data.moveHoldingLenght, data.totalDistanceAfterMoveAgain, data.distanceToStartRunningAgain, data.endDirection);
	}


	void OnDrawGizmos(){
		Transform t = GetComponent<Transform> ();
		BoxCollider2D derCollider = GetComponent<BoxCollider2D> ();
		Color myColor;

		float alpha = 1;
		Vector3 size = Vector3.one;
		if (derCollider)
			size = (Vector3)derCollider.size;

		switch (data.type) {
		case InstructionCreationData.InstructionType.Run: 
			myColor = new Color (1 , 0, 1, alpha);
			break;
		case InstructionCreationData.InstructionType.Jump : 
			myColor = new Color (1, 1 , 0, alpha);
			break;
		default : 
			myColor = new Color (0, 1, 1 , alpha); 
			break;
		}

		Gizmos.color = myColor;
		Gizmos.DrawCube (t.position, size);
	}
}
