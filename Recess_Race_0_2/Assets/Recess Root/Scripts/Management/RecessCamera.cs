using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecessCamera : MonoBehaviour {
	private Transform t;
	private Transform fitzNode;
	private Transform box;
	
	public static RecessCamera cam;
	const int scrnHeight = 480;
	const int scrnWidth = 640;
	
	private Transform[] parallaxes;
	public Sprite[] backgroundElements;
	
	private float farDistance;
	private int minimumAtStart = 5;
	private float minimumDistance = -10;
	private float bgDistance = 40;
	
	private float lerpAmount = 0.1f;
	private float maxParalax = 0.3f;
	private float furthestParalaxZ;
	
	private bool raceFinished = false;
	private float finishedTimer = 0;
	private float congratulationsAt = 0.5f;
	private float placeAt = 2.2f;
	private float scoredAt = 3.7f;
	private float showScoreAt = 4.3f;
	private float tryAgainAt = 6.5f;
	
	
	private Transform[] racers;
	private Transform fitz;
	
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
		
		
		Movable[] movables = FindObjectsOfType<Movable>();
		List<Transform> tlist = new List<Transform>();
		foreach (Movable item in movables) {
			if (item.GetComponent<Fitz>() != null){	
				fitz = item.transform;
			}
			else{
				tlist.Add(item.transform);
			}
		}
		racers = tlist.ToArray();
		
		Vector3 defaultPosition = new Vector3(minimumDistance, t.position.y, bgDistance);
		
		
		//---------------------------------------------------------------\\
		//--------------------------place bg tiles-----------------------\\
		//---------------------------------------------------------------\\
		List<Transform> parallaxen = new List<Transform>();
		
		for (int i = 0; i < minimumAtStart; i++) {
			GameObject newGuy = new GameObject("background" + i.ToString());
			SpriteRenderer newSprite = newGuy.AddComponent<SpriteRenderer>();
			int rando = Random.Range (0, backgroundElements.Length);
			newSprite.sprite = backgroundElements[rando];
			newGuy.transform.position = defaultPosition + Vector3.right * newSprite.sprite.bounds.extents.x * 2;
			defaultPosition = newGuy.transform.position;
			newSprite.sortingLayerName = "Background";
			parallaxen.Add (newGuy.transform);
		}
		
		parallaxes = parallaxen.ToArray();
		
		foreach(Transform tr in parallaxes){
			if (tr.position.z > furthestParalaxZ){
				furthestParalaxZ = tr.position.z;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 forepos = t.position;
		if (fitzNode != null){
			Vector3 target = new Vector3(fitzNode.position.x , fitzNode.position.y , t.position.z);
			t.position = Vector3.Lerp(t.position, target, lerpAmount);
		}
		Vector3 postPos = t.position;
		
		foreach (Transform tran in parallaxes){
			//Never Used
			//float parAmount = tran.position.z * maxParalax / furthestParalaxZ;		//figure out how much to move each object. Easy because player z = 0 always
			
			tran.Translate((postPos - forepos) * maxParalax, Space.World);
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
		
		
		RecessManager.CurrentTime += Time.deltaTime;
		if (raceFinished){
			finishedTimer += Time.deltaTime;
		}
		
	}
	
	void OnGUI(){
		if (!raceFinished){
			GUI.Box(new Rect(0, 0, 100, 50), RecessManager.Score.ToString());
			GUI.Box(new Rect(0, Screen.height - 50, 100, 50), RecessManager.TimeString);
			GUI.Box (new Rect(0, 50, 100, 50), RankString);
		} else {
			if (finishedTimer > congratulationsAt){
				GUI.Box (new Rect(Screen.width / 2 - 100, Screen.height/4, 200, 50), "Congratulations!");
			}
			
			if (finishedTimer > placeAt){
				GUI.Box (new Rect(Screen.width/ 2 - 100, Screen.height/4 + 60, 200, 30), "You came in " + RankString + " place!");
			}
			
			if (finishedTimer > scoredAt){
				GUI.Box (new Rect(Screen.width / 2 - 60, Screen.height/4 + 100, 120, 30), "You scored: " + (finishedTimer > showScoreAt? RecessManager.Score.ToString() : ""));
			}
			
			if (finishedTimer > tryAgainAt){
				GUI.Box (new Rect(Screen.width / 2 - 100, Screen.height/4 + 150, 200, 70), "Try again?");
				bool yes = GUI.Button (new Rect(Screen.width/2 - 50, Screen.height/4 + 180, 100, 20), "Yes");
				bool no = GUI.Button (new Rect(Screen.width/2 - 50, Screen.height/4 + 200, 100, 20), "No");
				
				if (yes){
					Application.LoadLevel(Application.loadedLevel);
				} else if (no){
					Application.Quit();
				}
			}
		}
	}
	
	public void FinishRace(){
		raceFinished = true;
		fitzNode.parent = null;
		
		RecessManager.Score += RankPoints;
	}
}
