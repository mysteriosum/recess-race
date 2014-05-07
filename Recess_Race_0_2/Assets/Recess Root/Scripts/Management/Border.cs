using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour {
	
	
	public Rect border;

	// Use this for initialization
	void Start () {
		if (RecessCamera.cam != null) {
				RecessCamera.cam.Border = border;		
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
