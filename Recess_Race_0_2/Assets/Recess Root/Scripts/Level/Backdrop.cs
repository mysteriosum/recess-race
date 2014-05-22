using UnityEngine;
using System.Collections;

public class Backdrop : MonoBehaviour {
	
	
	Transform cam;
	Transform t;
	
	// Use this for initialization
	void Start () {
		cam = GameManager.gm.transform;
		t = transform;
	}
	
	// Update is called once per frame
	void Update () {
		t.position = new Vector3(cam.position.x, t.position.y, t.position.z);
	}
}
