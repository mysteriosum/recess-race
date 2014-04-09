using UnityEngine;
using System.Collections;

public class RecessManager {
	

	private static int score;
	private static float currentTime;
	private static int garbage = 0;
	
	public static GameModes currentGameMode;
	
	private static int currentLevel;
	
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
			//int minutes = (int) Mathf.Floor(currentTime/60);
//			int seconds = (int) Mathf.Floor(currentTime % 60);
//			float centiSeconds = (int) Mathf.Floor((currentTime - (int) currentTime) * 100);
//			return Mathf.Floor(currentTime/60).ToString() + (seconds < 10? ":0" : ":") + seconds.ToString() + ":" + centiSeconds.ToString();
			
			return Textf.ConvertTimeToString(currentTime);
	
		}
	}
	public static readonly int garbageValue = 10;
	
	public static LevelStats[] levelStats = new LevelStats[4]{
		//level 1
		new LevelStats(1, 3000, "Banana", 200f, 230f, 260f),
		new LevelStats(2, 3000, "Hookshot", 200f, 230f, 260f),
		new LevelStats(3, 3000, "ComboCandy", 200f, 230f, 260f),
		new LevelStats(4, 3000, "Ahrah", 200f, 230f, 260f),
		
	};
	
	private static RecessManager instance;
	public static RecessManager Instance{
		get{
			if (instance == null){
				instance = new RecessManager();
			}
			return instance;
		}
	}
	
	static RecessManager(){
		
		
		
	}
	
	public static void AddGarbageToScore(){
		score+= garbageValue;
		garbage ++;
	}
	
	public static void AddScore (int value){
		score += value;
	}
	
	public static void SaveStatistics(int level, bool eraseCurrent){
		if (currentGameMode == GameModes.timeTrial){
			if (score > levelStats[level - 1].highScoreTT){
				PlayerPrefs.SetInt("highScoreTT" + level.ToString(), score);
				levelStats[level - 1].highScoreTT = score;
			}
//			else{
//				Debug.Log("Score too low");
//			}
			if (currentTime < levelStats[level - 1].bestTime || levelStats[level - 1].bestTime == 0){
				PlayerPrefs.SetFloat("bestTime" + level.ToString(), currentTime);
				levelStats[level - 1].bestTime = currentTime;
			}
//			else{
//				Debug.Log("time too low");
//			}
			if (eraseCurrent){
				score = 0;
				currentTime = 0;
			}
		} else {
			if (score > levelStats[level - 1].highScoreGP || levelStats[level - 1].highScoreGP == 0){
				PlayerPrefs.SetInt("highScoreGP" + level.ToString(), score);
				levelStats[level - 1].highScoreGP = score;
			}
			
			if (RecessCamera.cam.Rank < levelStats[level-1].bestRank || levelStats[level-1].bestRank == 0){
				PlayerPrefs.SetInt ("bestRank" + level.ToString(), RecessCamera.cam.Rank);
				levelStats[level - 1].bestRank = RecessCamera.cam.Rank;
			}
		}
		
	}
	
	public static void LoadLevel(int index, GameModes gameMode){
		currentGameMode = gameMode;
		Application.LoadLevel(index);
	}
	
	
}

public enum GameModes{
	grandPrix, timeTrial
}