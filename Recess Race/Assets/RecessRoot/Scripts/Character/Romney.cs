using UnityEngine;
using System.Collections;

public class Romney : Profile {
	
	private Bully bullyScript;
	
	private float influenceDistance = 360f;
	
	// Use this for initialization
	void Start () {
		base.Start();
		bullyScript = GetComponent<Bully>();
		
		bullyScript.SkillPenalty = 20;
		bullyScript.bullyMethod = BullyFitz;
		
		Chars.romney = this;
		
		speakTimer = 2.2f;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
	
	public void BullyFitz (Character fitz){
		if (currentSpeech.Active) return;
		
		if (Vector3.Distance(transform.position, Chars.brandon.transform.position) < influenceDistance && !bullyScript.BeingWatched){
			fitz.Hurt(gameObject, HurtDuration.Short, Vector2.zero);
			Speak("Get off me, gaylord!");
			Chars.brandon.Speak("Nice one, Romney");
			return;
		}
		if (Vector3.Distance(transform.position, Chars.ashley.transform.position) < influenceDistance){
			Speak("Augh he touched me! Guys don't touch me I'm infected!");
			return;
		}
		
		Speak ("Hey, watch it!");
	}
}
