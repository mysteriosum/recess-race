using UnityEngine;
using System.Collections;

public class TextBox : MonoBehaviour {
	
	private tk2dTextMesh textMesh;
	private string text;
	private int counter = 0;
	private bool showing = false;
	
	private string speaker;
	private bool finishedShowing;
	private float closeTiming = 2.4f;
	private bool letterTimer;
	private readonly bool letterTiming;
	
	private Vector3 nameOffset = new Vector3(-200, 73, -10);
	private Vector3 speechOffset = new Vector3(-200, 58, -10);
	
	public static TextBox bubble;
	
	
	// Use this for initialization
	void Start () {
		if (bubble){
			Destroy(gameObject);
		}
		else {
			bubble = this;
		}
		
		renderer.enabled = false;
		textMesh = GetComponentInChildren<tk2dTextMesh>();
		textMesh.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (showing && !finishedShowing){
			textMesh.text += text[counter];
			textMesh.Commit();
			counter ++;
			if (counter == text.Length){
				finishedShowing = true;
				Invoke("Close", closeTiming);
			}
		}
	}
	
	public void Open (string speaker, string text) {
		if (showing) return;
		renderer.enabled = true;
		textMesh.renderer.enabled = true;
		this.text = text;
		this.speaker = speaker;
		textMesh.text = speaker + "\n";
		showing = true;
		textMesh.maxChars = text.Length + speaker.Length + 1;
		textMesh.Commit();
	}
	
	void Close () {
		finishedShowing = false;
		showing = false;
		renderer.enabled = false;
		counter = 0;
		textMesh.renderer.enabled = false;
		textMesh.text = "";
		textMesh.Commit();
	}
}
