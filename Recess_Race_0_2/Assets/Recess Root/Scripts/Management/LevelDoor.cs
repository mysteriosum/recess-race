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
	public Sprite unlockedSprite;
	
	public Texture[] medalTextures;
	
	public ImageElement bg;
	public TextElement title;
	public ImageElement medal;
	public TextElement garbages;
	public TextElement score;
	
	private bool locked = false;
	// Use this for initialization
	void Start () {
		if (levelIndex < smallestIndex)
			smallestIndex = levelIndex;
		
		fitz = Fitz.fitz.transform;
		myBox = GetComponent<BoxCollider2D>();
		levelStats = RecessManager.GetLevelStats(levelIndex);
		
		if (RecessManager.GarbageCount < levelStats.garbagesToUnlock){
			locked = true;
		} else {
			
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			sr.sprite = unlockedSprite;
			//TODO make actual functions to save these things
			int placement = levelStats.bestPlacement;
			if (placement == 0){
				medal.texture = null;
			} else {
				int index = placement > 0? Mathf.Min (placement - 1, medalTextures.Length - 1) : medalTextures.Length - 1;
				medal.texture = medalTextures[index];
			}
			garbages.text = "Garbage: " + levelStats.mostGarbages.ToString ();
			
			score.text = "Best Score: " + levelStats.highScore.ToString();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (locked) return;
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
