using UnityEngine;
using System.Collections;
using System.Linq;

public class AgentAi {

    private static float RunToEpsilon = 0.2f;
    private static float JumpToEpsilon = 3f;
	public static Instruction generateMove(Agent agent, Plateform plateform){
        Plateform to = findNextPlateformToGetToNextWayPoint(agent,plateform);
		/*if (Mathf.Abs(to.transform.position.y - agent.transform.position.y) < 1) {
			BullyInstructionConfiguration config = new BullyInstructionConfiguration(LengthEnum.hold, CommandEnum.right, DifficultyEnum.assured);
           // return new 
			//NextJump nextJump = new NextJump(config, 5f);
			//nextJump.walkDistance = 1;
			//nextJump.holdLength = 10;
			//return nextJump;
		}*/

        Instruction run = new RunToInstruction(agent, plateform.getRightCornerPosition(), RunToEpsilon);
        Instruction jump = new JumpInstruction(agent, to.getLeftCornerPosition(), JumpToEpsilon);
        run.nextInstruction = jump;
        return run;
	}

    private static int trys = 0;
    private static int currentDepth;
    private static int maxDepth = 3;

    private static Plateform findNextPlateformToGetToNextWayPoint(Agent agent, Plateform plateform) {
        trys = 0;
        Plateform minPlateform = null;
        int min = maxDepth;
        int targetWayPointId = agent.currentWayPoint + 1;
        foreach (var p in plateform.linkedPlateform) {
            if (p.waypointId == targetWayPointId) {
                min = 1;
                minPlateform = p;
                break;
            }
            int minForP = findHowManyMoveToWayPoint(p, targetWayPointId);
            if (minForP < min) {
                min = minForP;
                minPlateform = p;
            }
        }
        Debug.Log("From plateform #" + plateform.id + " to wp #" + targetWayPointId + " in " + min + " jumps (trys " + trys + ").");
        if (minPlateform != null) {
            Debug.Log("use plateform #" + minPlateform.id);
         }
        return minPlateform;
    }

    private static int findHowManyMoveToWayPoint(Plateform from, int targetWayPointId) {
        trys++;
        if (currentDepth++ >= maxDepth) {
            currentDepth--;
            return maxDepth;
        } 
        int min = maxDepth;
        foreach (var plateform in from.linkedPlateform) {
            if (from.id == plateform.id) {
                continue;
            }else if (plateform.waypointId == targetWayPointId) {
                currentDepth--;
                return 1;
            } else {
                int nb = findHowManyMoveToWayPoint(plateform, targetWayPointId);
                min = Mathf.Min(nb, min);
            }
        }
        currentDepth--;
        return 1 + min;
    }
}
