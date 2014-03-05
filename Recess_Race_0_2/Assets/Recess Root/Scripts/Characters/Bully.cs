using UnityEngine;
using System.Collections;
using System;

public class Bully : MonoBehaviour {
	public string message = "Get away from me, faggot!";
	
	private string showMessage;
	private float showMessageTiming = 2.5f;
	private float showMessageTimer = 0;
	
	private float messageWidthPercent = 0.8f;
	private float messageHeightPercent = 0.2f;
	private int fontSize = 35;
	
	void Start(){
		showMessage = name + ":" + Environment.NewLine + message;
	}
	
	void Update(){
		if (showMessageTimer > 0){
			showMessageTimer -= Time.deltaTime;
		}
	}
	
	void CollideWithFitz(){
		showMessageTimer = showMessageTiming;
	}
	
	
	void OnGUI(){
		if (showMessageTimer > 0){
			Rect messageRect = 
				new Rect(Screen.width/2 - (Screen.width * (messageWidthPercent)/2), Screen.height - (Screen.height * messageHeightPercent), 
				Screen.width * messageWidthPercent, Screen.height * messageHeightPercent);
			GUIStyle style = new GUIStyle(RecessCamera.cam.hud.skin.box);
			//style.contentOffset = new Vector2(messageRect.width * 0.15f, messageRect.height * 0.15f);
			style.wordWrap = true;
			style.fontSize = fontSize;
			GUI.Box(messageRect, showMessage, style);
		}
	}
}
