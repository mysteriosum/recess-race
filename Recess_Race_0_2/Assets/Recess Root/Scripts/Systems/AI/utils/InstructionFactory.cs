using UnityEngine;
using System.Collections;

public class InstructionFactory {
	    
    /*public static Instruction makeRunJump(Agent agent, Vector3 runTo, Direction direction, float jumpHoldLenght, float jumpMoveLenght){
		Instruction run = new RunToInstruction(agent, runTo);
		Instruction jump = new JumpInstruction(agent, direction, jumpHoldLenght, jumpMoveLenght);
		run.nextInstruction = jump;
		return run;
	}*/

	public static Instruction makeInstruction(Agent agent, InstructionCreationData data){
		Instruction instruction = null;

		if (data.type.Equals (InstructionCreationData.InstructionType.Run)) {
			Vector3 p = new Vector3 (agent.transform.position.x + data.moveHoldingLenght * agent.getRandomSkillFactor (), agent.transform.position.y);
			instruction = new RunToInstruction (agent, p);
		} else if (data.type.Equals (InstructionCreationData.InstructionType.Jump)) {
			instruction = new JumpInstruction (agent 
                                  , data.direction, data.jumpHoldingLenght * agent.getRandomSkillFactor ()
                                  , data.moveHoldingLenght * agent.getRandomSkillFactor ()
                                  , data.distanceToStartRunningAgain * agent.getRandomSkillFactor ()
                                  , data.endDirection
                                  , data.totalDistanceAfterMoveAgain * agent.getRandomSkillFactor ());
		} else if (data.type.Equals (InstructionCreationData.InstructionType.DropOff)) {
			instruction = new DropOffInstruction (agent, data.direction, data.moveHoldingLenght, data.totalDistanceAfterMoveAgain, data.distanceToStartRunningAgain, data.endDirection);
		}

		return instruction;
	}
}
