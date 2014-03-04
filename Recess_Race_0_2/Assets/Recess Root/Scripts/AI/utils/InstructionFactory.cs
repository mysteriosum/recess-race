using UnityEngine;
using System.Collections;

public class InstructionFactory {
	
	public static Instruction makeRunJump(Agent agent, Vector3 runTo, JumpRunCreationData data){
        return makeRunJump(agent, runTo, data.direction, data.jumpHoldingLenght, data.moveHoldingLenght);
    }
    
    public static Instruction makeRunJump(Agent agent, Vector3 runTo, Direction direction, float jumpHoldLenght, float jumpMoveLenght){
		Instruction run = new RunToInstruction(agent, runTo);
		Instruction jump = new JumpInstruction(agent, direction, jumpHoldLenght, jumpMoveLenght);
		run.nextInstruction = jump;
		return run;
	}
}
