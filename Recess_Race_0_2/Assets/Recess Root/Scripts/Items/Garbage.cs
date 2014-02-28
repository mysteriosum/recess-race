using UnityEngine;
using System.Collections;

public class Garbage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other){
		Fitz fitz = other.GetComponent<Fitz>();
		if (fitz != null)
			RecessManager.AddGarbageToScore();
	
		Destroy(gameObject);
	}
}
