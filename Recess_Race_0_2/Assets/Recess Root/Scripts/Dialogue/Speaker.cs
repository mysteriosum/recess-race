using UnityEngine;
using System.Collections;

public class Speaker : MonoBehaviour {
	
	public SpriteRenderer triangle;
	public SpriteRenderer square;
	public SpriteRenderer speaker;
	public TextMesh tm;
	
	public Vector2 textOffset = new Vector2(0.2f, 0.2f);
	public int textSize = 14;
	public float heightPerLine = 1f;
	public float widthPerCharacter = 0.1f;
	public float defaultScaleX = 8.5f;
	public float extraHeight = 0.55f;
	public float extraWidth = 1.5f;
	public delegate void SpeechDoneDelegate();
	public SpeechDoneDelegate finishEvent = null;
	
	bool active = false;
	bool growing;
	float maxScaleY;
	float maxScaleX;
	
	float showTimer = 0;
	float showTiming;
	float showTimerDefault = 4f;
	
	float scaleX;
	float scaleY;
	
	int modifier = 1;
	public int lineLengths = 40;
	float growthRateMod = 16f;
	
	
	string showString = "";
	
	string dialogue;
	string testDialogue = "This is a wonderful string, really. It's long enough to test my purpose, and also I'm into things like this. So there!";
	
	// Use this for initialization
	void Awake () {
		triangle.renderer.enabled = false;
		square.renderer.enabled = false;
		tm.renderer.enabled = false;
		speaker = transform.parent.GetComponent<SpriteRenderer>();
		//DEV
		Debug.Log ("Disabling renderers from start");
		//Speak (testDialogue, showTimerDefault, SayItAgain);
		maxScaleX = defaultScaleX;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!active) return;
		
		modifier = Camera.main.transform.position.x > transform.position.x? -1 : 1;
		
		if (growing){
			scaleX += Time.deltaTime * growthRateMod;
			scaleY += Time.deltaTime * growthRateMod;
			scaleX = Mathf.Min (scaleX, maxScaleX);
			scaleY = Mathf.Min (scaleY, maxScaleY);
			square.transform.localScale = new Vector3(scaleX, scaleY, 1);
			if (scaleX == maxScaleX && scaleY == maxScaleY){
				growing = false;
			}
		}
		else{
			tm.text = dialogue;
			tm.renderer.enabled = true;
			showTimer += Time.deltaTime;
			tm.transform.position = new Vector3(square.bounds.min.x + textOffset.x, square.bounds.max.y + textOffset.y, -1);
			
			if (showTimer > showTiming){
				scaleX -= Time.deltaTime * growthRateMod;
				scaleY -= Time.deltaTime * growthRateMod;
				scaleX = Mathf.Max (scaleX, 0);
				scaleY = Mathf.Max (scaleY, 0);
				square.transform.localScale = new Vector3(scaleX, scaleY, 1);
				tm.renderer.enabled = false;
				if (scaleX <= 0 || scaleY <= 0){
					active = false;
					triangle.renderer.enabled = false;
					square.renderer.enabled = false;
					Debug.Log ("no more enabling on renderers");
					if (finishEvent != null){
						finishEvent();
						//finishEvent = null;
					}
				}
			}
		}
	}
	
	public void Speak (string speech, float duration, SpeechDoneDelegate delegation){
		finishEvent = delegation;
		int lineAmount;
		int longestLine;
		dialogue = Textf.GangsterWrap(speech, new int[]{ lineLengths }, out lineAmount, out longestLine);
		
		growing = true;	
		scaleX = 1;
		scaleY = 1;
		maxScaleY = (float) lineAmount * heightPerLine + extraHeight;
		maxScaleX = (float) longestLine * widthPerCharacter + extraWidth;
		square.transform.localScale = Vector3.one;
		square.renderer.enabled = true;
		triangle.renderer.enabled = true;
		Debug.Log ("my square's renderer is enabled: " + square.renderer.enabled);
		showTiming = duration;
		showTimer = 0;
		active = true;
//		tm.transform.position = square.transform.position + Vector3.up * (lineAmount-1) + new Vector3(textOffset.x, textOffset.y, -1);
		
	}
	
	public void Speak(string speech, float duration){
		Speak (speech, duration, null);
	}
	
	void SayItAgain (){
		Speak (testDialogue, showTimerDefault, SayItAgain);
	}
}
