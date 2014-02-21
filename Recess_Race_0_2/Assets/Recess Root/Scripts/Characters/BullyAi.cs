using UnityEngine;
using System.Collections;
using System.Linq;

public class BullyAi {

	public static NextJump generateMove(Bully bully, Plateform plateform){
		Plateform to = plateform.linkedPlateform.First ();
		if (Mathf.Abs(to.transform.position.y - bully.transform.position.y) < 1) {
			BullyInstructionConfiguration config = new BullyInstructionConfiguration(LengthEnum.hold, CommandEnum.right, DifficultyEnum.assured);
			NextJump nextJump = new NextJump(config, 5f);
			nextJump.walkDistance = 1;
			nextJump.holdLength = 10;
			return nextJump;
		}

		return null;
	}


}
