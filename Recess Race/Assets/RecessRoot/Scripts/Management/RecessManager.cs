using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecessManager : MonoBehaviour {
	
	static RecessManager instance;
	
	public int levelOffset = 200;
	
	public int loadLevelMin = 0;
	public int loadLevelMax;
	public int levelsToLoad = 10;
	public int roomsPerLevel = 5;
	
	private int curLevel = -1;
	private Transform curCheckpoint;
	
	public int docLayer;
	public int brickLayer;
	
	string roomString = "room_";
	
	private tk2dUIManager tk2dUI;
	
	public static RecessManager Instance{
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(RecessManager)) as RecessManager;
			
			return instance;
		}
	}
	
	private GameObject prevLevel;
	
	public int loadingLevel = 0;
	
	private List<Decoy[]> tempDecoyses = new List<Decoy[]>();
	private Decoy[][] decoyses;
	// Use this for initialization
	void Start () {
		
		//loadLevelMax = Application.levelCount - 1;
		DontDestroyOnLoad(this.gameObject);
		
		for (loadingLevel = 1; loadingLevel <= levelsToLoad; loadingLevel ++){
		
			int index = Random.Range(0, roomsPerLevel);
			
			Application.LoadLevelAdditive(roomString + loadingLevel.ToString() + "_" + index.ToString());
			
		}
		
		if (TextBox.bubble == null){
			Instantiate(Resources.Load("textBox"));
		}
		
		tk2dUI = GetComponent<tk2dUIManager>();
		
		
			//-------------------------------setting layer variables-------------------------------
		docLayer = LayerMask.NameToLayer("danger");
		brickLayer = LayerMask.NameToLayer("brickBlock");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void NextLevel (int index, Transform cp){
		if (index == curLevel) return;
		Debug.Log ("NEXT LEVLE");
		curCheckpoint = cp;
		foreach (Decoy decoy in decoyses[index]){
			decoy.Activate ();
		}
		if (index != 0){
			
			foreach(Decoy decoy in decoyses[index - 1]){
				decoy.Deactivate ();
			}
		}
		curLevel = index;
	}
	
	public void Death() {
		Fitz.fitz.transform.position = curCheckpoint.position;
	}
	
	public void AddDecoys (Decoy[] decoys){
		Debug.Log ("DECOYS ADD");
		tempDecoyses.Add (decoys);
		if (tempDecoyses.Count == levelsToLoad){
			decoyses = tempDecoyses.ToArray ();
		}
	}
}
