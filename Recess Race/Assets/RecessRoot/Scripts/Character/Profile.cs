using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Profile : MonoBehaviour {
	
	public static List<Profile> allCharacters = new List<Profile>();
	
	public bool rightHandBubble = true;
	private Vector3 offsetVector = new Vector3(16, 24, -20);
	protected UnityEngine.Object speechObject;
	protected SpeechBubble currentSpeech;
	protected Conversation myConvo;
	
	protected float speakTimer = 1f;
	public float SpeakTimer {
		get { return speakTimer; }
		set { speakTimer = value; }
	}
	
	private int maxBubbleLength;
	
	
	protected String[] queue;
	protected int currentIndex = 0;
	
	private int bubbleLeeway = 5;
	
	public virtual string Cheer {
		get {
			int n = cheers.Length;
			int max = n * (n + 1) / 2;
			int roll = UnityEngine.Random.Range(0, max);
			int index = (int) (Mathf.Sqrt(8 * roll + 1) - 1) /2;
			return cheers[index];
		}
	}
	
	protected String[] cheers = new String[] {
		"You can do it!",
		"Go go go!",
		"Go Brandon!",
		"Go Ashley!",
		"Go Romney!",
		"Don't let Fitz win!",
		"Don't give up!",
		"I believe in you!",
		"Don't forget the tennis balls!",
	};
	
	
	
	// Use this for initialization
	protected void Start () {
		speechObject = Resources.Load ("res_speechBubble");
		GameObject spb = Instantiate (speechObject, transform.position, transform.rotation) as GameObject;
		currentSpeech = spb.GetComponent<SpeechBubble>();
		currentSpeech.Active = false;
		currentSpeech.SetSpeaker (this);
		
		if (!rightHandBubble){
			currentSpeech.Flip ();
			offsetVector = new Vector3(offsetVector.x * -1, offsetVector.y, offsetVector.z);
		}
		
		currentSpeech.transform.position += offsetVector;
		currentSpeech.transform.parent = transform;
		
		if (transform.parent){
			myConvo = transform.parent.GetComponent<Conversation>();
		}
		
	}
	
	// Update is called once per frame
	protected void Update () {
		
	}
	
	public void ProcessSpeech(String fullLine){
		if (!myConvo){
			Debug.LogWarning ("You're trying to start a conversation with someone who is not the child of a conversation!");
			return;
		}
		
		int max = currentSpeech.MaxLength - bubbleLeeway;
		String brokenLine = Textf.SplitIntoComponents(fullLine, max);
		queue = brokenLine.Split (new char[] { '*' });
		Debug.Log ("My queue length is " + queue.Length + " and the first line is " + queue[0].Length);
		currentIndex = 0;
		Speak ();
	}
	
	
	public virtual void Speak (){
		
		currentSpeech.Active = true;
		Debug.Log ("what I'll show now: " + queue[currentIndex]);
		currentSpeech.Text = queue[currentIndex];
//		currentSpeech.Text = "this is bullshit";
		currentIndex++;
		
	}
	
	public virtual void Speak (String speech){
		queue = new String[] { speech };
		currentIndex = 0;
		Speak();
	}
	
	public virtual void Speak (String[] queue){
		this.queue = queue;
		currentIndex = 0;
		Speak();
	}
	
	public virtual void ContinueToNextInQueue(){
		
		if (currentIndex < queue.Length){
			
			Invoke ("Speak", speakTimer);
		}
		else{
			Debug.Log ("Going to the next line now because my index is " + currentIndex );
			Invoke ("DeactivateSpeechObject", speakTimer);
			if (myConvo != null)
				myConvo.Invoke ("NextLine", speakTimer);
		}
	}
	
	private void DeactivateSpeechObject (){
		currentSpeech.Active = false;
	}
	
	
}
public class Chars {
	//main character
	public static Fitz fitz;
	//other racers
	public static Brandon brandon;
	public static Ashley ashley;
	public static Romney romney;
	
	//monitors
	public static Hill hill;
	public static Carol carol;
	public static Jane jane;
	public static Phyllis phyllis;
	
}