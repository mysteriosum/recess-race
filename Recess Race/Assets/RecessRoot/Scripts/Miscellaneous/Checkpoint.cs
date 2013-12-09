using UnityEngine;
using System.Collections;

public class Checkpoint : GizmoDad {
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter (Collider other){
		Character charScript = other.gameObject.GetComponent<Character>();
		
		if (charScript){
			charScript.SetCheckpoint(transform);
		}
	}
}
