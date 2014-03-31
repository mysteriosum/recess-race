using UnityEngine;
using System.Collections;

public enum LetterGrades{
	d,c,b,a,s,ss
}

public class LevelStats {
	
	public int highScore;
	public float bestTime;
	public readonly int levelIndex;
	public readonly int scoreToUnlock;
	public readonly GameObject itemToUnlock;
	public readonly string objectName;
	
	public readonly float goldTime;
	public readonly float silverTime;
	public readonly float bronzeTime;

	public bool HasGold {
		get{ return bestTime > 0 && bestTime < goldTime; }
	}
	public bool HasSilver {
		get{ return bestTime > 0 && bestTime < silverTime; }
	}
	public bool HasBronze {
		get{ return bestTime > 0 && bestTime < bronzeTime; }
	}
	
	public bool HasUnlockedItem{
		get{ return highScore > scoreToUnlock; }
	}
	public string BestTimeString{
		get{ return Textf.ConvertTimeToString(bestTime); }
	}
	
	public string GoldTimeString{
		get{ return Textf.ConvertTimeToString(goldTime); }
	}
	public string SilverTimeString{
		get{ return Textf.ConvertTimeToString(silverTime); }
	}
	public string BronzeTimeString{
		get{ return Textf.ConvertTimeToString(bronzeTime); }
	}
	
	public string LetterGradeString{
		get{
			float fraction = (float) highScore / (float) (scoreToUnlock);
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
	
	public LevelStats (int levelIndex, int scoreToUnlock, string objectName, float goldTime, float silverTime, float bronzeTime)
	{
		this.levelIndex = levelIndex;
		this.scoreToUnlock = scoreToUnlock;
		this.objectName = objectName;
		this.goldTime = goldTime;
		this.silverTime = silverTime;
		this.bronzeTime = bronzeTime;
		
		itemToUnlock = Resources.Load ("objects/" + objectName) as GameObject;
		
		highScore = PlayerPrefs.GetInt("highScore" + levelIndex.ToString(), 0);
		bestTime = PlayerPrefs.GetFloat ("bestTime" + levelIndex.ToString (), 0);
		
	}

	
}
