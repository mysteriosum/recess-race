using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RecessCamera : MonoBehaviour {
	private Transform trans;
	private Transform fitzNode;
	private Transform box;

	public AudioSource audioSource;
	public Sounds sounds;
	
	public static RecessCamera cam;
	const int scrnHeight = 480;
	const int scrnWidth = 640;
	private Rect border;
	private Rect effectiveBorder;
	public Rect Border {
		get{ return border; }
		set{ border = value; }
	}
	
	
	// parallax members
    [HideInInspector]
    public List<Transform> parallaxes;
    [HideInInspector]
    public float furthestParalaxZ;
	private float farDistance;
	
	private float lerpAmount = 0.1f;
	private float maxParallax = 0.3f;
	
	
	//race variables
	
	private bool raceBegun = false;
	private float readyTime = 1.0f;
	private float setTime = 2.0f;
	private float goTime = 3.0f;
	private float noMoreGoTime = 4.5f;
	private float goTimer = 0;

	private bool raceFinished = false;
	private float finishedTimer = 0;
	private float congratulationsAt = 0.5f;
	private float placeAt = 2.2f;
	private float scoredAt = 3.7f;
	private float showScoreAt = 4.3f;
	private float tryAgainAt = 6.5f;
	
	private bool timeTrial = false;
	
	public int Rank {
		get{
			return rank;
		}
	}
	
	//popups and other points stuff: combos, etc
	
	Popup comboPopup = new Popup();
	int baseGarbageValue = 10;
	int garbageMaxMultiplier = 5;
	int garbageCurrentMultiplier = 1;
	int garbageMinMultiplier = 1;
	float popupXOffset = 0.9f;
	float popupYOffset = 0.3f;
	
	Vector3 comboPosition {
		get{
			float x = camera.orthographicSize * popupXOffset * (Screen.width / Screen.height);
			float y = camera.orthographicSize * popupYOffset;
			return new Vector3(x, y, 5f);
		}
	}
	Color[] comboColours = new Color[5] { Color.white, Color.cyan, Color.blue, Color.magenta, Color.red };
	int[] comboSizes = new int[5] { 10, 11, 12, 14, 17 };
	
	float comboTimer = 0;
	float comboTiming = 2.5f;
	int comboIncrement = 3;
	int comboCounter = 0;
	float comboRotation = 15f;
	
	float timeLimit = 300f;
	float currentTime;
	int pointsPerSecondLeft = 5;
	
	
	Popup bonusPopup = new Popup();
	int topFlagValue = 500;
	int midFlagValue = 200;
	
	Vector3 BonusPosition {
		get{
			float x = camera.orthographicSize * popupXOffset * (Screen.width / Screen.height);
			float y = camera.orthographicSize * (comboPopup.IsActive? 0 : popupYOffset);
			return new Vector3(x, y, 5f);
		}
	}
	Color bonusColour = Color.yellow;
	int bonusSize = 14;
	float showBonusFor = 2.0f;
	float bonusRotation = 15f;
	StyleManager pointsManager = new StyleManager();
	
	public float TimeRemaining{
		get { return timeLimit - currentTime; }
	}
	
	public string TimeRemainingString{
		get {
			
			if (timeTrial){
				return Textf.ConvertTimeToString(RecessManager.CurrentTime);
			}
			
			int seconds = (int) Mathf.Floor(TimeRemaining % 60);
			float centiSeconds = (int) Mathf.Floor((currentTime - (int) currentTime) * 100);
			return Mathf.Floor(TimeRemaining/60).ToString() + (seconds < 10? ":0" : ":") + seconds.ToString() + (centiSeconds < 10? ":0" : ":") + centiSeconds.ToString();
		
		}
	}
	
	public int TimeRemainingPoints{
		get {
			return Mathf.Max((int) TimeRemaining * pointsPerSecondLeft, 0);
		}
	}
	
	[System.SerializableAttribute]
	public class HUDTextures{
		public Texture2D fitzMini;
		public Texture2D ottoMini;
		public Texture2D pinkyMini;
		public Texture2D boogerBoyMini;
		
		public Vector2 miniBorder = new Vector2(12, 12);
		public float miniScreenProportion = 0.3f;
		public Vector2 rankOffsetFromMini = new Vector2 (20, 0);
		public Vector2 timeBorder = new Vector2(14, 14);
		public Vector2 scoreBorder = new Vector2 (10, 20);
		
		public Texture2D ready;
		public Texture2D getSet;
		public Texture2D go;
		
		public Texture2D selectIcon;
		
		public Texture2D itemSlot;
		
		public GUISkin skin;
		
		public Font font;
		public GameObject normalTextMesh;
		
		private int cursorIndex = 0;
		public int CursorIndex {
			get { return cursorIndex; }
			set { cursorIndex = value; }
		}
		
		public Vector2 choiceSize = new Vector2(0.185f, 0.085f);
		public Vector2 congratsStart = new Vector2(0.1f, 0.15f);
		public float statsOffsetX = 0.11f;
	}
	
	public HUDTextures hud;
	//end of the race 
	public float[] endListIntervals = new float[]{ 0.1f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f };
	public bool produceBackground = true;
	public bool toReadySetGo = true;
	
	private Transform[] racers;
	private Transform fitz;
	private List<Popup> popups = new List<Popup>();
	
	private Button continueButton;
	private Button restartButton;
	private Button quitButton;
	
//	private Controller controller = new Controller();
	
	private int rank = 0;
	
	public string RankString {
		get {
			switch (rank){
			case 1:
				return "1st";
			case 2:
				return "2nd";
			case 3:
				return "3rd";
			case 4:
				return "4th";
			default:
				return "0?";
			}
		}
	}
	
	public int RankPoints {
		get {
			if (rank == 1){
				return 200;
			} else if (rank == 2){
				return 125;
			} else if (rank == 3){
				return 50;
			}
			return 0;
		}
	}
	
	// Use this for initialization
	void Awake () {
		if (cam != null){
			Destroy (gameObject);
		}
		else{
			cam = this;
		}
		
		if (RecessManager.currentGameMode == GameModes.timeTrial){
			timeTrial = true;
			
			Bully[] bullies = FindObjectsOfType<Bully>();
			
			foreach (var item in bullies) {
				Destroy(item.gameObject);
			}
		}
	}
	void Start () {

		effectiveBorder = new Rect(border);
		effectiveBorder = new RectOffset((int) camera.orthographicSize * Screen.width/Screen.height,(int)  camera.orthographicSize * Screen.width/Screen.height,
		                                 (int) camera.orthographicSize,(int)  camera.orthographicSize).Remove(effectiveBorder);

		trans = transform;
		audioSource = GetComponent<AudioSource>();
		//Fitz fitzScript = GameObject.FindObjectOfType(typeof(Fitz)) as Fitz;
		try{
			fitzNode = GameObject.Find("Fitzwilliam").GetComponentInChildren<GizmoDad>().transform;
		}
		catch{
			Debug.LogError ("There's no 'Fitzwilliam' in the scene, the camera doesn't like it");
		}
		
		sounds = new Sounds();
		
		Bully[] bullies = FindObjectsOfType<Bully>();
		List<Transform> tlist = new List<Transform>();
		foreach (Bully item in bullies) {
			tlist.Add(item.transform);
		}
		fitz = Fitz.fitz.transform;
		racers = tlist.ToArray();
		transform.position = new Vector3(fitz.transform.position.x, fitz.transform.position.y, transform.position.z);
		
		
		Transform roulette = GetComponentInChildren<Roulette>().transform;
		roulette.localPosition = new Vector3(roulette.localPosition.x, camera.orthographicSize * 0.8f, roulette.localPosition.z);
		
		if (!toReadySetGo){
			goTimer = goTime;
			
		}
		UnityEngine.Object button = Resources.Load("Button");
		GameObject continueButtonGO = Instantiate(button) as GameObject;
		continueButton = continueButtonGO.GetComponent<Button>();
		continueButton.transform.parent = trans;
		continueButton.Text = "Continue!";
		continueButton.camOffset = new Vector2(0, 1.27f);
		continueButton.buttonFunction = TogglePause;
		
		
		GameObject restartButtonGO = Instantiate(button) as GameObject;
		restartButton = restartButtonGO.GetComponent<Button>();
		restartButton.transform.parent = trans;
		restartButton.camOffset = new Vector2(0, -1.41f);
		
		restartButton.Text = "Restart!";
		
		restartButton.buttonFunction = delegate() {
			RecessManager.ClearCurrentScores();
			Application.LoadLevel(Application.loadedLevel);
			Time.timeScale = 1;
		};
		
		GameObject quitButtonGO = Instantiate(button) as GameObject;
		quitButton = quitButtonGO.GetComponent<Button>();
		quitButton.transform.parent = trans;
		
		quitButton.Text = "Quit!";
		quitButton.camOffset = new Vector2(0, -4.11f);
		quitButton.buttonFunction = delegate() {
			RecessManager.ClearCurrentScores();
			Application.LoadLevel(0);
			Time.timeScale = 1;
		};
		ToggleButtons(false);
	}
	
	
	void StartRace() {
		raceBegun = true;
		foreach (Movable mov in FindObjectsOfType(typeof(Movable)) as Movable[]){
			mov.setActivated();
		}
	}
	
	void ToggleButtons (bool value){
		continueButton.Active = value;
		quitButton.Active = value;
		restartButton.Active = value;
	}
	
	void TogglePause (){
		Time.timeScale = 1 - Time.timeScale;
		PlaySound(sounds.collect);
		
		if (Time.timeScale > 0){
			ToggleButtons(false);
		}
		else{
			ToggleButtons(true);
		}
	}
	
	void Update(){
		//pauseTime
		
		bool pauseButton = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P);
		if (pauseButton){
			TogglePause();
		}
		
	}
	
	void FixedUpdate () {
		//TEMP give points for free to test
		if (Input.GetKey(KeyCode.Equals)){
			RecessManager.AddScore(10);
		}
		if (goTimer < noMoreGoTime){
			goTimer += Time.deltaTime;
			if (goTimer > goTime)
				StartRace();
		}
		
		foreach (Popup popup in popups){
			popup.Update ();
		}
		if (comboTimer > 0){
			comboTimer -= Time.deltaTime;
			if (comboTimer <= 0){
				garbageCurrentMultiplier = 1;
				comboCounter = 0;
			}
		}
		if (comboPopup.IsActive){
			comboPopup.Update ();
		}
		
		Vector3 forepos = trans.position;
		if (fitzNode != null && raceBegun){
			Vector3 target = new Vector3(fitzNode.position.x , fitzNode.position.y , trans.position.z);
			trans.position = Vector3.Lerp(trans.position, target, lerpAmount);
			
			if (effectiveBorder.width > 1 && !effectiveBorder.Contains((Vector2)trans.position)){
				trans.position = new Vector3(Mathf.Clamp(trans.position.x, effectiveBorder.xMin, effectiveBorder.xMax),
										Mathf.Clamp(trans.position.y, effectiveBorder.yMin, effectiveBorder.yMax), trans.position.z);
			}
		}
		Vector3 postPos = trans.position;
		
		foreach (Transform tran in parallaxes){
			//Never Used
			//float parAmount = tran.position.z * maxParalax / furthestParalaxZ;		//figure out how much to move each object. Easy because player z = 0 always
			
			tran.Translate((postPos - forepos) * (maxParallax * tran.position.z / furthestParalaxZ), Space.World);
		}
		
		//check for rank
		if (!timeTrial && !raceFinished){
			rank = 1;
			for (int i = 0; i < racers.Length; i ++){
				if (fitz.position.x < racers[i].position.x){
					rank++;
				}
			}
		}
		
		if (raceBegun && !raceFinished){
			RecessManager.CurrentTime += Time.deltaTime;
			currentTime += Time.deltaTime;
		}
		if (raceFinished){
			finishedTimer += Time.deltaTime;
		}
		
	}
	
	void OnGUI(){
		if (goTimer < noMoreGoTime){
			
			
			if (goTimer > readyTime){
				Texture2D goTexture = hud.ready;
				if (goTimer > setTime){
					goTexture = hud.getSet;
				}
				if (goTimer > goTime){
					goTexture = hud.go;
				}
				float gotexWidth = Screen.width * 0.42f;
				float gotexHeight = Screen.height * 0.4f;
				Rect goRect = new Rect(Screen.width/2 - gotexWidth/2, 0, gotexWidth, gotexHeight);
				
				GUI.DrawTexture(goRect, goTexture, ScaleMode.ScaleAndCrop);
			}
		}
		if (!raceFinished){
			Texture2D miniTexture = hud.fitzMini;;
			
			 if (Fitz.fitz.IsOtto){
				miniTexture = hud.ottoMini;
			} else if (Fitz.fitz.IsPinky){
				miniTexture = hud.pinkyMini;
			} else if (Fitz.fitz.IsBoogerBoy){
				miniTexture = hud.boogerBoyMini;
			}
			//miniature showing face
			float miniWidth = Screen.width * hud.miniScreenProportion;
			float miniHeight = miniWidth * miniTexture.height / miniTexture.width;
			Rect miniRect = new Rect(hud.miniBorder.x, hud.miniBorder.y, miniWidth, miniHeight);
			
			GUI.DrawTexture(miniRect, miniTexture);
			
			//text showing place
			//Rect rankRect = new Rect(miniRect.xMin + hud.rankOffsetFromMini.x, miniRect.yMax + hud.rankOffsetFromMini.y, 100,100);
			//GUI.TextArea (rankRect, RankString, hud.skin.textArea);
			
			
			//time of race
			Rect timeRect = new Rect(hud.timeBorder.x, Screen.height - hud.timeBorder.y - 60, 125, 60);
			//TODO: specify between time trial and not time trial
			//GUI.TextArea (timeRect, RecessManager.TimeString, hud.skin.customStyles[0]);
			GUI.TextArea (timeRect, TimeRemainingString, hud.skin.customStyles[0]);
			
			//score areas
			int numberHeight = 50;
			int textHeight = 30;
			int numberWidth = 125;
			Rect scoreRect = new Rect(Screen.width - hud.scoreBorder.x - numberWidth, hud.scoreBorder.y, numberWidth, numberHeight * 2 + textHeight * 2);
			
			GUI.BeginGroup (scoreRect);
				GUI.TextArea (new Rect (0, 0, numberWidth, numberHeight), RecessManager.Score.ToString (), hud.skin.customStyles[1]);
				GUI.TextArea (new Rect (0, numberHeight, numberWidth, textHeight), "Score", hud.skin.customStyles[0]);
				//GUI.TextArea (new Rect (0, numberHeight + textHeight, numberWidth, numberHeight), RecessManager.GarbageCount.ToString(), hud.skin.customStyles[1]);
				//GUI.TextArea (new Rect(0, numberHeight * 2 + textHeight, numberWidth, textHeight), "Garbage", hud.skin.customStyles[0]);
			GUI.EndGroup ();
			
		} else {
			float congratsWidth = Screen.width * 0.7f;
			float congratsHeight = Screen.height * 0.8f;
			float xValue = Screen.height * hud.congratsStart.x;
			float yValue = Screen.height * hud.congratsStart.y;
			float heightValue = Screen.height * 0.05f;
			int intervalIndex = 0;
			GUIStyle statsStyle = new GUIStyle(hud.skin.GetStyle("LevelSelect"));
			statsStyle.fontSize = (int) heightValue;
			
			if (finishedTimer > congratulationsAt){
				GUIStyle boxStyle = new GUIStyle(hud.skin.box);
				boxStyle.contentOffset = new Vector2(0, 35);
				boxStyle.fontSize = (int) (Screen.height * 0.15f);
				GUI.Box (new Rect(xValue, yValue, congratsWidth, congratsHeight), "Congratulations!", boxStyle);
			}
			xValue += hud.statsOffsetX * Screen.width;
			yValue += endListIntervals[intervalIndex] * Screen.height;
			if (finishedTimer > placeAt){
				GUI.TextArea (new Rect(xValue, yValue, congratsWidth - 2 * xValue, 50), "You came in " + RankString + " place!", hud.skin.textArea);
			}
			intervalIndex ++;
			yValue += endListIntervals[intervalIndex] * Screen.height;
			if (finishedTimer > scoredAt){		//TODO: Make this prettier (make sounds happen and count up and appear one at a time and the total should be last
				string stringy = "Your score";
				if (finishedTimer > showScoreAt){
					stringy += ": " + RecessManager.Score.ToString() + Environment.NewLine + 
						"Garbage: " + pointsManager.garbagePoints.ToString() + Environment.NewLine +
						"Rank: " + pointsManager.rankPoints.ToString() + Environment.NewLine +
						"Style: " + pointsManager.stylePoints.ToString() + Environment.NewLine +
						"Time: " + pointsManager.timePoints.ToString();
				}
					
				GUI.TextArea (new Rect(xValue, yValue, congratsWidth - 2 * xValue, heightValue), stringy, hud.skin.textArea);
			
			}
			intervalIndex ++;
			yValue += endListIntervals[intervalIndex] * Screen.height;
			if (finishedTimer > tryAgainAt){
				GUI.TextArea (new Rect(xValue, yValue, congratsWidth - 2 * xValue, heightValue), "What now?", hud.skin.textArea);

			intervalIndex ++;
			yValue += endListIntervals[intervalIndex] * Screen.height;
				
				
//				//input: change cursor
//				controller.CursorInput();
//				if (controller.getUDown){
//					hud.CursorIndex -= 1;
//					if (hud.CursorIndex < 0)
//						hud.CursorIndex = 1;
//				}
//				if (controller.getDDown){
//					hud.CursorIndex += 1;
//					if (hud.CursorIndex > 1){
//						hud.CursorIndex = 0;
//					}
//				}
				
				GUIStyle yesnoStyle = new GUIStyle(hud.skin.customStyles[0]);
				yesnoStyle.alignment = TextAnchor.MiddleLeft;
				GUIStyle homeStyle = new GUIStyle(yesnoStyle);
//				GUIStyle retryStyle = new GUIStyle(yesnoStyle);
				
				if (Application.loadedLevel < Application.levelCount-1){
					
					GUIStyle nextStyle = new GUIStyle(yesnoStyle);
					Rect nextRect = MainMenu.SelectionRect(xValue, yValue, Screen.width * hud.choiceSize.x, Screen.height * hud.choiceSize.y, nextStyle);
						
					bool nextPressed = GUI.Button(nextRect, "Next Race", nextStyle);
					if (nextPressed){
						RecessManager.SaveStatistics(Application.loadedLevel, true);
						Application.LoadLevel(Application.loadedLevel+1);
					}
					
					intervalIndex ++;
					yValue += endListIntervals[intervalIndex] * Screen.height;
				}
				
				Rect homeRect = MainMenu.SelectionRect(xValue, yValue, Screen.width * hud.choiceSize.x, Screen.height * hud.choiceSize.y, homeStyle);
				bool homePressed = GUI.Button(homeRect, "Main Menu", homeStyle);
				if (homePressed){
					RecessManager.SaveStatistics(Application.loadedLevel, true);
					
					RecessManager.LoadLevel(0, RecessManager.currentGameMode);
				}
				/*
				GUIContent yesContent = new GUIContent("Yes");
				if (hud.CursorIndex == 0){
					yesContent.image = hud.selectIcon;
				}
				
				
				bool yes = GUI.Button (new Rect(xValue, yValue, congratsWidth - 2 * xValue, 30), yesContent, yesnoStyle);
				yValue += 40;
				
				GUIContent noContent = new GUIContent("No");
				if (hud.CursorIndex == 1){
					noContent.image = hud.selectIcon;
				}
				bool no = GUI.Button (new Rect(xValue, yValue, congratsWidth - 2 * xValue, 30), noContent, yesnoStyle);
				
				if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return) || Input.GetButtonDown("Jump")){

					if (hud.CursorIndex == 0)
						yes = true;
					if (hud.CursorIndex == 1)
						no = true;
				}
				
				if (yes){
					RecessManager.SaveStatistics(true);
					Application.LoadLevel(Application.loadedLevel + 1);
				}
				if (no){
					RecessManager.SaveStatistics(true);
					Application.LoadLevel(0);
				}*/
			}
		}
	}
	
	public void PlaySound(AudioClip clip){
		PlaySound(clip, 1f);
	}
	public void PlaySound(AudioClip clip, float volume){
		audioSource.volume = volume;
		audioSource.clip = clip;
		audioSource.Play();
	}
	
	public void FinishRace(){
		raceFinished = true;
		fitzNode.parent = null;
		
		//TODO : display the point amounts (maybe make a class of what I earn points for?
		//class for points?
		//points for class B)


		RecessManager.Score += RankPoints;
		RecessManager.Score += TimeRemainingPoints;
		pointsManager.timePoints += TimeRemainingPoints;
		pointsManager.rankPoints += RankPoints;
	}
	
	public void AddGarbage(){
		comboTimer = comboTiming;
		comboCounter ++;
		if (comboCounter % comboIncrement == 0){
			
			garbageCurrentMultiplier ++;
			garbageCurrentMultiplier = Mathf.Clamp (garbageCurrentMultiplier, garbageMinMultiplier, garbageMaxMultiplier);
			comboPopup.Initiate("Combo x " + garbageCurrentMultiplier.ToString(), 
					comboColours[garbageCurrentMultiplier - 1], 
					comboSizes[garbageCurrentMultiplier - 1], 
					comboTiming, 
					comboPosition,
					comboRotation);
			
		} else if (comboPopup.IsActive){
			comboPopup.ExtendPopup();	
		}
		
		RecessManager.AddScore (baseGarbageValue * garbageCurrentMultiplier);
		pointsManager.garbagePoints += baseGarbageValue * garbageCurrentMultiplier;
		
	}
	
	public void ExtraFlagTouchPoints (bool topOfPole){
		int points = topOfPole? topFlagValue : midFlagValue;
		RecessManager.AddScore(points);
		comboPopup.Initiate("Flag Bonus! " + Environment.NewLine + points.ToString(), 
					bonusColour, 
					bonusSize, 
					showBonusFor, 
					BonusPosition,
					bonusRotation);
		
		pointsManager.stylePoints += points;
	}
	
	public void MiscellaneousBonusPoints (string bonusName, int value){
		RecessManager.AddScore(value);
		bonusPopup.Initiate(bonusName + Environment.NewLine + value.ToString(), 
					bonusColour, 
					bonusSize, 
					showBonusFor, 
					BonusPosition,
					bonusRotation);
		
		pointsManager.stylePoints += value;
	}
}
