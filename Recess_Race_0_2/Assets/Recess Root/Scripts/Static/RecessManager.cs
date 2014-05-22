using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum GameModes{
	grandPrix, timeTrial, levelSelect,
}


public enum ItemsEnum {
	boogerBoy,
	otto,
	pinky,
	banana,
}

public static class RecessManager {
	

	private static int score;
	private static float currentTime;
	private static int garbage = 0;
	
	public static ItemInfo[] itemInfo; 
	
	public static void InitiateItemInfo () {
		itemInfo = new ItemInfo[4] {
			new ItemInfo("Booger", 0, Fitz.fitz.ChangeToBoogerBoy),
			new ItemInfo("Bowtie", 1, Fitz.fitz.ChangeToOtto),
			new ItemInfo("Lolly", 2, Fitz.fitz.ChangeToPinky),
			new ItemInfo("Banana", 3, Fitz.fitz.ChangeToBoogerBoy)
		};
	}
	private const int defaultItems = 1 << (int)ItemsEnum.boogerBoy | 1 << (int)ItemsEnum.otto | 1 << (int)ItemsEnum.pinky;
	
	public static GameModes currentGameMode = GameModes.grandPrix;
	
	//private static int currentLevel;
	
	
	public static int Score {
		get{ return score; }
		set{ score = value; }
	}
	public static int GarbageCount {
		get { return garbage; }
	}
	public static float CurrentTime{
		get{ return currentTime; }
		set{ currentTime = value; }
	}
	public static string TimeString{
		get {
			return Textf.ConvertTimeToString(currentTime);
		}
	}
	public static readonly int garbageValue = 10;
//	
//	public static LevelStats[] levelStats = new LevelStats[4]{
//		//level 1
//		new LevelStats(1, 3000, "Banana", 200f, 230f, 260f),
//		new LevelStats(2, 3000, "Hookshot", 200f, 230f, 260f),
//		new LevelStats(3, 3000, "ComboCandy", 200f, 230f, 260f),
//		new LevelStats(4, 3000, "Ahrah", 200f, 230f, 260f),
//		
//	};
	public static LevelStats[] levelStats = new LevelStats[4]{
		new LevelStats(1), new LevelStats(2), new LevelStats(3), new LevelStats(4)
	};
	
	public static int LevelSelectIndex{
		get{ return Application.levelCount - 1; }
	}
	
	public static Vector2 levelSelectPosition;
	
	public static LevelStats currentLevelStats = null;
	
	
	public static LevelStats GetLevelStats (int index){
		foreach (LevelStats ls in levelStats){
			if (ls.levelIndex == index){
				return ls;
			}
		}
		return null;
	}
	
	
	
	public static void AddGarbageToScore(){
		score+= garbageValue;
		garbage ++;
	}
	
	public static void AddScore (int value){
		score += value;
	}
	
	
	
	public static void ClearCurrentScores(){
		score = 0;
		currentTime = 0;
		garbage = 0;
	}
	
	public static void LoadLevel(int index, GameModes gameMode){
		currentGameMode = gameMode;
		currentLevelStats = levelStats[index];
//		currentLevel = index;
		
		if (Application.loadedLevel == LevelSelectIndex){
			levelSelectPosition = Fitz.fitz.transform.position;
		}
		Application.LoadLevel(index);
	}
	
	public static void NextLevel () {
		LoadLevel(currentLevelStats.levelIndex + 1, GameModes.grandPrix);
	}
	
	public static void ReloadThisLevel () {
		LoadLevel (currentLevelStats.levelIndex, GameModes.grandPrix);
	}
	
	public static void LoadLevelSelect () {
		currentLevelStats = null;
		currentGameMode = GameModes.levelSelect;
		Application.LoadLevel(LevelSelectIndex);
	}
	
	public static ItemInfo[] ItemsUnlocked (){
		if (itemInfo == null){
			InitiateItemInfo();
		}
		int items = PlayerPrefs.GetInt("ItemsInRoulette", defaultItems);
		Debug.Log("Default items: " + items);
		
		List<ItemInfo> itemList = new List<ItemInfo>();
		int max = System.Enum.GetValues(typeof (ItemsEnum)).Length - 1;
		
		for (int i = 0; i < max; i ++){
			int shifted = 1 << i;
			if ((items & shifted) == shifted){
				itemList.Add(itemInfo[i]);
				Debug.Log("Adding an item: " + itemInfo[i].name);
			}
		}
		
		return itemList.ToArray();
	}
}