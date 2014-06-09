using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

	public AudioSource audioSource;
	public Sounds sounds;
	
	public static GameManager gm;
	
	//race variables
	
	private bool raceBegun = false;
	private float readyTime = 1.0f;
	private float setTime = 2.0f;
	private float goTime = 3.0f;
	private float noMoreGoTime = 4.5f;
	private float goTimer = 0;

	private bool raceFinished = false;
	private float finishedTimer = 0;
	
	private bool timeTrial = false;
	
	public int Rank {
		get{
			return rank;
		}
	}
	
	public bool RaceBegun{
		get { return raceBegun; }
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
	
	public int ComboIndex {
		get{
			return comboCounter;
		}
	}
	
	public int MaxCombo {
		get{
			return comboSizes.Length;
		}
	}
	
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
	public ScoreManager pointsManager = new ScoreManager();
	
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
		public Texture2D[] medalTextures;
	}
	
	
	public HUDTextures hud;
	private Texture2D MedalTexture {
		get{
			int index = Mathf.Min(rank - 1, hud.medalTextures.Length - 1);
			return hud.medalTextures[index];
		}
	}
	
	//end of the race 
	public float[] endListIntervals = new float[]{ 0.1f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f };
	
	public TextElement[] endTexts;
	public ImageElement[] images;
	public ImageElement MedalElement {
		get{
			return images[1];
		}
	}
	
	public bool toReadySetGo = true;
	public bool isLevelSelect = false;
	
	private Transform[] racers;
	private Transform fitz;
	private List<Popup> popups = new List<Popup>();
	
	
	
	public Button continueButton;
	public Button restartButton;
	public Button quitButton;
	
	[System.SerializableAttribute]
	public class EndcardButtons {
		public Button nextButton;
		public Button restartButton;
		public Button levelButton;
	}
	public EndcardButtons endcardButtons;
	
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
			default:
				return rank.ToString() + "th";
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
		if (gm != null){
			Destroy (gameObject);
		}
		else{
			gm = this;
		}
		
		if (isLevelSelect){
			toReadySetGo = false;
		}
		/*
		if (RecessManager.currentGameMode == GameModes.timeTrial){
			timeTrial = true;
			
			Bully[] bullies = FindObjectsOfType<Bully>();
			
			foreach (var item in bullies) {
				Destroy(item.gameObject);
			}
		}*/
	}
	void Start () {

		
		audioSource = GetComponent<AudioSource>();
		//Fitz fitzScript = GameObject.FindObjectOfType(typeof(Fitz)) as Fitz;
		
		sounds = new Sounds();
		
		Bully[] bullies = FindObjectsOfType<Bully>();
		List<Transform> tlist = new List<Transform>();
		foreach (Bully item in bullies) {
			tlist.Add(item.transform);
		}
		fitz = Fitz.fitz.transform;
		racers = tlist.ToArray();
		transform.position = new Vector3(fitz.transform.position.x, fitz.transform.position.y, transform.position.z);
		
		/*
		Transform roulette = GetComponentInChildren<Roulette>().transform;
		roulette.localPosition = new Vector3(roulette.localPosition.x, camera.orthographicSize * 0.8f, roulette.localPosition.z);
		*/
		if (!toReadySetGo){
			goTimer = noMoreGoTime;
			StartRace();
			
		}
		if (RecessManager.currentGameMode == GameModes.levelSelect){
			isLevelSelect = true;
		}
		
		continueButton.Init();
		continueButton.buttonFunction = TogglePause;
		restartButton.Init();
		restartButton.buttonFunction = RestartLevel;
		quitButton.Init();
		quitButton.buttonFunction = BackToMainMenu;
		
	}
	
	void RestartLevel () {
		Application.LoadLevel(Application.loadedLevel);
		Time.timeScale = 1;
	}
	
	void BackToMainMenu () {
		Application.LoadLevel(0);
		Time.timeScale = 1;
	}
	
	void StartRace() {
		raceBegun = true;
		foreach (Movable mov in FindObjectsOfType(typeof(Movable)) as Movable[]){
			mov.setActivated();
		}
	}
	
	
	void TogglePause (){
		Time.timeScale = 1 - Time.timeScale;
		PlaySound(sounds.collect);
		/*
		if (Time.timeScale > 0){
			ToggleButtons(false);
		}
		else{
			ToggleButtons(true);
		}*/
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
		
		
		//all this stuff is unnecessary if I'm just in a level select
		if (!isLevelSelect){
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
			
			
			if (Time.timeScale <= 0){
				continueButton.Show();
				restartButton.Show();
				quitButton.Show();
				
			}
			
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
			
			if (!isLevelSelect){
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
					GUI.TextArea (new Rect (0, 0, numberWidth, numberHeight), pointsManager.totalScore.ToString (), hud.skin.customStyles[1]);
					GUI.TextArea (new Rect (0, numberHeight, numberWidth, textHeight), "Score", hud.skin.customStyles[0]);
					//GUI.TextArea (new Rect (0, numberHeight + textHeight, numberWidth, numberHeight), RecessManager.GarbageCount.ToString(), hud.skin.customStyles[1]);
					//GUI.TextArea (new Rect(0, numberHeight * 2 + textHeight, numberWidth, textHeight), "Garbage", hud.skin.customStyles[0]);
				GUI.EndGroup ();
			}
		
		} else {
			int smallest = Mathf.Min(new int[3] { this.endTexts.Length, this.endListIntervals.Length, this.images.Length});
			
			float runningTally = 0;
			for (int i = 0; i < smallest; i ++){
				runningTally += endListIntervals[i];
				if (finishedTimer > runningTally){
					if (images[i] != null)
						images[i].Show();
					if (endTexts[i] != null)
						endTexts[i].Show();
				}
			}
			
			
			if (finishedTimer > runningTally && !leavingLevel){
				endcardButtons.levelButton.Show();
				endcardButtons.nextButton.Show();
				endcardButtons.restartButton.Show();
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
		CameraFollow.cam.EndRace();
		
		MedalElement.texture = MedalTexture;
		endcardButtons.nextButton.buttonFunction = GoToNextRace;
		endcardButtons.restartButton.buttonFunction = RetryRace;
		endcardButtons.levelButton.buttonFunction = BackToLevelSelect;

		pointsManager.EndRace();
		pointsManager.AddPoints(TimeRemainingPoints, StylePointTypes.time);
		pointsManager.AddPoints(RankPoints, StylePointTypes.rank);
	}
	
	public void SaveStats () {
		leavingLevel = true;
		try{
			RecessManager.currentLevelStats.SaveStats(pointsManager.currentGarbage, pointsManager.totalScore, rank);
		}
		catch(NullReferenceException nre){
			Debug.LogError("My thing says there's an error. Here's currentLevelStats: " + RecessManager.currentLevelStats.ToString() + " and some random stuff " + nre.Data);
		}
	}
	
	public void GoToNextRace() {
		SaveStats();
		RecessManager.NextLevel();
	}

	public void RetryRace () {
		SaveStats();
		RecessManager.ReloadThisLevel();
	}
	
	public void BackToLevelSelect (){
		Debug.Log("first, here's the currentlevelstats" + RecessManager.currentLevelStats.ToString());
		SaveStats();
		RecessManager.LoadLevelSelect();
	}
	private bool leavingLevel = false;


	public void AddGarbage(PopupText popup){
		
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
		int points = baseGarbageValue * garbageCurrentMultiplier;

		popup.text = "+ " + points;
		Gradient gradient = popup.popupConfiguration.gradient;
		GradientColorKey[] colorkey = new GradientColorKey[2];
		colorkey[0] = new GradientColorKey (comboColours [garbageCurrentMultiplier - 1],0);
		colorkey[1] = new GradientColorKey (Color.white,0);
		gradient.SetKeys (colorkey, gradient.alphaKeys);
		pointsManager.addGarbagePoints (popup, points);
	}
	
	public void ExtraFlagTouchPoints (bool topOfPole){
		int points = topOfPole? topFlagValue : midFlagValue;
		pointsManager.AddPoints(points, StylePointTypes.style);
		comboPopup.Initiate("Flag Bonus! " + Environment.NewLine + points.ToString(), 
					bonusColour, 
					bonusSize, 
					showBonusFor, 
					BonusPosition,
					bonusRotation);
	}
	
	public void MiscellaneousBonusPoints (string bonusName, int value){
		bonusPopup.Initiate(bonusName + Environment.NewLine + value.ToString(), 
					bonusColour, 
					bonusSize, 
					showBonusFor, 
					BonusPosition,
					bonusRotation);
		
		pointsManager.AddPoints(value, StylePointTypes.style);
	}
}
