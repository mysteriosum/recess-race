using UnityEngine;
using System.Collections;

public class InstructionFactory {
	    
    public static Instruction makeRunJump(Agent agent, Vector3 runTo, Direction direction, float jumpHoldLenght, float jumpMoveLenght){
		Instruction run = new RunToInstruction(agent, runTo);
		Instruction jump = new JumpInstruction(agent, direction, jumpHoldLenght, jumpMoveLenght);
		run.nextInstruction = jump;
		return run;
	}

	public static Instruction makeInstruction(Agent agent, InstructionCreationData creationData){
		Instruction instruction = null;

		if (creationData is RunToInstruction.CreationData) {
			RunToInstruction.CreationData runData = (RunToInstruction.CreationData) creationData;
			Vector3 p = new Vector3(agent.transform.position.x + runData.runDistance, agent.transform.position.y);
			instruction = new RunToInstruction(agent,p);
		} else if (creationData is JumpInstruction.CreationData) {
			JumpInstruction.CreationData jumpData = (JumpInstruction.CreationData) creationData;		
			instruction = new JumpInstruction(agent, jumpData.startingDirection,jumpData.holdLenght, jumpData.moveLenght);
		}

		if (instruction != null && creationData.nextInstructionCreationData != null) {
			instruction.nextInstruction = makeInstruction(agent, creationData.nextInstructionCreationData);		
		}
		return instruction;

	}
}
