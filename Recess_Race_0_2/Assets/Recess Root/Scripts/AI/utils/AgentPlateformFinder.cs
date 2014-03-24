using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AgentPlateformFinder {

    private static int trys = 0;
    private static int currentDepth;
    private static int maxDepth = 10;

    public static LinkedPlateform generateMove(Agent agent, Plateform plateform) {
        trys = 0;

        LinkedPlateform minPlateform = null;
        int min = maxDepth;

        int targetWayPointId = agent.currentWayPoint + 1;
        List<Plateform> tryiedPlateform = new List<Plateform>();
        foreach (var link in plateform.linkedJumpPlateform) {
            if (tryiedPlateform.Contains(link.plateform)) {
                continue;
            }
			//float xToGo = getXToGetToMakeTheJump(agent,link);
			//if(canMakeItToTheJumpLocationAtFullSpeed(agent, xToGo)){
				tryiedPlateform.Add(link.plateform);
				if (link.plateform.waypointId == targetWayPointId) {
					min = 1;
					minPlateform = link;
					break;
				}
				int minForP = findHowManyMoveToWayPoint(link.plateform, targetWayPointId);
				if (minForP < min) {
					min = minForP;
					minPlateform = link;
				}
			//}
        }
		//Debug.Log("From plateform #" + plateform.id + " to wp #" + targetWayPointId + " in " + min + " jumps (trys " + trys + ").");
        if (minPlateform != null) {
            //Debug.Log("use plateform #" + minPlateform.plateform.id + " from " + minPlateform.jumpStart.ToString());
         }
        return minPlateform;
    }

	public static float getXToGetToMakeTheJump(Agent agent, LinkedPlateform link){
		float xToGo = link.startLocation.x;
		float direction = (float)link.startingDirection;
		if (agent.transform.position.x > link.startLocation.x) { // agent on right of target
			if(link.startingDirection.Equals(Direction.right)){
				xToGo -= 1.5f;
			}else if(agent.transform.position.x - link.startLocation.x < 1.5f){
				xToGo += 1.5f;
			}
		} else {
			if(link.startingDirection.Equals(Direction.left)){
				Debug.Log("aaww");
				xToGo += 1.5f;
			}else if(agent.transform.position.x - link.startLocation.x > -1.5f){
				Debug.Log("aaww2");
				xToGo -= 1.5f;
			}	
		}
		Debug.Log ("location : " + agent.transform.position.x  + ", wanted : " + link.startLocation.x + " -- xToGo: " + xToGo);
		return xToGo;
	}

	private static bool canMakeItToTheJumpLocationAtFullSpeed(Agent agent, float x){
		float runningAccelerationThreachhold = 0.9f - 0.8f * (Mathf.Abs (agent.getXSpeed()) / agent.getMaxXSpeed());
		if (runningAccelerationThreachhold < 0) runningAccelerationThreachhold = 0.1f; 
		if (Mathf.Abs (x - agent.transform.position.x) < runningAccelerationThreachhold) {
			//Debug.Log ("Jump Tooo close distance " + Mathf.Abs (x - agent.transform.position.x) + " - Threshold " + runningAccelerationThreachhold);
			return false;
		}
		return true;
	}

    private static int findHowManyMoveToWayPoint(Plateform from, int targetWayPointId) {
        trys++;
        if (currentDepth++ >= maxDepth) {
            currentDepth--;
            return maxDepth;
        }
        int min = maxDepth;
        List<Plateform> tryiedPlateform = new List<Plateform>();
        foreach (var link in from.linkedJumpPlateform) {
            if (tryiedPlateform.Contains(link.plateform)) {
                continue;
            }
            tryiedPlateform.Add(link.plateform);
            if (from.id == link.plateform.id) {
                continue;
            } else if (link.plateform.waypointId == targetWayPointId) {
                currentDepth--;
                return 1;
            } else {
                int nb = findHowManyMoveToWayPoint(link.plateform, targetWayPointId);
                min = Mathf.Min(nb, min);
            }
        }
        currentDepth--;
        return 1 + min;
    }
}
