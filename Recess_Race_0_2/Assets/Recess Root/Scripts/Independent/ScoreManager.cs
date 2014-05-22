using UnityEngine;
using System.Collections;

public enum StylePointTypes{
	garbage,
	style,
	time,
	rank,
}

public class ScoreItem {
	public bool earned = false;
	public int points;
	
	public ScoreItem (int points){
		this.points = points;
	}
	
	public int Accomplished (){
		if (!earned){
			earned = true;
			return points;
		} else {
			return 0;
		}
	}
}

public class ScoreManager {
	private bool raceOver = false;
	
	public int totalScore = 0;

	public int rankPoints = 0;
	public int timePoints = 0;
	public int garbagePoints = 0;
	public int stylePoints = 0;
	
	public int currentGarbage = 0;
	public int longestCombo = 0;
	private const int pointsPerCombo = 5;
	
	public ScoreItem hasWallJumped = new ScoreItem(100);
	public ScoreItem hasFloated = new ScoreItem (100);
	public ScoreItem hasFlapped = new ScoreItem(100);
	
	public ScoreItem hasTumbleJumped = new ScoreItem(100);
	public ScoreItem tumbledPastBully = new ScoreItem(200);
	public ScoreItem hasCaughtBalls = new ScoreItem(150);
	
	public void CheckMaxCombo (int combo) {
		if (combo > longestCombo)
			longestCombo = combo;
	}
	
	public void EndRace () {
		if (raceOver)
			return;
		raceOver = true;
		
		int comboPoints = longestCombo * pointsPerCombo;
		stylePoints += comboPoints;
		totalScore += comboPoints;
		
	}
	
	public void AddPoints (int amount, StylePointTypes kind){
		switch (kind){
		case StylePointTypes.garbage:
			currentGarbage++;
			garbagePoints += amount;
			totalScore += amount;
			break;
		case StylePointTypes.rank:
			rankPoints += amount;
			totalScore += amount;
			break;
		case StylePointTypes.style:
			stylePoints += amount;
			totalScore += amount;
			break;
		case StylePointTypes.time:
			timePoints += amount;
			totalScore += amount;
			break;
		default:
			totalScore += amount;
			break;
		}
	}
	
}
