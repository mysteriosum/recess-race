using UnityEngine;
using System.Collections;

public class RecessCamera : MonoBehaviour {
	private Transform t;
	private Transform fitz;
	
	public static RecessCamera cam;
	
	// Use this for initialization
	void Awake () {
		if (cam != null){
			Destroy (gameObject);
		}
		else{
			cam = this;
		}
	}
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
