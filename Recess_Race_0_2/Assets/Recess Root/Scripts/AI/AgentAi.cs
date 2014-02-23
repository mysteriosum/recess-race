using UnityEngine;
using System.Collections;
using System.Linq;

public class AgentAi {

    private static float RunToEpsilon = 0.2f;
	public static Instruction generateMove(Agent agent, Plateform plateform){
		Plateform to = plateform.linkedPlateform.First ();
		/*if (Mathf.Abs(to.transform.position.y - agent.transform.position.y) < 1) {
			BullyInstructionConfiguration config = new BullyInstructionConfiguration(LengthEnum.hold, CommandEnum.right, DifficultyEnum.assured);
           // return new 
			//NextJump nextJump = new NextJump(config, 5f);
			//nextJump.walkDistance = 1;
			//nextJump.holdLength = 10;
			//return nextJump;
		}*/

        Instruction run = new RunToInstruction(agent, plateform.getRightCornerPosition(), RunToEpsilon);
        Instruction jump = new JumpInstruction(agent, plateform.linkedPlateform.First().getLeftCornerPosition(), RunToEpsilon);
        run.nextInstruction = jump;
        return run;
	}


}
