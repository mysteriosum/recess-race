using UnityEngine;
using System.Collections;

public class RecessCamera : MonoBehaviour {
	private Transform t;
	private Transform fitz;
	
	// Use this for initialization
	void Start () {
		t = transform;
		Fitz fitzScript = GameObject.FindObjectOfType(typeof(Fitz)) as Fitz;
		fitz = fitzScript.transform;
	}
	
	// Update is called once per frame
	void Update () {
		t.position = new Vector3(0, fitz.position.y - 240, t.position.z);
	}
}
