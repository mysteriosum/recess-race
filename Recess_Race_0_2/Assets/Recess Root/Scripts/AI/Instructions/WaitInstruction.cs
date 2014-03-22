using UnityEngine;
using System.Collections;

[System.Serializable]
public class WaitInstruction : Instruction {

	public float waitTime;
	public float waitedTime = 0;
	public WaitInstruction(Agent agent, float waitTime)
	: base(agent) {
		this.waitTime = waitTime;
	}


	public override void start() {
		waitedTime = 0;
	}
	
	public override void update() {
		waitedTime += Time.deltaTime;
		if (waitedTime >= waitTime) {
			this.isDone = true;		
		}
	}

	public override Direction getStartingDirection (){
		return Direction.right;
	}

    public override string ToString() {
        return "WaitInstruction (waitTime=" + waitTime + ")";
    }
}