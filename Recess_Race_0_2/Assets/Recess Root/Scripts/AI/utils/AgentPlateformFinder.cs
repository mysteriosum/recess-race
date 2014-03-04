using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AgentPlateformFinder {

    private static int trys = 0;
    private static int currentDepth;
    private static int maxDepth = 10;

    public static LinkedJumpPlateform generateMove(Agent agent, Plateform plateform) {
        trys = 0;
        LinkedJumpPlateform minPlateform = null;
        int min = maxDepth;
        int targetWayPointId = agent.currentWayPoint + 1;
        List<Plateform> tryiedPlateform = new List<Plateform>();
        foreach (var link in plateform.linkedJumpPlateform) {
            if (tryiedPlateform.Contains(link.plateform)) {
                continue;
            }
            float runningAccelerationThreachhold = 0.9f - 0.8f * (agent.getXSpeed() / agent.getMaxXSpeed());
            if (runningAccelerationThreachhold < 0) runningAccelerationThreachhold = 0.1f; 
            if (Mathf.Abs(link.jumpStart.x - agent.transform.position.x) < runningAccelerationThreachhold) {
                Debug.Log("Jump Tooo close distance " + Mathf.Abs(link.jumpStart.x - agent.transform.position.x) + " - Threshold " + runningAccelerationThreachhold);
                continue;
            }
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
        }
        Debug.Log("From plateform #" + plateform.id + " to wp #" + targetWayPointId + " in " + min + " jumps (trys " + trys + ").");
        if (minPlateform != null) {
            Debug.Log("use plateform #" + minPlateform.plateform.id + " from " + minPlateform.jumpStart.ToString());
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
