using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class AgentPlateformFinder {

	private Agent agent;

	private int minJumpsFound;
    private int trys;
    private int currentDepth;
    private int maxDepth = 10;

	public AgentPlateformFinder(Agent agent){
		this.agent = agent;
	}


	public PreciseJumpConfig generateMove(Plateform fromPlateform) {
		List<PlateformAndNbTry> tryiedPlateform = generateListPlateformAndNbTry (fromPlateform);
		LinksToPlateform choosenPlateform = choosenPlateformWithSkillAndStuff (tryiedPlateform);
		if (choosenPlateform == null || choosenPlateform.jumps.Count == 0) return null;
		return findAPreciseJump (fromPlateform, choosenPlateform.plateform);
    }

	private List<PlateformAndNbTry> generateListPlateformAndNbTry(Plateform fromPlateform){
		this.trys = 0;
		this.currentDepth = 0;
		this.minJumpsFound = maxDepth;

		int targetWayPointId = agent.currentWayPoint + 1;
		List<PlateformAndNbTry> tryiedPlateform = new List<PlateformAndNbTry>();

		foreach (var links in fromPlateform.linksToPlateforms) {
			if(links.plateform.waypointId == targetWayPointId){
				minJumpsFound = 1;
				tryiedPlateform.Clear();
				tryiedPlateform.Add(new PlateformAndNbTry(1,links));
				Debug.Log("On fait un jump direct!");
				return tryiedPlateform;
			}	

			int minForP = 1+ findHowManyMoveToWayPoint(links.plateform, null, targetWayPointId);
			minJumpsFound = Mathf.Min(minForP, minJumpsFound);
			tryiedPlateform.Add(new PlateformAndNbTry(minForP,links));
		}


		return tryiedPlateform;
	}

	private LinksToPlateform choosenPlateformWithSkillAndStuff(List<PlateformAndNbTry> countedPlateforms){
		if (this.minJumpsFound == 1) {
			if (agent.debug)
				Debug.Log("Agent go to wp #" + (agent.currentWayPoint +1) + " by p#" + countedPlateforms[0].links.plateform.id+" in direct jumps (trys " + trys + ").");		
			return countedPlateforms[0].links;
		} else {
			int add = ( UnityEngine.Random.Range(0f,1f) > agent.jumpDecissionSkill) ? 1 : 0;
			int target = this.minJumpsFound + add;
			
			PlateformAndNbTry plateforandNbTry = findPlateformForTargetJump(countedPlateforms, target);
			if(plateforandNbTry != null){
				if (agent.debug)
					Debug.Log("Agent go to wp #" + (agent.currentWayPoint +1) + " by p#" + plateforandNbTry.links.plateform.id+" in " + target + " jumps (trys " + trys + ").");
				return plateforandNbTry.links;
			}else if(target != this.minJumpsFound){
				plateforandNbTry = findPlateformForTargetJump(countedPlateforms, this.minJumpsFound);
				if (agent.debug)
					Debug.Log("Agent go to wp #" + (agent.currentWayPoint +1) + " by p#" + plateforandNbTry.links.plateform.id+" in "+minJumpsFound+"jumps (trys " + trys + ").");
				return plateforandNbTry.links;
			}
		}
		return null;
	}

	private PlateformAndNbTry findPlateformForTargetJump(List<PlateformAndNbTry> tryiedPlateform, int targetJumps){

		List<PlateformAndNbTry> plateformTryesInTagerJumpsCount = new List<PlateformAndNbTry> ();

		foreach (var item in tryiedPlateform) {
			if(item.nbTry == targetJumps){
				plateformTryesInTagerJumpsCount.Add(item);
			}
		}
		if (plateformTryesInTagerJumpsCount.Count == 0) {
			return null;		
		} else {
			int randomIndex = UnityEngine.Random.Range (0, plateformTryesInTagerJumpsCount.Count-1);
			return plateformTryesInTagerJumpsCount[randomIndex];		
		}

	}


	//Random pour linstant
	private PreciseJumpConfig findAPreciseJump(Plateform from, Plateform to){
		LinksToPlateform links = from.getLinksTo (to);
		int randomIndex = UnityEngine.Random.Range (0, links.jumps.Count-1);
		return links.jumps[randomIndex];
	}



	private int findHowManyMoveToWayPoint(Plateform from, Plateform beforeFrom, int targetWayPointId) {
		trys++;
		if (currentDepth++ >= maxDepth) {
			currentDepth--;
			return maxDepth;
		}
		int min = maxDepth;
		foreach (var link in from.linksToPlateforms) {
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

	public static float getXToGetToMakeTheJump(Agent agent, PreciseJumpConfig preciseJump){
		float xToGo = preciseJump.startLocation.x;
		if (agent.transform.position.x > preciseJump.startLocation.x) { // agent on right of target
			if(preciseJump.startingDirection.Equals(Direction.right)){
				xToGo -= 0.5f;
			}else if(agent.transform.position.x - preciseJump.startLocation.x < 0.5f){
				xToGo += 0.5f;
			}
		} else {
			if(preciseJump.startingDirection.Equals(Direction.left)){
				xToGo += 0.5f;
			}else if(agent.transform.position.x - preciseJump.startLocation.x > -0.5f){
				xToGo -= 0.5f;
			}	
		}
		//Debug.Log ("location : " + agent.transform.position.x  + ", wanted : " + link.startLocation.x + " -- xToGo: " + xToGo);
		return xToGo;
	}

	private class PlateformAndNbTry{
		public int nbTry;
		public LinksToPlateform links;

		public PlateformAndNbTry (int nbTry, LinksToPlateform links){
			this.nbTry = nbTry;
			this.links = links;
		}
	}
}
