using UnityEngine;
using System.Collections;

public class LoadOperator : MonoBehaviour {
	private Transform[] chilluns;
	private RecessManager man;
	private Decoy[] decoys;
	
	
	private static int loaders = 0;
	// Use this for initialization
	
	
	void Start () {
		
		decoys = GetComponentsInChildren<Decoy>();	
		
		if (Application.loadedLevelName != "room_main") return;
		
		man = RecessManager.Instance;
		loaders ++;
		man.AddDecoys(decoys);
		transform.position += new Vector3(0, loaders * man.levelOffset, 0);
		
		if (loaders % 2 == 1){
			transform.localScale = new Vector3(-1, 1, 1);
			BroadcastMessage("Flip", SendMessageOptions.DontRequireReceiver);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
