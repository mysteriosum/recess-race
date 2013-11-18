using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {
	
	private tk2dTextMesh tmesh;
	private tk2dSprite bubble;
	
	public int[] lineLengths;
	
	public string Text {
		get { return tmesh.text; }
		set {
			if (!tmesh)
				tmesh = GetComponentInChildren<tk2dTextMesh>();
			tmesh.text = Textf.GangsterWrap(value, lineLengths);
			tmesh.Commit();
		}
	}
	
	public bool Active {
		get { return tmesh.renderer.enabled; }
		set { 
			if (!tmesh)
				tmesh = GetComponentInChildren<tk2dTextMesh>();
			tmesh.renderer.enabled = value;
			if (!bubble)
				bubble = GetComponentInChildren<tk2dSprite>();
			bubble.renderer.enabled = value;
		}
	}
	// Use this for initialization
	void Start () {
		tmesh = GetComponentInChildren<tk2dTextMesh>();
		
		bubble = GetComponentInChildren<tk2dSprite>();
		
	}
		
	// Update is called once per frame
	void Update () {
		
	}
	
}
