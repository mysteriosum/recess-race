﻿using UnityEngine;
using System.Collections;

public enum MenuEnum{
	intro,
	title,
	main,
	credits,
	options,
	modeSelect,
	levelSelect,
}

public class MainMenu : MonoBehaviour {
	
	public GUISkin skin;
	public Texture2D mainPic;
	public Texture2D credits;
	
	private MenuEnum currentMenu = MenuEnum.title;
	public Vector2 mainButtonOffset = Vector2.zero;
	
	public GUIContent[] mainMenu = new GUIContent[]{ new GUIContent("Play"), new GUIContent("Credits")};
	
	public string firstLevelName = "Level1_v_02";
	// Use this for initialization
	void Start () {
		
		//set up menus, make sure the menu skin is correct...
	}
	
	
	// Update is called once per frame
	void OnGUI () {
		//start with intro animation: credits, logos, etc.
		switch(currentMenu){
		case MenuEnum.intro:
			//play intro animation!
			
			if (Input.GetButtonDown ("Jump")){
				currentMenu ++;
			}
			break;
		case MenuEnum.title:
			//render background (animated?! or just one long animated texture that loops?)
			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainPic);
			
			Texture2D buttonTex = skin.button.normal.background;
			
			Rect playRect = new Rect(mainButtonOffset.x * Screen.width, Screen.height * mainButtonOffset.y, 200, 100);
			bool pressPlay = GUI.Button(playRect, mainMenu[0], skin.customStyles[2]);
			if (pressPlay){
				Application.LoadLevel (firstLevelName);
			}
			
			Rect creditsRect = new RectOffset(0, 0, -150, 100).Add (playRect);
			bool pressCredits = GUI.Button (creditsRect, mainMenu[1], skin.customStyles[2]);
			if (pressCredits){
				currentMenu = MenuEnum.credits;
			}
			break;
		case MenuEnum.main:
			
			break;
		case MenuEnum.credits:
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainPic);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), credits);
			
			if (Input.GetKeyDown (KeyCode.Escape) || Input.GetMouseButtonDown(0)){
				currentMenu = MenuEnum.title;
			}
			
			break;
		case MenuEnum.options:
			
			break;
		case MenuEnum.modeSelect:
			
			break;
		case MenuEnum.levelSelect:
			
			break;
		default:
			Debug.LogError ("There's something wrong with this situation");
			Destroy (this.gameObject);
			break;
		}
		//then go to the main title screen ("Press Start!")
		
		
		//then the main menu:
				//Play
		
				//Options
		
		
		//Option screen:
				//sound 
		
				//resolution?
		
		
		//Mode select
				//Grand Prix (starts race from beginning and runs through all of them. Will have to communicate with the Manager to indicate that this is the case.
					//genre, "Manager.mode = Modes.GrandPrix"
		
				//Time Trials (the only way to practice the race with no one around. There will also be no conversations and no bullies.)
					//"Manager.mode = Mode.TimeTrials" (indicates we should get rid of the other racers, the monitors, and add the timer)
					
		//Level select
				//only for time trials. Show best times (get from Manager). 
		
		
	}
	
}
