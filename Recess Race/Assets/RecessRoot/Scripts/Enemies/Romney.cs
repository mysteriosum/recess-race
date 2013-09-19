using UnityEngine;
using System.Collections;

public class Romney : Bully {
	
	

	// Use this for initialization
	void Start () {
		
		if (romney){
			Destroy(gameObject);
		}
		else{
			romney = this;
		}
		
		Setup();
		
		message = "Don't fucking touch me!";
		otherMessage = "Booger boy touched me! Help!";
		warningMessage = "Hey, watch it!";
		
		movement.wSpeed = 1.8f;
		
		Harrass = RomneyHarrass;
	}
}
