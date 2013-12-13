using UnityEngine;
using System.Collections;

public class Brandon : Profile {

	private Bully bullyScript;
	
	// Use this for initialization
	void Start () {
		base.Start();
		bullyScript = GetComponent<Bully>();
		bullyScript.bullyMethod = BullyFitz;
		
		Chars.brandon = this;
		speakTimer = 2.2f;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
	
	public void BullyFitz(Character fitz){
		if (currentSpeech.Active) return;
		
		if (bullyScript.BeingWatched){
			Speak("You're lucky the monitor's right there.");
			return;
		}
		Speak("No one to help you now, faggot");
		fitz.Hurt(gameObject, HurtDuration.Long, Vector2.zero);
		
		
	}
	
}
