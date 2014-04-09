using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AgentPlateformFinder {

    private static int trys = 0;
    private static int currentDepth;
    private static int maxDepth = 10;

    public static LinkedPlateform generateMove(Agent agent, Plateform plateform) {
        trys = 0;

        int min = maxDepth;
		LinkedPlateform plateformWayPoint = null;

        int targetWayPointId = agent.currentWayPoint + 1;
        List<PlateformNbTry> tryiedPlateform = new List<PlateformNbTry>();
        foreach (var link in plateform.linkedJumpPlateform) {
			if (contain(tryiedPlateform, link.plateform)) continue;

			if (link.plateform.waypointId == targetWayPointId) {
				min = 1;
				plateformWayPoint = link;
				break;
			}

			int minForP = findHowManyMoveToWayPoint(link.plateform, null, targetWayPointId);
			if (minForP < min) {
				min = minForP;
				plateformWayPoint = link;
			}
			tryiedPlateform.Add(new PlateformNbTry(minForP,link));
        }


		if (min == 1) {
			if (agent.debug)
				Debug.Log("From p#" + plateform.id + " to wp #" + targetWayPointId + " by p#" + plateformWayPoint.plateform.id+" in direct jumps (trys " + trys + ").");
			return plateformWayPoint;
		} else {

			int target = (int) (min + (1 - agent.jumpDecissionSkill) * UnityEngine.Random.Range (0, 5));
			for (int i = 0; i < 5; i++) {
				for (int j = -1; j <= 1; j+=2) {
					PlateformNbTry plateformTry = getPlateformWithMoveOf(tryiedPlateform,target + j*i);
					if(plateformTry != null){
						if (agent.debug)
							Debug.Log("From p#" + plateform.id + " to wp #" + targetWayPointId + " by p#" + plateformTry.plateform.plateform.id+" in " + min + " jumps (trys " + trys + ").");
						return plateformTry.plateform;
					}
				}
			}
		}
		return null;
    }

	private static bool contain(List<PlateformNbTry> plateforms, Plateform p){
		foreach (var item in plateforms) {
			if(item.plateform.plateform.id == p.id){
				return true;
			}	
		}
		return false;
	}

	private static PlateformNbTry getPlateformWithMoveOf(List<PlateformNbTry> plateforms, int nbMoves){
		foreach (var item in plateforms) {
			if(item.nbTry == nbMoves){
				return item;
			}
		}
		return null;
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

	private class PlateformNbTry{
		public int nbTry;
		public LinkedPlateform plateform;

		public PlateformNbTry (int nbTry, LinkedPlateform plateform){
			this.nbTry = nbTry;
			this.plateform = plateform;
		}
	}
}
