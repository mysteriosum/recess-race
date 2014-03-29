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
	
	public Texture2D stopwatchTexture;
	public Texture2D medalTexture;
	public Texture2D returnArrow;
	public Texture2D arrowRight;
	public Texture2D arrowLeft;
	public Texture2D[] levelThumbs;
	public Texture2D goldTexture;
	public Texture2D silverTexture;
	public Texture2D bronzeTexture;
	
	
	private MenuEnum currentMenu = MenuEnum.main;
	public Vector2 mainButtonOffset = Vector2.zero;
	
	private float selectFontScreenRation = 0.085f;
	public Rect timeTrialRect = new Rect(0.1f, 0.6f, 0.3f, 0.085f);
	public Rect grandPrixRect = new Rect(0.1f, 0.4f, 0.3f, 0.085f);
	
	public Rect levelSelectRect = new Rect(0.15f, 0.25f, 0.3f, 0.2f);
	
	public float stopwatchX = 0.6f;
	public float grandPrixMedalX = 0.3f;
	public float modeSelectIconsY = 0.5f;
	public float stopwatchScale = 1f;
	public float grandPrixMedalScale = 1f;
	
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
	
	public static RectOffset SelectRectOffset {
		get{
			int margin = (int) (Screen.height * 0.04f);
			return new RectOffset(margin, margin, margin, margin);
		}
	}
	
	public static float selectSizeMod = 0.2f;
	
	private float DefaultMargin{
		get{ return LevelSelectRect.width/2 - ThumbWidth/2; }
	}
	public Vector2 returnArrowOrigin = new Vector2(0, 0.9f);
	public float returnArrowScale = 0.2f;
	public float arrow1X = 0.1f;
	public float arrow2X = 0.4f;
	public float arrowY = 0.5f;
	public float arrowScale = 0.075f;
	public float ArrowHeight{
		get{ return Screen.height * arrowScale; }
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
	
	public Rect MultiplyRectByScreenDimensions (float left, float top, float width, float height){
		return MultiplyRectByScreenDimensions(new Rect(left, top, width, height));
	}
	
	public GUIContent[] mainMenu = new GUIContent[]{ new GUIContent("Play"), new GUIContent("Credits")};
	
	public string firstLevelName = "Level1_v_02";
	// Use this for initialization
	[System.SerializableAttribute]
	public class LevelSelectMenu{
		public float bestsX = 0.6f;
		public float courseY = 0.39f;
		public float bestScoreY = 0.49f;
		public float bestTimeY = 0.59f;
		public float bestsWidth = 0.2f;
		public float bestsHeight = 0.075f;
		public Vector2 medalsOrigin = new Vector2(0.62f, 0.62f);
		public float goldY = 0.73f;
		public float medalInterval = 0.1f;
		public float medalsScale = 0.25f;
	}
	public LevelSelectMenu levelSelectPositions;
	
	void Start () {
		currentSpeed = goingUp? arrowMax : -arrowMax;
		
		
		
		//set up menus, make sure the menu skin is correct...
	}
	
	void Update (){
		if (currentMenu == MenuEnum.timeTrial || currentMenu == MenuEnum.grandPrix){
			int directionModifier = goingUp? 1 : -1;
			
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
		case MenuEnum.main:
			//render background (animated?! or just one long animated texture that loops?)
			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainPic);
			
			//Texture2D buttonTex = skin.button.normal.background;
			GUIStyle playStyle = new GUIStyle(skin.customStyles[2]);
			float buttonHeight = Screen.height * 0.085f;
			float buttonWidth = Screen.width * 0.235f;
			Rect playRect = SelectionRect(mainButtonOffset.x * Screen.width, Screen.height * mainButtonOffset.y, buttonWidth, buttonHeight, playStyle);
			bool pressPlay = GUI.Button(playRect, mainMenu[0], playStyle);
			if (pressPlay){

//				Application.LoadLevel (firstLevelName);
				currentMenu = MenuEnum.modeSelect;
			}
			GUIStyle creditsStyle = new GUIStyle(skin.customStyles[2]);
			Rect creditsRect = SelectionRect(mainButtonOffset.x * Screen.width, (mainButtonOffset.y + 0.15f) * Screen.height, buttonWidth, buttonHeight, creditsStyle);
			bool pressCredits = GUI.Button (creditsRect, mainMenu[1], creditsStyle);
			if (pressCredits){
				currentMenu = MenuEnum.credits;
			}
			break;
		case MenuEnum.title:
			
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
			
			GUIStyle trialStyle = new GUIStyle(skin.customStyles[2]);
			trialStyle.fontSize = (int) (Screen.height * selectFontScreenRation);
			GUIStyle prixStyle = new GUIStyle(trialStyle);
			
			
			Rect trialRect = SelectionRect(TimeTrialRect.x, TimeTrialRect.y, TimeTrialRect.width, TimeTrialRect.height, trialStyle);
			bool timeTrialSelect = GUI.Button (TimeTrialRect, "Time Trial", trialStyle);
			
			Rect prixRect = SelectionRect(GrandPrixRect.x, GrandPrixRect.y, GrandPrixRect.width, GrandPrixRect.height, prixStyle);
			bool grandPrixSelect = GUI.Button (GrandPrixRect, "Grand Prix", prixStyle);
			
			if (timeTrialSelect){
				currentMenu = MenuEnum.timeTrial;
			} else if (grandPrixSelect){
				RecessManager.LoadLevel(1, GameModes.grandPrix);
			}
			
			GUI.DrawTexture(new Rect(grandPrixMedalX * Screen.width, modeSelectIconsY * Screen.height, medalTexture.width * grandPrixMedalScale, medalTexture.height * grandPrixMedalScale), medalTexture);
			GUI.DrawTexture(new Rect(stopwatchX * Screen.width, modeSelectIconsY * Screen.height, stopwatchTexture.width * stopwatchScale, stopwatchTexture.height * stopwatchScale), stopwatchTexture);
			
			break;
		case MenuEnum.timeTrial:
			
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), timeTrial);
			
			Rect arrowRect1 = SelectionRect(arrow1X * Screen.width, arrowY * Screen.height, ArrowHeight, ArrowHeight);
			if (!arrowRect1.Contains(new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y))){
				arrowRect1 = new Rect(arrowRect1.x, arrowRect1.y + arrowOscillator, arrowRect1.width, arrowRect1.height);
			}
			
			Rect arrowRect2 = SelectionRect(arrow2X * Screen.width, arrowY * Screen.height, ArrowHeight, ArrowHeight);
			if (!arrowRect2.Contains(new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y))){
				arrowRect2 = new Rect(arrowRect2.x, arrowRect2.y + arrowOscillator, arrowRect2.width, arrowRect2.height);
			}
		
			bool arrow1 = GUI.Button(arrowRect1, arrowLeft, skin.button);
			bool arrow2 = GUI.Button (arrowRect2, arrowRight, skin.button);
			
			if (arrow1){
				currentIndex = Mathf.Max (0, currentIndex - 1);
			} else if (arrow2){
				currentIndex = Mathf.Min (levelThumbs.Length - 1, currentIndex + 1);
			}
			
		GUI.BeginGroup (LevelSelectRect);
			for (int i = 0; i < levelThumbs.Length; i++) {
				float offset = i * (ThumbWidth + ThumbWidth * thumbMarginPercent);
				GUI.DrawTexture (new Rect(thumbsStartX + offset,0, ThumbWidth, LevelSelectRect.height), levelThumbs[i]);
				
				
			}
		GUI.EndGroup ();
			
			GUIStyle goStyle = new GUIStyle(skin.GetStyle("LevelSelect"));
			goStyle.alignment = TextAnchor.MiddleCenter;
			goStyle.fontSize = (int)(LevelSelectRect.height * 0.2f);
			float goWidth = LevelSelectRect.width * 0.3f;
			float goHeight= LevelSelectRect.height * 0.2f;
			
			Rect goRect = SelectionRect(LevelSelectRect.xMin + LevelSelectRect.width/2 - goWidth/2, LevelSelectRect.yMin + LevelSelectRect.height - goHeight, goWidth, goHeight, goStyle);
			bool goPressed = GUI.Button(goRect, "GO!", goStyle);
			
			if (goPressed){
				RecessManager.LoadLevel(currentIndex + 1, GameModes.timeTrial);
			}
			
			
			GUIStyle levelSelectStyle = new GUIStyle(skin.GetStyle("LevelSelect"));
			
			GUI.TextArea(MultiplyRectByScreenDimensions(levelSelectPositions.bestsX, levelSelectPositions.courseY, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
				"Course " + (currentIndex + 1).ToString(), levelSelectStyle);
			
			GUI.TextArea(MultiplyRectByScreenDimensions(levelSelectPositions.bestsX, levelSelectPositions.bestScoreY, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
				"Best Score: " + RecessManager.levelStats[currentIndex].bestScore.ToString(), levelSelectStyle);
			GUI.TextArea(MultiplyRectByScreenDimensions(levelSelectPositions.bestsX, levelSelectPositions.bestTimeY, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
				"Best Time: " + Textf.ConvertTimeToString(RecessManager.levelStats[currentIndex].bestTime), levelSelectStyle);
			if (RecessManager.levelStats[currentIndex].HasBronze){
				Texture2D texture = RecessManager.levelStats[currentIndex].HasGold? goldTexture : (RecessManager.levelStats[currentIndex].HasSilver? silverTexture : bronzeTexture);
				
				GUI.DrawTexture(new Rect(levelSelectPositions.medalsOrigin.x * Screen.width, levelSelectPositions.medalsOrigin.y * Screen.height, 
										texture.width * levelSelectPositions.medalsScale, texture.height * levelSelectPositions.medalsScale), texture);
				
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
		
		
		
		//draw a back arrow
		bool returnArrowPressed = false;
		if (currentMenu != MenuEnum.main && currentMenu != MenuEnum.intro && currentMenu != MenuEnum.title){
			GUIStyle rectStyle = new GUIStyle(skin.button);
			Rect returnRect = SelectionRect(returnArrowOrigin.x * Screen.width, Screen.height * returnArrowOrigin.y, 
											returnArrow.height * returnArrowScale, returnArrow.width * returnArrowScale);
			
			returnArrowPressed = GUI.Button(returnRect, returnArrow, rectStyle);
			
			if (returnArrowPressed){
				if (currentMenu == MenuEnum.timeTrial || currentMenu == MenuEnum.grandPrix){
					currentMenu = MenuEnum.modeSelect;
				}
				else{
					currentMenu = MenuEnum.main;
				}
			}
		}
	}
	
	public Rect SelectionRect(float left, float top, float width, float height, GUIStyle style){
		Rect rect = new Rect(left, top, width, height);
		
		if (rect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))){
			if (Input.GetMouseButton(0)){
				rect = new Rect(left + width * selectSizeMod/2, top + height * selectSizeMod/2, width * (1 - selectSizeMod), height * (1 - selectSizeMod));
				style.fontSize = (int)(style.fontSize * (1f - selectSizeMod));
			}
			else{
				rect = new Rect(left - width * selectSizeMod/2, top - height * selectSizeMod/2, width * (1 + selectSizeMod), height * (1 + selectSizeMod));
				style.fontSize = (int)(style.fontSize * (1f + selectSizeMod));
			}
		}
		
		return rect;
	}
	
	public Rect SelectionRect(float left, float top, float width, float height){
		return SelectionRect(left, top, width, height, new GUIStyle());
	}
	
}
