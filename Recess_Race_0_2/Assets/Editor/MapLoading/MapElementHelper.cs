using UnityEngine;
using System.Collections;

public class MapElementHelper {

	//private GameObject tileHoverPrefab;
	private GameObject bullyInstructionPrefab;


	private static MapElementHelper instance = new MapElementHelper();
	private MapElementHelper(){
		//tileHoverPrefab = Resources.Load<GameObject>("TilePlateformHover");
		bullyInstructionPrefab = Resources.Load<GameObject>("BullyInstruction");

	}

	public static BullyInstruction generateInstructionOnCentered(BullyInstructionConfiguration configuration, Plateform plateform, Transform parent)
	{
		BullyInstruction bullyInstruction = generateInstruction (configuration, plateform, parent);
		
		Bounds bound =  plateform.getBound();
		bullyInstruction.transform.Translate(bound.center.x, bound.center.y, bound.center.z);
		
		return bullyInstruction;
	}

	public static BullyInstruction generateInstructionOnRight(BullyInstructionConfiguration configuration, Plateform plateform, Transform parent)
	{
		BullyInstruction bullyInstruction = generateInstruction (configuration, plateform, parent);
		
		Bounds bound = plateform.getBound ();
		bullyInstruction.transform.Translate(bound.max.x - 0.5f, bound.center.y, bound.center.z);

		return bullyInstruction;
	}

	private static BullyInstruction generateInstruction(BullyInstructionConfiguration configuration, Plateform plateform, Transform parent){
		GameObject newInstruction = (GameObject)GameObject.Instantiate(instance.bullyInstructionPrefab);
		newInstruction.name = "Ins: " + configuration.jumpLength + " " + configuration.moveDirection + " " + configuration.jumpDifficulty;
		newInstruction.transform.parent = parent;

		BullyInstruction bullyInstruction = (BullyInstruction)newInstruction.GetComponent<BullyInstruction>();
		bullyInstruction.setTo(configuration);	
		return bullyInstruction;
	}
}
