using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
	
	public string[] messages;
	private float showMessageTiming = 2.5f;
	
	private Speaker speaker;
	private static bool speaking = false;
	private int currentIndex = 0;
	public bool random = false;
	
	protected virtual void Start(){
		speaker = GetComponent<Speaker>();
	}
	
	protected virtual void Update () {
		
	}
	
	
	protected void Done(){
		speaking = false;
	}
	
	protected void CollideWithFitz(){
		if (speaking) return;
		if (!random) currentIndex ++;
		int i = random? Random.Range (0, messages.Length - 1) : currentIndex;
		Speak(i);
		speaking = true;
	}
	
	protected void Speak (int index) {
		speaker.Speak (messages[index], showMessageTiming);
	}
}
