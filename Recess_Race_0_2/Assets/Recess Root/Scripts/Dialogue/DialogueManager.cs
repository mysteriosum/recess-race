using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
	
	
	public TextAsset text;
	
	private Speaker[] speakers;
	
	private bool waiting;
	public class DialogueCommand{
		public string toSay;
		public string speakerName;
		public float showTime;
		public bool isPause;
		
		public DialogueCommand (string toSay, string speakerName, float showTime)
		{
			this.toSay = toSay;
			this.speakerName = speakerName;
			this.showTime = showTime;
			isPause = false;
		}
		public DialogueCommand (float showTime)
		{
			this.showTime = showTime;
			isPause = true;
		}

	}
	
	private List<DialogueCommand> commands = new List<DialogueCommand>();
	private float timePerCharacter = 0.055f;
	
	private int convoIndex = 0;
	
	// Use this for initialization
	void Start () {
		speakers = GetComponentsInChildren<Speaker>(true);
		string[] lines = Textf.ParseScript(text);
		
		foreach (string line in lines) {
			if (line.StartsWith("PAUSE")){
				string length = line.Remove (0, "PAUSE: ".Length);
				commands.Add (new DialogueCommand(float.Parse (length)));
				continue;
			}
			foreach(Speaker speaker in speakers){
				string theirName = speaker.name;
				string formattedName = theirName.ToUpper();
				if (line.StartsWith(formattedName)){
					//string toSay = line.Remove (0, theirName.Length + 2);
					commands.Add (new DialogueCommand(line, theirName, (float)line.Length * timePerCharacter));
				}
			}
		}
		
		ExecuteCommand(commands[0]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ExecuteCommand(DialogueCommand command){
		if (command.isPause){
			Invoke ("NextCommand", command.showTime);
		} else{
			foreach(Speaker speaker in speakers){
				if (speaker.name == command.speakerName){
					speaker.Speak (command.toSay, command.showTime, NextCommand);
				}
			}
		}
	}
	
	public void NextCommand(){
		convoIndex ++;
		if (convoIndex < commands.Count)
			ExecuteCommand(commands[convoIndex]);

	}
}
