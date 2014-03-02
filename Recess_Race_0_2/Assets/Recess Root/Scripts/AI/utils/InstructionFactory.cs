using UnityEngine;
using System.Collections;

public class InstructionFactory {
	
	public static Instruction makeRunJump(Agent agent, Vector3 runTo, Direction direction, float jumpHoldLenght, float jumpMoveLenght, float runToEpsilon = 0.3f, float jumpToEpsilon = 3f){
		Instruction run = new RunToInstruction(agent, runTo	, runToEpsilon);
		Instruction jump = new JumpInstruction(agent, direction, jumpHoldLenght, jumpMoveLenght);
		run.nextInstruction = jump;
		return run;
	}
}
