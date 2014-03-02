using UnityEngine;
using System.Collections;
using System.Linq;

public class AgentPlateformFinder {

    private static int trys = 0;
    private static int currentDepth;
    private static int maxDepth = 3;

    public static Plateform generateMove(Agent agent, Plateform plateform) {
        trys = 0;
        Plateform minPlateform = null;
        int min = maxDepth;
        int targetWayPointId = agent.currentWayPoint + 1;
        foreach (var link in plateform.linkedJumpPlateform) {
            if (link.plateform.waypointId == targetWayPointId) {
                min = 1;
                minPlateform = link.plateform;
                break;
            }
            int minForP = findHowManyMoveToWayPoint(link.plateform, targetWayPointId);
            if (minForP < min) {
                min = minForP;
                minPlateform = link.plateform;
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
        foreach (var link in from.linkedJumpPlateform) {
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
