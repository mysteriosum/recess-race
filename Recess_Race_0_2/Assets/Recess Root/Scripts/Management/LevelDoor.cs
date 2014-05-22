using UnityEngine;
using System.Collections;

public class LevelDoor : MonoBehaviour {
	
	
	private Transform fitz;
	private BoxCollider2D fitzBox;
	private BoxCollider2D myBox;
	
	private LevelStats levelStats;
	
	public int levelIndex;
	public static int smallestIndex = int.MaxValue;
	
	new private bool active = false;
	
	public Texture[] medalTextures;
	
	public ImageElement bg;
	public TextElement title;
	public ImageElement medal;
	public TextElement garbages;
	public TextElement score;
	// Use this for initialization
	void Start () {
		if (levelIndex < smallestIndex)
			smallestIndex = levelIndex;
		
		fitz = Fitz.fitz.transform;
		myBox = GetComponent<BoxCollider2D>();
		levelStats = RecessManager.GetLevelStats(levelIndex);
		
		//TODO make actual functions to save these things
		medal.texture = medalTextures[0];
		garbages.text += " 0";
		
		score.text += " " + levelStats.highScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (myBox.OverlapPoint(fitz.position)){
			active = true;
			
			if (Fitz.fitz.PushedUpThisFrame){
				RecessManager.LoadLevel(levelIndex, GameModes.grandPrix);
			}
		} else {
			active = false;
		}
		
	}
	
	void OnGUI () {
		if (active){
			bg.Show();
			title.Show();
			
			medal.Show();
			score.Show();
			garbages.Show();
		}
	}
}
