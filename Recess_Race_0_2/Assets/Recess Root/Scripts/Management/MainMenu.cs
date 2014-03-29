using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenuEnum{
	intro,
	title,
	main,
	credits,
	options,
	modeSelect,
	grandPrix,
	timeTrial,
}

public class MainMenu : MonoBehaviour {
	
	public GUISkin skin;
	public Texture2D mainPic;
	public Texture2D credits;
	public Texture2D modeSelect;
	public Texture2D timeTrial;
	public Texture2D grandPrix;
	
	public Texture2D arrowRight;
	public Texture2D arrowLeft;
	public Texture2D[] levelThumbs;
	private MenuEnum currentMenu = MenuEnum.title;
	public Vector2 mainButtonOffset = Vector2.zero;
	
	private float selectFontScreenRation = 0.085f;
	public Rect timeTrialRect = new Rect(0.1f, 0.6f, 0.3f, 0.085f);
	public Rect grandPrixRect = new Rect(0.1f, 0.4f, 0.3f, 0.085f);
	
	public Rect levelSelectRect = new Rect(0.15f, 0.25f, 0.3f, 0.2f);
	
	private float thumbMarginPercent = 0.085f;
	private float thumbsStartX = 0;
	private float lerpAmount = 0.1f;
	private int currentIndex = 0;
	
	private float ThumbsStartAt{
		get{
			int mod = -currentIndex;
			float result = (ThumbWidth + (ThumbWidth * thumbMarginPercent)) * mod + DefaultMargin;
			return result;
		}
	}
	
	private float DefaultMargin{
		get{ return LevelSelectRect.width/2 - ThumbWidth/2; }
	}
	
	public float arrow1X = 0.1f;
	public float arrow2X = 0.4f;
	public float arrowY = 0.5f;
	public float ArrowHeight{
		get{ return Screen.height * 0.075f; }
	}
	
	private float arrowOscillator = 0;
	private float currentSpeed = 0;
	private float arrowMax = 0.33f;
	private float accelAmount = 1.2f;
	private bool goingUp = false;
	
	private float ThumbWidth {
		get{ return LevelSelectRect.height * levelThumbs[0].width / levelThumbs[0].height; }
	}
	
	public Rect TimeTrialRect {
		get{
			return MultiplyRectByScreenDimensions(timeTrialRect);
		}
	}
	public Rect GrandPrixRect {
		get{
			return MultiplyRectByScreenDimensions(grandPrixRect);
		}
	}
	
	public Rect LevelSelectRect {
		get{ return MultiplyRectByScreenDimensions(levelSelectRect); }
	}
	
	public Rect MultiplyRectByScreenDimensions (Rect original){
		return new Rect(original.x * Screen.width, original.y * Screen.height, original.width * Screen.width, original.height * Screen.height);
	}
	
	public GUIContent[] mainMenu = new GUIContent[]{ new GUIContent("Play"), new GUIContent("Credits")};
	
	public string firstLevelName = "Level1_v_02";
	// Use this for initialization
	void Start () {
		currentSpeed = goingUp? arrowMax : -arrowMax;
		
		
		
		//set up menus, make sure the menu skin is correct...
	}
	
	void Update (){
		if (currentMenu == MenuEnum.timeTrial || currentMenu == MenuEnum.grandPrix){
			//int directionModifier = goingUp? 1 : -1;
			
			currentSpeed += Time.deltaTime * accelAmount * (goingUp? 1 : -1);
			currentSpeed = Mathf.Clamp (currentSpeed, -arrowMax, arrowMax);
			arrowOscillator += currentSpeed;
			if ((arrowOscillator < -arrowMax && !goingUp) || (goingUp && arrowOscillator > arrowMax)){
				goingUp = !goingUp;
			}
		}
		
		if (thumbsStartX != ThumbsStartAt){
			thumbsStartX = Mathf.Lerp (thumbsStartX, ThumbsStartAt, lerpAmount);
		}
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
			
			//Texture2D buttonTex = skin.button.normal.background;
			
			Rect playRect = new Rect(mainButtonOffset.x * Screen.width, Screen.height * mainButtonOffset.y, 200, 100);
			bool pressPlay = GUI.Button(playRect, mainMenu[0], skin.customStyles[2]);
			if (pressPlay){

//				Application.LoadLevel (firstLevelName);
				currentMenu = MenuEnum.modeSelect;
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
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), modeSelect);
			GUIStyle modeStyle = new GUIStyle(skin.textArea);
			modeStyle.fontSize = (int) (Screen.height * selectFontScreenRation);
			bool timeTrialSelect = GUI.Button (TimeTrialRect, "Time Trial", modeStyle);
			bool grandPrixSelect = GUI.Button (GrandPrixRect, "Grand Prix", modeStyle);
			
			if (timeTrialSelect){
				currentMenu = MenuEnum.timeTrial;
			} else if (grandPrixSelect){
				Application.LoadLevel (firstLevelName);
			}
			
			break;
		case MenuEnum.timeTrial:
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), timeTrial);
			
			Rect arrowRect1 = new Rect(arrow1X * Screen.width, arrowY * Screen.height, ArrowHeight, ArrowHeight);
			if (!arrowRect1.Contains(new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y))){
				arrowRect1 = new Rect(arrowRect1.x, arrowRect1.y + arrowOscillator, arrowRect1.width, arrowRect1.height);
			}
			
			Rect arrowRect2 = new Rect(arrow2X * Screen.width, arrowY * Screen.height, ArrowHeight, ArrowHeight);
			if (!arrowRect2.Contains(new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y))){
				arrowRect2 = new Rect(arrowRect2.x, arrowRect2.y + arrowOscillator, arrowRect2.width, arrowRect2.height);
			}
		
		GUI.BeginGroup (LevelSelectRect);
			for (int i = 0; i < levelThumbs.Length; i++) {
				float offset = i * (ThumbWidth + ThumbWidth * thumbMarginPercent);
				GUI.DrawTexture (new Rect(thumbsStartX + offset,0, ThumbWidth, LevelSelectRect.height), levelThumbs[i]);
			}
		
		GUI.EndGroup ();
		
			bool arrow1 = GUI.Button(arrowRect1, arrowLeft, skin.button);
			bool arrow2 = GUI.Button (arrowRect2, arrowRight, skin.button);
			
			if (arrow1){
				currentIndex = Mathf.Max (0, currentIndex - 1);
				Debug.Log ("New index is " + currentIndex);
			} else if (arrow2){
				currentIndex = Mathf.Min (levelThumbs.Length - 1, currentIndex + 1);
				Debug.Log ("New index is " + currentIndex);
			}
			break;
		case MenuEnum.grandPrix:
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), grandPrix);
			
			break;
		default:
			Debug.LogError ("There's something wrong with this situation. Time to bail!");
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
