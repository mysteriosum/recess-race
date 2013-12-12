using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Conversation : MonoBehaviour {
	
	public TextAsset script;
	
	public Profile[] characters;
	
	private String[] charNames;
	
	private List<String> tags = new List<String>();
	private String[] lines;
	
	private int convoIndex = 0;
	private float defaultPause = 2f;
	
	public class TagDefinitions {
		public String pause = "PAUSE: ";
		public String condition = "CONDITION: ";
	}
	
	private TagDefinitions convoTags = new TagDefinitions();
	
	void Start () {
		
		
		lines = Textf.ParseScript(this.script, this.charNames);
		
		List<String> tempNames = new List<String>();
		
		
		foreach (Profile t in characters){
			tempNames.Add (t.name.ToUpper ());
			tags.Add (tempNames[tempNames.Count -1] + ": ");
			
		}
		
		tags.Add ("PAUSE: ");
		tags.Add ("CONDITION: ");
		
		charNames = tempNames.ToArray();
		
		Invoke ("NextLine", defaultPause);
	}
	
	void Update () {
		
	}
	
	public void NextLine(){
		String nowLine = lines[convoIndex];
		String lineTag = "";
		int counter = 0;
		foreach (String t in tags){
			if (nowLine.StartsWith(t)){
				lineTag = t;
				nowLine = nowLine.Remove (0, t.Length);
				if (t == convoTags.pause){
					Debug.Log ("A pause of " + nowLine + " is required");
					convoIndex ++;
					Invoke ("NextLine", float.Parse (nowLine));
					return;	//return so I don't do the rest of this with non-dialogue
				}
				break;		//break the loop to keep the counter value
			}
			counter ++;
		}
//		Debug.Log (nowLine);
		characters[counter].ProcessSpeech (nowLine);
		convoIndex++;
		//Invoke ("NextLine", defaultPause);
	}
}
