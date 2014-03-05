using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D (Collider2D other){
		Fitz fitz = other.gameObject.GetComponent<Fitz>();
		if (fitz){
			fitz.FinishRace();
			RecessCamera.cam.FinishRace();
			RecessCamera.cam.PlaySound(RecessCamera.cam.sounds.children);
		}
	}
}
