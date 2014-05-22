using UnityEngine;
using System.Collections;

public enum LetterGrades{
	d,c,b,a,s,ss
}

public class LevelStats {
	
	public int highScore;
	public float bestTime;
	public int bestPlacement;
	public int mostGarbages;
	
	private readonly int garbagesNeededPerLevel = 35;
	
	public readonly int levelIndex;
	public readonly int garbagesToUnlock;
	public readonly GameObject itemToUnlock;
	public readonly string objectName;
	
	public readonly float goldTime;
	public readonly float silverTime;
	public readonly float bronzeTime;
	
	private string ScoreKey {
		get{
			return "HighScore" + levelIndex.ToString();
		}
	}
	private string GarbageKey {
		get{
			return "MostGarbages" + levelIndex.ToString();
		}
	}
	private string PlaceKey {
		get{
			return "BestPlacement" + levelIndex.ToString();
		}
	}
	
	private bool GrandPrix{
		get{ return RecessManager.currentGameMode == GameModes.grandPrix; }
	}
	
	public bool HasUnlockedItem{
		get{ return highScore > garbagesToUnlock; }
	}
	
	public string BestTimeString{
		get{ return Textf.ConvertTimeToString(bestTime); }
	}
	
	public string RankString {
		get {
			switch (bestPlacement){
			case 1:
				return "1st";
			case 2:
				return "2nd";
			case 3:
				return "3rd";
			case 4:
				return "4th";
			default:
				return "-";
			}
		}
	}
	public string LetterGradeString{
		get{
			int scoreInQuestion = RecessManager.currentGameMode == GameModes.grandPrix? highScore : highScore;
			float fraction = (float) scoreInQuestion / (float) (garbagesToUnlock);
			LetterGrades letterGrade = (LetterGrades)Mathf.Round(fraction * (int)LetterGrades.ss);
			
			switch (letterGrade){
			case LetterGrades.ss:
				return "S+";
			case LetterGrades.s:
				return "S";
			case LetterGrades.a:
				return "A";
			case LetterGrades.b:
				return "B";
			case LetterGrades.c:
				return "C";
			default:
				return "D";
			
				
			}
		}
	}
	
	public LevelStats (int levelIndex)
	{
		this.levelIndex = levelIndex;
		this.garbagesToUnlock = (levelIndex - 1) * garbagesNeededPerLevel;
		LoadStats();
		
	}
	
	public void SaveStats (int garbages, int score, int placement) {
		mostGarbages = Mathf.Max(garbages, mostGarbages);
		highScore = Mathf.Max(score, highScore);
		
		int compareTo = bestPlacement == 0? int.MaxValue : bestPlacement;
		bestPlacement = Mathf.Min(placement, compareTo);
		
		PlayerPrefs.SetInt(GarbageKey, mostGarbages);
		PlayerPrefs.SetInt(ScoreKey, highScore);
		PlayerPrefs.SetInt(PlaceKey, bestPlacement);
	}
	
	public void LoadStats (){
		mostGarbages = PlayerPrefs.GetInt(GarbageKey, 0);
		highScore = PlayerPrefs.GetInt(ScoreKey, 0);
		bestPlacement = PlayerPrefs.GetInt(PlaceKey, 0);
	}
	
	public override string ToString ()
	{
		return string.Format ("[LevelStats: Index={0}, garbages={1}, RankString={2}, BestScore={3}]", levelIndex, mostGarbages, RankString, highScore);
	}
}
