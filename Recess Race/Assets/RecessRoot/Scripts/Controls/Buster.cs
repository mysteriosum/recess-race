using UnityEngine;
using System.Collections;

public class Buster : MonoBehaviour {
	
	bool getRun;
	float holdTimer;
	float tier1 = 1.0f;
	float tier2 = 2.5f;
	
	Transform t;
	// Use this for initialization
	void Start () {
		t = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		getRun = Input.GetButton("Run");
		
		
		if (getRun){
			if (holdTimer == 0){
				Shoot(BoogerSize.small);
			}
			
			holdTimer += Time.deltaTime;
			
			
		}
		else{
			if (holdTimer > tier1){
				Shoot(BoogerSize.medium);
			}
			
			if (holdTimer > tier2){
				Shoot(BoogerSize.large);
			}
			
			holdTimer = 0;
		}
	}
	
	void Shoot(BoogerSize size){
		int ind = (int) size;
		string bName = "pre_booger";
		
		switch (size){
		case BoogerSize.small:
			bName += "Small";
			break;
		case BoogerSize.medium:
			bName += "Medium";
			break;
		case BoogerSize.large:
			bName += "Large";
			break;
		}
		Booger boog = Instantiate(Resources.Load(bName), t.position, t.rotation) as Booger;
		
	}
}
