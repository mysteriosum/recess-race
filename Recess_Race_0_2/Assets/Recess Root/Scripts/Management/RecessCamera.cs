using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RecessCamera : MonoBehaviour {
	private Transform t;
	private Transform fitzNode;
	private Transform box;
	
	public static RecessCamera cam;
	const int scrnHeight = 480;
	const int scrnWidth = 640;
	
	private Transform[] parallaxes;
	public Sprite[] backgroundElements;
	public Sprite silhouette;
	public Sprite fence; 
	
	private float farDistance;
	private int minimumAtStart = 70;
	private float minimumDistance = -10;
	private float bgDistance = 60;
	private float silhouetteDistance = 100;
	private float silhouetteYOffset = 1;
	private float fenceYOffset = -2;
	private float fenceDistance = 27;
	
	
	private float lerpAmount = 0.1f;
	private float maxParallax = 0.3f;
	private float furthestParalaxZ;
	
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
	
	
	[System.SerializableAttribute]
	public class HUDTextures{
		public Texture2D fitzMini;
		public Texture2D ottoMini;
		public Texture2D pinkyMini;
		public Texture2D boogerBoyMini;
		public Texture2D stunnedMini;
		
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
		
		
		private int cursorIndex = 0;
		public int CursorIndex {
			get { return cursorIndex; }
			set { cursorIndex = value; }
		}
	}
	
	public HUDTextures hud;
	
	private Transform[] racers;
	private Transform fitz;
	
	private Controller controller = new Controller();
	
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
	}
	void Start () {
		t = transform;
		//Fitz fitzScript = GameObject.FindObjectOfType(typeof(Fitz)) as Fitz;
		try{
			fitzNode = GameObject.Find("Fitzwilliam").GetComponentInChildren<GizmoDad>().transform;
		}
		catch{
			Debug.LogError ("There's no 'Fitzwilliam' in the scene, the camera doesn't like");
		}
		
		
		Bully[] bullies = FindObjectsOfType<Bully>();
		List<Transform> tlist = new List<Transform>();
		foreach (Bully item in bullies) {
			tlist.Add(item.transform);
		}
		fitz = Fitz.fitz.transform;
		racers = tlist.ToArray();
		
		Vector3 defaultPosition = new Vector3(minimumDistance, t.position.y, bgDistance);
		
		
		//---------------------------------------------------------------\\
		//--------------------------place bg tiles-----------------------\\
		//---------------------------------------------------------------\\
		List<Transform> parallaxen = new List<Transform>();
		
		for (int i = 0; i < minimumAtStart; i++) {
			GameObject newGuy = new GameObject("background" + i.ToString());
			SpriteRenderer newSprite = newGuy.AddComponent<SpriteRenderer>();
			int rando = UnityEngine.Random.Range (0, backgroundElements.Length);
			newSprite.sprite = backgroundElements[rando];
			newGuy.transform.position = defaultPosition + Vector3.right * newSprite.sprite.bounds.size.x;
			defaultPosition = newGuy.transform.position;
			newSprite.sortingLayerName = "Background";
			parallaxen.Add (newGuy.transform);
		}
		
		Vector3 silhouettePosition = new Vector3(minimumDistance, t.position.y + silhouetteYOffset, silhouetteDistance);
		
		for (int i = 0; i < minimumAtStart; i++) {
			GameObject newGuy = new GameObject("background" + i.ToString());
			SpriteRenderer newSprite = newGuy.AddComponent<SpriteRenderer>();
			newSprite.sprite = silhouette;
			newGuy.transform.position = silhouettePosition + Vector3.right * newSprite.sprite.bounds.size.x;
			silhouettePosition = newGuy.transform.position;
			newSprite.sortingLayerName = "Background";
			parallaxen.Add (newGuy.transform);
		}
		
		Vector3 fencePosition = new Vector3(minimumDistance, t.position.y + fenceYOffset, fenceDistance);
		
		for (int i = 0; i < minimumAtStart; i++) {
			GameObject newGuy = new GameObject("background" + i.ToString());
			SpriteRenderer newSprite = newGuy.AddComponent<SpriteRenderer>();
			newSprite.sprite = fence;
			newGuy.transform.position = fencePosition + Vector3.right * newSprite.sprite.bounds.size.x;
			fencePosition = newGuy.transform.position;
			newSprite.sortingLayerName = "Background";
			parallaxen.Add (newGuy.transform);
		}
		parallaxes = parallaxen.ToArray();
		
		foreach(Transform tr in parallaxes){
			if (tr.position.z > furthestParalaxZ){
				furthestParalaxZ = tr.position.z;
				Debug.Log ("Furthest is " + furthestParalaxZ);
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (goTimer < noMoreGoTime){
			goTimer += Time.deltaTime;
			
			if (goTimer > goTime){
				raceBegun = true;
				foreach (Movable mov in FindObjectsOfType(typeof(Movable)) as Movable[]){
					mov.Go();
				}
			}
		}
		
		
		Vector3 forepos = t.position;
		if (fitzNode != null && raceBegun){
			Vector3 target = new Vector3(fitzNode.position.x , fitzNode.position.y , t.position.z);
			t.position = Vector3.Lerp(t.position, target, lerpAmount);
		}
		Vector3 postPos = t.position;
		
		foreach (Transform tran in parallaxes){
			//Never Used
			//float parAmount = tran.position.z * maxParalax / furthestParalaxZ;		//figure out how much to move each object. Easy because player z = 0 always
			
			tran.Translate((postPos - forepos) * (maxParallax * tran.position.z / furthestParalaxZ), Space.World);
		}
		
		//check for rank
		if (!raceFinished){
			rank = 1;
			for (int i = 0; i < racers.Length; i ++){
				if (fitz.position.x < racers[i].position.x){
					rank++;
				}
			}
		}
		
		if (raceBegun && !raceFinished){
			RecessManager.CurrentTime += Time.deltaTime;
		}
		if (raceFinished){
			finishedTimer += Time.deltaTime;
		}
		
	}
	
	void OnGUI(){
		if (goTimer < noMoreGoTime){
			
			
			if (goTimer > readyTime){
				string goText = "Ready...";
				Texture2D goTexture = hud.ready;
				if (goTimer > setTime){
					goTexture = hud.getSet;
					goText = "Set...";
				}
				if (goTimer > goTime){
					goTexture = hud.go;
					goText = "GO!";
				}
				float gotexWidth = Screen.width * 0.42f;
				float gotexHeight = Screen.height * 0.4f;
				Rect goRect = new Rect(Screen.width/2 - gotexWidth/2, 0, gotexWidth, gotexHeight);
				
				GUI.DrawTexture(goRect, goTexture, ScaleMode.ScaleAndCrop);
				GUI.TextArea(goRect, goText, hud.skin.customStyles[3]);
			}
		}
		if (!raceFinished){
			Texture2D miniTexture = hud.fitzMini;;
			
			if (Fitz.fitz.IsHurt){
				miniTexture = hud.stunnedMini;
			} else if (Fitz.fitz.IsOtto){
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
			Rect rankRect = new Rect(miniRect.xMin + hud.rankOffsetFromMini.x, miniRect.yMax + hud.rankOffsetFromMini.y, 100,100);
			//GUI.TextArea (rankRect, RankString, hud.skin.textArea);
			
			
			GUI.Box(new Rect(0, 0, 100, 50), RecessManager.Score.ToString());
			//time of race
			Rect timeRect = new Rect(hud.timeBorder.x, Screen.height - hud.timeBorder.y - 60, 125, 60);
			GUI.TextArea (timeRect, RecessManager.TimeString, hud.skin.customStyles[0]);
			
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
			GUI.BeginGroup(new Rect((Screen.width - congratsWidth) / 2, (Screen.height - congratsHeight)/2, congratsWidth, congratsHeight));
			float yValue = 0;
			float xValue = 100;
			
			if (finishedTimer > congratulationsAt){
				GUIStyle boxStyle = new GUIStyle(hud.skin.box);
				boxStyle.contentOffset = new Vector2(0, 35);
				GUI.Box (new Rect(0, yValue, congratsWidth, congratsHeight), "Congratulations!", boxStyle);
			}
			yValue += 125;
			if (finishedTimer > placeAt){
				GUI.TextArea (new Rect(xValue, yValue, congratsWidth - 2 * xValue, 50), "You came in " + RankString + " place!", hud.skin.textArea);
			}
			yValue += 150;
			if (finishedTimer > scoredAt){
				GUI.TextArea (new Rect(xValue, yValue, congratsWidth - 2 * xValue, 50), "You scored: " + (finishedTimer > showScoreAt? RecessManager.Score.ToString() : ""), hud.skin.textArea);
			}
			yValue += 100;
			if (finishedTimer > tryAgainAt){
				GUI.TextArea (new Rect(xValue, yValue, congratsWidth - 2 * xValue, 70), "Try again?", hud.skin.textArea);
				yValue += 70;
				
				
				//input: change cursor
				controller.CursorInput();
				if (controller.getUDown){
					hud.CursorIndex -= 1;
					Debug.Log ("going up" + hud.CursorIndex);
					if (hud.CursorIndex < 0)
						hud.CursorIndex = 1;
					Debug.Log ("going up" + hud.CursorIndex);
				}
				if (controller.getDDown){
					hud.CursorIndex += 1;
					if (hud.CursorIndex > 1){
						hud.CursorIndex = 0;
					}
				}
				
				GUIStyle yesnoStyle = new GUIStyle(hud.skin.customStyles[0]);
				yesnoStyle.alignment = TextAnchor.MiddleLeft;
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
				
				if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)){
					if (hud.CursorIndex == 0)
						yes = true;
					if (hud.CursorIndex == 1)
						no = true;
				}
				
				if (yes){
					RecessManager.SaveStatistics(true);
					Application.LoadLevel(Application.loadedLevel);
				}
				if (no){
					RecessManager.SaveStatistics(true);
					Application.Quit();
				}
			}
			GUI.EndGroup();
		}
	}
	
	public void FinishRace(){
		raceFinished = true;
		fitzNode.parent = null;
		
		RecessManager.Score += RankPoints;
	}
}
