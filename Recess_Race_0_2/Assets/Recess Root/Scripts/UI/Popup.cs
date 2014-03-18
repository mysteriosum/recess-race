using UnityEngine;
using System.Collections;

public class Popup {

	
	//to be set in a makeshift constructor
	private int maxSize;
	private float timeShown;
	
	//not to be set in a constructor
	private float timeToGetToMaxSize = 0.65f;
	private GameObject go;
	private TextMesh tm;
	private Transform t;
	private float growTimer = 0;
	private float showTimer = 0;
	
	public Popup(string text, Color colour, int maxSize, float timeShown, Vector2 position){
		this.maxSize = maxSize;
		this.timeShown = timeShown;
		
		go = new GameObject("Popup");
		t = go.transform;
		tm = go.AddComponent<TextMesh>();
		
		t.position = position;
		tm.text = text;
		tm.renderer.material.color = colour;
		tm.fontSize = 1;
	}
	
	public void Update(){
		if (growTimer < timeToGetToMaxSize && showTimer < timeShown){
			growTimer += Time.deltaTime;
			tm.fontSize = Mathf.Max(1, (int) Mathf.Lerp(1, maxSize, growTimer/timeToGetToMaxSize));
			
		}
	}
}
