using UnityEngine;
using System.Collections;

public class Popup {

	
	//to be set in a makeshift constructor
	private int maxSize;
	private float timeShown;
	
	//not to be set in a constructor
	private float timeToGetToMaxSize = 0.25f;
	private GameObject go;
	private TextMesh tm;
	private Transform t;
	private float growTimer = 0;
	private float showTimer = 0;
	
	public bool IsActive{
		get { return go != null; }
	}
	
	public Popup (){
		
	}
	public void Initiate(string text, Color colour, int maxSize, float timeShown, Vector3 position, float rotation){
		if (go){
			GameObject.Destroy (go);
		}
		growTimer = 0;
		showTimer = 0;
		
		this.maxSize = maxSize;
		this.timeShown = timeShown;
		
		go = GameObject.Instantiate (RecessCamera.cam.hud.normalTextMesh) as GameObject;
		t = go.transform;
		tm = go.GetComponent<TextMesh>();
		
		
		t.parent = RecessCamera.cam.transform;
		t.localPosition = position;
		t.Rotate (Vector3.forward * rotation);
		
		tm.text = text;
		tm.renderer.material.color = colour;
		tm.fontSize = 1;
	}
	
	public void Update(){
		if (growTimer < timeToGetToMaxSize && showTimer < timeShown){
			growTimer += Time.deltaTime;
			tm.fontSize = Mathf.Max(1, (int) Mathf.Lerp(1, maxSize, growTimer/timeToGetToMaxSize));
		} else if (showTimer < timeShown){
			showTimer += Time.deltaTime;
		} else if (growTimer > 0){
			growTimer -= Time.deltaTime;
			tm.fontSize = Mathf.Max(1, (int) Mathf.Lerp(1, maxSize, growTimer/timeToGetToMaxSize));
			
		}else if (growTimer <= 0){
			GameObject.Destroy (go);
			
		}
	}
	
	public void ExtendPopup(){
		if (showTimer > 0 && growTimer >= timeToGetToMaxSize){
			showTimer = 0;
		}
	}
	
}
