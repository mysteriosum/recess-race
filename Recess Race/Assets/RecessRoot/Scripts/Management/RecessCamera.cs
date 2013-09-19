using UnityEngine;
using System.Collections;

public class RecessCamera : MonoBehaviour {
	private Transform t;
	private Transform fitz;
	private Transform box;
	
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
		if (fitzScript != null)
			fitz = fitzScript.transform;
		
		if (TextBox.bubble != null){
			box = TextBox.bubble.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fitz != null){
			t.position = new Vector3(0, fitz.position.y - 240, t.position.z);
		}
		
		if (box != null){
			box.position = t.position + new Vector3(320, 240, 3);
		}
		else if (TextBox.bubble != null){
			box = TextBox.bubble.transform;
		}
	}
}
