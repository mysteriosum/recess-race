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
            if (tryiedPlateform.Contains(link.plateform)) continue;

			tryiedPlateform.Add(link.plateform);
			if (link.plateform.waypointId == targetWayPointId) {
				min = 1;
				minPlateform = link;
				break;
			}
			int minForP = findHowManyMoveToWayPoint(link.plateform, null, targetWayPointId);
			if (minForP < min) {
				min = minForP;
				minPlateform = link;
			}
        }
		Debug.Log("From plateform #" + plateform.id + " to wp #" + targetWayPointId + " in " + min + " jumps (trys " + trys + ").");
        if (minPlateform != null) {
            //Debug.Log("use plateform #" + minPlateform.plateform.id + " from " + minPlateform.jumpStart.ToString());
         }
        return minPlateform;
    }

	public static float getXToGetToMakeTheJump(Agent agent, LinkedPlateform link){
		float xToGo = link.startLocation.x;
		if (agent.transform.position.x > link.startLocation.x) { // agent on right of target
			if(link.startingDirection.Equals(Direction.right)){
				xToGo -= 1.5f;
			}else if(agent.transform.position.x - link.startLocation.x < 1.5f){
				xToGo += 1.5f;
			}
		} else {
			if(link.startingDirection.Equals(Direction.left)){
				xToGo += 1.5f;
			}else if(agent.transform.position.x - link.startLocation.x > -1.5f){
				xToGo -= 1.5f;
			}	
		}
		//Debug.Log ("location : " + agent.transform.position.x  + ", wanted : " + link.startLocation.x + " -- xToGo: " + xToGo);
		return xToGo;
	}

    private static int findHowManyMoveToWayPoint(Plateform from, Plateform beforeFrom, int targetWayPointId) {
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
			if (from.id == link.plateform.id || (beforeFrom != null && beforeFrom.id == link.plateform.id)) {
                continue;
            } else if (link.plateform.waypointId == targetWayPointId) {
                currentDepth--;
                return 1;
            } else {
                int nb = findHowManyMoveToWayPoint(link.plateform, from, targetWayPointId);
                min = Mathf.Min(nb, min);
            }
        }
        currentDepth--;
        return 1 + min;
    }
}
