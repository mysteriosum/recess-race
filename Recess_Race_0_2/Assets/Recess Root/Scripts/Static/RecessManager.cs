﻿using UnityEngine;
using System.Collections;

public class RecessManager {
	

	private static int score;
	private static float currentTime;
	private static int highScore;
	private static float bestTime = 0f;
	private static int garbage = 0;
	
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
			int seconds = (int) Mathf.Floor(currentTime % 60);
			float centiSeconds = (int) Mathf.Floor((currentTime - (int) currentTime) * 100);
			return Mathf.Floor(currentTime/60).ToString() + (seconds < 10? ":0" : ":") + seconds.ToString() + ":" + centiSeconds.ToString();}
	}
	
	public static readonly int garbageValue = 10;
	
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
		highScore = PlayerPrefs.GetInt("highScore", 0);
		bestTime = PlayerPrefs.GetFloat("bestTime", 0);
	}
	
	public static void AddGarbageToScore(){
		score+= garbageValue;
		garbage ++;
	}
	
	public static void AddScore (int value){
		score += value;
	}
	
	public static void SaveStatistics(bool eraseCurrent){
		if (score > highScore){
			PlayerPrefs.SetInt("highScore", score);
			highScore = score;
		}
		if (currentTime < bestTime){
			PlayerPrefs.SetFloat("bestTime", currentTime);
			bestTime = currentTime;
		}
		if (eraseCurrent){
			score = 0;
			currentTime = 0;
		}
	}
	
	
}
