using UnityEngine;
using System;
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
	controls,
	reset,
}

public class MainMenu : MonoBehaviour {
	
	public GUISkin skin;
	public Texture2D mainPic;
	public Texture2D credits;
	public Texture2D modeSelect;
	public Texture2D timeTrial;
	public Texture2D grandPrix;
	public Texture2D optionsTex;
	public Texture2D controlsTex;
	
	public Texture2D stopwatchTexture;
	public Texture2D medalTexture;
	public Texture2D returnArrow;
	public Texture2D arrowRight;
	public Texture2D arrowLeft;
	public Texture2D[] levelThumbs;
	public Texture2D goldTexture;
	public Texture2D silverTexture;
	public Texture2D bronzeTexture;
	public Texture2D participationTexture;
	public Texture2D popupTexture;
	
	public Sprite[] logoCards;
	public SpriteRenderer sceneLogoCard;
	private int logoIndex = 0;
	private float fadeTimer = 0;
	private float fadeFor = 0.9f;
	private float showFor = 2.3f;
	private float showTimer = 0;
	
	public Rect playButtonRect;
	public Rect creditsButtonRect;
	public Rect optionsButtonRect;
	public Texture2D buttonBanner;
	
	private MenuEnum currentMenu = MenuEnum.intro;
	public Vector2 mainButtonOffset = Vector2.zero;
	public Vector2 creditsOffset;
	public Vector2 optionsOffset;
	private float selectFontScreenRation = 0.085f;
	public Rect timeTrialRect = new Rect(0.1f, 0.6f, 0.3f, 0.085f);
	public Rect grandPrixRect = new Rect(0.1f, 0.4f, 0.3f, 0.085f);
	
	public Rect levelSelectRect = new Rect(0.15f, 0.25f, 0.3f, 0.2f);
	
	public Rect creditsGroupRect;
	
	public Texture2D hermitLogo;
	public Rect hermitRect;
	public Texture2D temp8Logo;
	public Rect temp8Rect;
	public Texture2D banana;
	public Rect bananaRect;
	public Texture2D actraLogo;
	public Rect actraRect;
	
	public CreditsElement[] creditItems;
	
	private float thumbMarginPercent = 0.085f;
	private float thumbsStartX = 0;
	private float lerpAmount = 0.1f;
	private int currentIndex = 0;
	
	[System.SerializableAttribute]
	public class ControlsVariables{
		public Rect movementRect;
		public Rect titleRect;
		public float leftMargin;
		public float rightMargin;
		public float yTop = 0.1f;
		
		public Vector2 buttonSize = new Vector2(0.1f, 0.1f);
		public Rect waitRect;
		public Rect waitTextRect = new Rect(0.1f,0.1f,0.1f,0.1f);
	}
	
	
	[System.SerializableAttribute]
	public class OptionsVariables{
		public Rect titleRect;
		public float leftMargin = 0.1f;
		public float selectionsTop = 0.1f;
		public float spacing = 0.1f;
		
		public float buttonHeight = 0.1f;
		public float controlsWidth = 0.1f;
		public float resetWidth = 0.1f;
		
	}
	public OptionsVariables options;
	
	public ControlsVariables controls = new ControlsVariables();
	
	//input & stuff
	private bool[] waitingForInput = new bool[2] {false, false };
//	private KeyCode keytoAssign = KeyCode.None;
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
	
	public static float selectSizeMod = 0.15f;
	
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
	
	public static Rect MultiplyRectByScreenDimensions (Rect original){
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
		
		public int gpFontSizeDivisor = 12;
		public float courseStatInterval;
		public float courseStatX;
		public float courseStatBeginY;
		public float statInterval;
		
		public Rect startRect;
	}
	public LevelSelectMenu levelSelectPositions;
	
	
	[System.SerializableAttribute]
	public class CreditsElement{
		public string title;
		public string[] credit;
	}
	private float CreditsMaxSpeed { get { return Screen.height * 0.0004f; } } //fraction of screen height
	private float CreditsAccel{ get { return Screen.height * 0.001f; } }
	private float creditsSpeed;
	private float creditsDelay = 3.5f;
	private float creditsTimer;
	private float creditsOrigin;
	private float CreditsMin { get { return Screen.height * -1.5f; } }
	
	[System.SerializableAttribute]
	public class CreditsVariables{
		public float origin;
		public float betweenCategories;
		public float betweenNames;
		public float afterTitle;
		public float beforeImage;
		
		public Vector2 itemSize;
		public float fontSize;
		public float shadowOffset;
	}
	public CreditsVariables creditsVars;
	void Awake () {
		currentSpeed = goingUp? arrowMax : -arrowMax;
		if (logoCards.Length > 0){
			sceneLogoCard.sprite = logoCards[0];
			sceneLogoCard.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, sceneLogoCard.transform.position.z);	
		}
		else{
			currentMenu = MenuEnum.main;
		}
			
		//set up menus, make sure the menu skin is correct...
	}
	
	void Start (){
		Application.targetFrameRate = 100;
	}
	void Update (){
		
		//TEST change keys
		if (waitingForInput[0]){
			KeyCode code;
			string input = Input.inputString;
			if (input.Length > 1){
				input = input.Remove(1);
			}
			if (input != ""){
				if (input == " "){
					code = KeyCode.Space;
				}
				else{
					try{
						code = (KeyCode) System.Enum.Parse(typeof(KeyCode), input.ToUpper());
					}
					catch(System.ArgumentException){
						code = KeyCode.F;
					}
				}
				PlayerPrefs.SetString("Button" + System.Convert.ToInt32(waitingForInput[1]).ToString(), code.ToString());
				waitingForInput[0] = false;
			}
			
		}
		
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
		
		if (currentMenu == MenuEnum.intro){
			sceneLogoCard.transform.localScale = Vector3.one;
			float yScale = Camera.main.orthographicSize / sceneLogoCard.bounds.extents.y;
			float screenRatio = (float)Screen.width / (float)Screen.height;
			float xScale = Camera.main.orthographicSize * screenRatio / sceneLogoCard.bounds.extents.x;
			sceneLogoCard.transform.localScale = new Vector3(xScale, yScale, 1f);
			
			if (fadeTimer < fadeFor && showTimer <= 0){
				
				fadeTimer += Time.deltaTime;
				float amount = Mathf.Lerp(0, 1f, fadeTimer/fadeFor);
				sceneLogoCard.renderer.material.color = new Color(amount, amount, amount, 1f);
				
			} else if (fadeTimer >= fadeFor && showTimer <= showFor){
				
				showTimer += Time.deltaTime;
				
			} else if (showTimer >= showFor){
				
				fadeTimer -= Time.deltaTime;
				float amount = Mathf.Lerp(0, 1f, fadeTimer/fadeFor);
				sceneLogoCard.renderer.material.color = new Color(amount, amount, amount, 1f);
				
				if (fadeTimer <= 0 && logoIndex < logoCards.Length - 1){
					showTimer = 0;
					logoIndex ++;
					sceneLogoCard.sprite = logoCards[logoIndex];
				} else if (fadeTimer <= 0){
					currentMenu = MenuEnum.main;
				}
			}
			
			if (Input.GetButtonDown ("Jump") || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)){
				if (logoIndex < logoCards.Length-1){
					logoIndex++;
					showTimer = 0;
					fadeTimer = fadeFor;
					sceneLogoCard.renderer.material.color = new Color(1f, 1f, 1f, 1f);
					
					sceneLogoCard.sprite = logoCards[logoIndex];
				}
				else{
					currentMenu = MenuEnum.main;
				}
			}
			if (Input.GetKeyDown(KeyCode.Escape)){
				currentMenu = MenuEnum.main;
			}
		}
		
		if (currentMenu == MenuEnum.credits && creditsOrigin > CreditsMin){
			if (creditsTimer < creditsDelay){
				creditsTimer += Time.deltaTime;
			} else if (creditsSpeed < CreditsMaxSpeed){
				creditsSpeed += Time.deltaTime * CreditsAccel;
			}
			creditsOrigin -= creditsSpeed;
			
		}
	}
	
	// Update is called once per frame
	void OnGUI () {
		//start with intro animation: credits, logos, etc.
		float yValue;
		
		switch(currentMenu){
			#region main
		case MenuEnum.main:
			//render background (animated?! or just one long animated texture that loops?)
			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainPic);
			
			GUI.DrawTexture(MultiplyRectByScreenDimensions(playButtonRect), buttonBanner);
			GUI.DrawTexture(MultiplyRectByScreenDimensions(creditsButtonRect), buttonBanner);
			GUI.DrawTexture(MultiplyRectByScreenDimensions(optionsButtonRect), buttonBanner);
			
			//Texture2D buttonTex = skin.button.normal.background;
			GUIStyle playStyle = new GUIStyle(skin.customStyles[2]);
			float buttonHeight = Screen.height * playButtonRect.height/2;
			float buttonWidth = Screen.width * playButtonRect.width / 2;
			playStyle.fontSize = (int)((float)Screen.height * playButtonRect.height/2);
			GUIStyle creditsStyle = new GUIStyle(skin.customStyles[2]);
			GUIStyle optionsStyle = new GUIStyle(skin.customStyles[2]);
			Rect playRect = SelectionRect(mainButtonOffset.x * Screen.width, Screen.height * mainButtonOffset.y, buttonWidth, buttonHeight, playStyle);
			bool pressPlay = GUI.Button(playRect, mainMenu[0], playStyle);
			if (pressPlay){

//				Application.LoadLevel (firstLevelName);
				//currentMenu = MenuEnum.modeSelect;
				RecessManager.LoadLevelSelect();
				return;
			}
			Rect optionsRect = SelectionRect((mainButtonOffset.x + optionsOffset.x) * Screen.width, (mainButtonOffset.y + optionsOffset.y) * Screen.height, buttonWidth, buttonHeight, optionsStyle);
			bool pressOptions = GUI.Button (optionsRect, mainMenu[1], optionsStyle);
			if (pressOptions){
				currentMenu = MenuEnum.options;
			}
			Rect creditsRect = SelectionRect((mainButtonOffset.x + creditsOffset.x) * Screen.width, (mainButtonOffset.y + creditsOffset.y) * Screen.height, buttonWidth, buttonHeight, creditsStyle);
			bool pressCredits = GUI.Button (creditsRect, mainMenu[2], creditsStyle);
			if (pressCredits){
				currentMenu = MenuEnum.credits;
				creditsOrigin = creditsVars.origin * Screen.height;
				creditsSpeed = 0;
				creditsTimer = 0;
			}
			
			
			
			break;
			#endregion
			#region title
		case MenuEnum.title:
			
			break;
			#endregion
			#region credits
		case MenuEnum.credits: 
			
			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), credits);
			Rect groupRect = MultiplyRectByScreenDimensions(creditsGroupRect);
		GUI.BeginGroup(groupRect);
			float x = groupRect.width / 4f;
			float y = creditsOrigin;
			float w = groupRect.width / 2f;
			float h = creditsVars.fontSize * Screen.height;
			
			GUIStyle nameStyle = new GUIStyle(skin.GetStyle("LevelSelect"));
			nameStyle.fontSize = (int)((float)Screen.height * creditsVars.fontSize);
			nameStyle.alignment = TextAnchor.MiddleCenter;
			
			GUIStyle shadowStyle = new GUIStyle(nameStyle);
			shadowStyle.normal.textColor = Color.black;
			
			foreach (CreditsElement cred in creditItems){
				GUI.TextArea(new Rect(x + h * creditsVars.shadowOffset, y + h * creditsVars.shadowOffset, w, h), cred.title, shadowStyle);
				GUI.TextArea(new Rect(x, y, w, h), cred.title, nameStyle);
				y += Screen.height * creditsVars.afterTitle;
				foreach (string credName in cred.credit) {
					GUI.TextArea(new Rect(x, y, w, h), credName, nameStyle);
					y += Screen.height * creditsVars.betweenNames;
				}
				y += Screen.height * creditsVars.betweenCategories;
			}
			
		GUI.EndGroup();
			break;
			#endregion
			#region options
		case MenuEnum.options:	
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), optionsTex);
			GUIStyle controlsStyle = new GUIStyle(skin.customStyles[2]);
			GUIStyle resetStyle = new GUIStyle(skin.customStyles[2]);
			
			yValue = options.selectionsTop;
			Rect controlsRect = MultiplyRectByScreenDimensions(new Rect(options.leftMargin, yValue, options.controlsWidth, options.buttonHeight));
			controlsRect = SelectionRect(controlsRect, controlsStyle);
			bool controlsPressed = GUI.Button(controlsRect, "Controls", controlsStyle);
			
			if (controlsPressed){
				currentMenu = MenuEnum.controls;
			}
			
			yValue += options.spacing;
			Rect resetRect = MultiplyRectByScreenDimensions(new Rect(options.leftMargin, yValue, options.resetWidth, options.buttonHeight));
			resetRect = SelectionRect(resetRect, resetStyle);
			bool resetPressed = GUI.Button(resetRect, "Reset Data", resetStyle);
			
			if (resetPressed){
				currentMenu = MenuEnum.reset;
			}
			
			break;
		case MenuEnum.controls:
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), controlsTex);
			GUIStyle movementStyle = new GUIStyle(skin.customStyles[1]);
			movementStyle.fontSize = (int)(Screen.height * controls.movementRect.height);
			movementStyle.alignment = TextAnchor.MiddleCenter;
			GUI.TextArea(MultiplyRectByScreenDimensions(controls.movementRect), "Arrows or WASD to move", movementStyle);
			if (waitingForInput[0]){
				Rect waitRect = MultiplyRectByScreenDimensions(controls.waitRect);
				GUI.DrawTexture (waitRect, popupTexture);
				GUIStyle waitStyle = new GUIStyle(skin.customStyles[1]);
				waitStyle.alignment = TextAnchor.MiddleCenter;
				Rect waitTextRect = MultiplyRectByScreenDimensions(controls.waitTextRect);
				GUI.TextArea(waitTextRect, "Press a Key " + Environment.NewLine + "to assign to Button " + (System.Convert.ToInt32(waitingForInput[1])+1).ToString(), waitStyle);
			}
			else{
				GUIStyle[] buttonStyles = new GUIStyle[]{new GUIStyle(skin.customStyles[1]), new GUIStyle(skin.customStyles[1])};
				yValue = controls.yTop;
				for (int i = 0; i < 2; i++) {
					GUI.TextArea(MultiplyRectByScreenDimensions(new Rect(controls.rightMargin, yValue, 0.1f, 0.1f)), PlayerPrefs.GetString("Button" + i.ToString(), i == 0? "Z": "X"), skin.customStyles[1]);
					Rect buttonRect = MultiplyRectByScreenDimensions(new Rect(controls.leftMargin, yValue, controls.buttonSize.x, controls.buttonSize.y));
					buttonRect = SelectionRect(buttonRect, buttonStyles[i]);
					bool buttonPressed = GUI.Button(buttonRect, "Button " + (i+1).ToString(), buttonStyles[i]);
					
					if (buttonPressed){
						waitingForInput = new bool[2]{true, System.Convert.ToBoolean(i)};
					}
					
					yValue += options.spacing;
				}
				
				
				
			}
			
			
			break;
		case MenuEnum.reset:
			
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), optionsTex);
			GUI.DrawTexture (MultiplyRectByScreenDimensions(controls.movementRect), popupTexture);
			
			break;
			#endregion
			#region modeSelect
		case MenuEnum.modeSelect:
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), modeSelect);
			
			GUIStyle trialStyle = new GUIStyle(skin.customStyles[2]);
			trialStyle.fontSize = (int) (Screen.height * selectFontScreenRation);
			GUIStyle prixStyle = new GUIStyle(trialStyle);
			
			
			Rect trialRect = SelectionRect(TimeTrialRect.x, TimeTrialRect.y, TimeTrialRect.width, TimeTrialRect.height, trialStyle);
			bool timeTrialSelect = GUI.Button (trialRect, "Time Trial", trialStyle);
			
			Rect prixRect = SelectionRect(GrandPrixRect.x, GrandPrixRect.y, GrandPrixRect.width, GrandPrixRect.height, prixStyle);
			bool grandPrixSelect = GUI.Button (prixRect, "Grand Prix", prixStyle);
			
			if (timeTrialSelect){
				currentMenu = MenuEnum.timeTrial;
				RecessManager.currentGameMode = GameModes.timeTrial;
			} else if (grandPrixSelect){
				currentMenu = MenuEnum.grandPrix;
				RecessManager.currentGameMode = GameModes.grandPrix;
			}
			
			break;
		case MenuEnum.timeTrial:
			/*
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
			GUIStyle courseStyle = new GUIStyle(levelSelectStyle);
			courseStyle.fontSize = (int)((double)courseStyle.fontSize * 1.4);
			GUI.TextArea(MultiplyRectByScreenDimensions(levelSelectPositions.bestsX, levelSelectPositions.courseY, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
				"Course " + (currentIndex + 1).ToString(), levelSelectStyle);
			
			GUI.TextArea(MultiplyRectByScreenDimensions(levelSelectPositions.bestsX, levelSelectPositions.bestScoreY, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
				"Best Score: " + RecessManager.levelStats[currentIndex].highScore.ToString(), levelSelectStyle);
			GUI.TextArea(MultiplyRectByScreenDimensions(levelSelectPositions.bestsX, levelSelectPositions.bestTimeY, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
				"Best Time: " + Textf.ConvertTimeToString(RecessManager.levelStats[currentIndex].bestTime), levelSelectStyle);
			if (RecessManager.levelStats[currentIndex].HasBronze){
				Texture2D texture = RecessManager.levelStats[currentIndex].HasGold? goldTexture : (RecessManager.levelStats[currentIndex].HasSilver? silverTexture : bronzeTexture);
				
				GUI.DrawTexture(new Rect(levelSelectPositions.medalsOrigin.x * Screen.width, levelSelectPositions.medalsOrigin.y * Screen.height, 
										texture.width * levelSelectPositions.medalsScale, texture.height * levelSelectPositions.medalsScale), texture);
				
			}*/
			break;
		case MenuEnum.grandPrix:
			/*
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), grandPrix);
			GUIStyle startStyle = new GUIStyle(skin.GetStyle("LevelSelect"));
			startStyle.fontSize = (int)((float)Screen.height * levelSelectPositions.startRect.height);
			startStyle.alignment = TextAnchor.MiddleCenter;
			Rect startRect = SelectionRect(MultiplyRectByScreenDimensions(levelSelectPositions.startRect), startStyle);
			bool startPressed = GUI.Button(startRect, "Start!", startStyle);
			if (startPressed){
				RecessManager.LoadLevel(1, GameModes.grandPrix);
			}
			
			GUIStyle gpScreenStyle = new GUIStyle(skin.GetStyle("LevelSelect"));
			gpScreenStyle.fontSize = Screen.height / levelSelectPositions.gpFontSizeDivisor;
			GUIStyle courseGPStyle = new GUIStyle(gpScreenStyle);
			courseGPStyle.fontSize = (int)((double)courseGPStyle.fontSize * 1.4);
			
			for (int i = 0; i < RecessManager.levelStats.Length; i++) {
				float xValue = levelSelectPositions.courseStatX;
				yValue = levelSelectPositions.courseStatBeginY + i * levelSelectPositions.courseStatInterval;
				
				if (RecessManager.levelStats[i].HasParticipated){
					Texture2D texture = participationTexture;
					if (RecessManager.levelStats[currentIndex].HasBronze){
						texture = RecessManager.levelStats[currentIndex].HasGold? goldTexture : (RecessManager.levelStats[currentIndex].HasSilver? silverTexture : bronzeTexture);
					}
					
					GUI.DrawTexture(new Rect(Screen.width*(xValue + levelSelectPositions.bestsWidth), Screen.height * yValue, 
											texture.width * levelSelectPositions.medalsScale, texture.height * levelSelectPositions.medalsScale), texture);
				}
				
				GUI.TextArea(MultiplyRectByScreenDimensions(xValue, yValue, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
					"Course " + (i + 1).ToString(), gpScreenStyle);
				
				yValue += levelSelectPositions.statInterval;
				
				GUI.TextArea(MultiplyRectByScreenDimensions(xValue, yValue, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
					"Best Score: " + RecessManager.levelStats[i].highScore.ToString(), gpScreenStyle);
				
				yValue += levelSelectPositions.statInterval;
				
				GUI.TextArea(MultiplyRectByScreenDimensions(xValue, yValue, levelSelectPositions.bestsWidth, levelSelectPositions.bestsHeight),
					"Best Rank: " + RecessManager.levelStats[i].RankString, gpScreenStyle);
				
			}
			
			
			break;*/
			#endregion
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
			
			if (returnArrowPressed || Input.GetKeyDown (KeyCode.Escape)){
				if (currentMenu == MenuEnum.timeTrial || currentMenu == MenuEnum.grandPrix){
					currentMenu = MenuEnum.modeSelect;
				}
				else if (currentMenu == MenuEnum.controls || currentMenu == MenuEnum.reset){
					currentMenu = MenuEnum.options;
					waitingForInput[0] = false;
				}
				else{
					currentMenu = MenuEnum.main;
				}
			}
		}
	}
	public static Rect SelectionRect(Rect rect, GUIStyle style){
		return SelectionRect (rect.xMin, rect.yMin, rect.width, rect.height, style);
	}
	public static Rect SelectionRect(float left, float top, float width, float height, GUIStyle style){
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
	
	public static Rect SelectionRect(float left, float top, float width, float height){
		return SelectionRect(left, top, width, height, new GUIStyle());
	}
	
}
