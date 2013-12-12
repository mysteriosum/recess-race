using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {
	
	private tk2dTextMesh tmesh;
	private tk2dSprite bubble;
	private string targetText = "";
	
	public int[] lineLengths;
	
	private float showLetterTiming = 0.04f;
	private float letterTimer = 0;
	private int currentIndex;
	
	private Profile speaker;
	
	
	public string Text {
		get { return tmesh.text; }
		set {
			currentIndex = 0;
			TMesh.text = "";
			TMesh.Commit ();
			targetText = Textf.GangsterWrap(value, lineLengths);
//			TMesh.text = Textf.GangsterWrap(value, lineLengths);
//			TMesh.Commit();
		}
	}
	
	public bool Active {	
		get { return tmesh.renderer.enabled; }
		set { 
			TMesh.renderer.enabled = value;
			if (!bubble)
				bubble = GetComponentInChildren<tk2dSprite>();
			bubble.renderer.enabled = value;
		}
	}
	
	public tk2dTextMesh TMesh {
		get {
			if (!tmesh)
				tmesh = GetComponentInChildren<tk2dTextMesh>();
			return tmesh;
		}
	}
	
	public int MaxLength {
		get { return TMesh.maxChars; }
	}
	// Use this for initialization
	void Start () {
		tmesh = GetComponentInChildren<tk2dTextMesh>();
		bubble = GetComponentInChildren<tk2dSprite>();
		
	}
		
	// Update is called once per frame
	void Update () {
		if (Active && Text != targetText && targetText.Length > 0){
			letterTimer += Time.deltaTime;
			if (letterTimer > showLetterTiming){
				currentIndex ++;
				TMesh.text = targetText.Substring(0, currentIndex);
				TMesh.Commit ();
				letterTimer = 0;
			}
			if (targetText == Text){
				speaker.ContinueToNextInQueue();
			}
		}
		
	}
	
	public void Flip (){
		bubble.scale = new Vector3(bubble.scale.x * -1, bubble.scale.y, bubble.scale.z);
		//TMesh.transform.localPosition = new Vector3(TMesh.transform.localPosition.x * -1, TMesh.transform.localPosition.y, TMesh.transform.localPosition.z);
		TMesh.transform.position -= new Vector3(Mathf.Abs (bubble.boxCollider.size.x) - TMesh.transform.localPosition.x, 0, 0);
	}
	
	public void SetSpeaker (Profile speaker){
		this.speaker = speaker;
	}
	
}
