using UnityEngine;
using System.Collections;

public class Brandon : Bully {
	
	
	
	// Use this for initialization
	void Start () {
		if (brandon){
			Destroy(gameObject);
		}
		else{
			brandon = this;
		}
		
		message = "Get the fuck out of my way, faggot.";
		warningMessage = "You're lucky the teachers are right there.";
		
		Setup();
		movement.wSpeed = 2.0f;
		Harrass = BrandonHarrass;
	}
	
}
