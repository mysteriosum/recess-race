using UnityEngine;
using System.Collections;

public class Ashley : Bully {
	
	
	// Use this for initialization
	void Start () {
		
		if (ashley){
			Destroy(gameObject);
		}
		else{
			ashley = this;
		}
		message = "Eww, don't touch me!";
		
		Setup();
		movement.wSpeed = 1.9f;
		Harrass = AshleyHarrass;
	}
	
	
	
}
