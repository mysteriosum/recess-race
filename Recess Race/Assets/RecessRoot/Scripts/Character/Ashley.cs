using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Ashley : Profile {
	
	private Bully bullyScript;

	private String[] insults = new String[] {
		"Eww! You're so gross!",
		"Oh my god, don't touch me.",
		"You're never going to win.",
		"You should just give up, Boogerboy.",
		"Augh, I don't even want to look at you"
	};
	
	// Use this for initialization
	void Start () {
		base.Start();
		bullyScript = GetComponent<Bully>();
		
		bullyScript.SkillPenalty = 10;
		bullyScript.bullyMethod = BullyFitz;
		
		Chars.ashley = this;
		speakTimer = 2.2f;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
	
	public void BullyFitz (Character fitz){
		if (currentSpeech.Active) return;
		int index = UnityEngine.Random.Range(0, insults.Length);
		
		Speak(insults[index]);
	}
}
