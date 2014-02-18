﻿using UnityEngine;
using System.Collections;

public class Roulette : MonoBehaviour {
	
	public Item[] items;
	private Item currentItem;
	private SpriteRenderer display;
	private int index = 0;
	
	private float displayTiming = 0.15f;
	private float displayTimingMedium = 0.4f;
	private float displayTimingLong = 0.66f;
	
	private float currentTiming;
	
	private float rouletteTiming = 7f;
	private float rouletteTimer = float.MaxValue;
	private float spinTimer = 0f;
	
	private float mediumAt = 4f;
	private float longAt = 6f;
	
	private float runHoldMultiplier = 3f;
	private static Roulette instance;
	public static Roulette Instance{
		get{ 
			if (!instance)
				instance = FindObjectOfType<Roulette>() as Roulette;
			return instance;
		}
	}
	
	public Item CurrentItem{
		get { return currentItem; }
	}
	
	private int NextIndex(int current, int max){
		int result = current + 1;
		if (result >= max){
			result = 0;
		}
		return result;
	}
	// Use this for initialization
	void Start () {
		//StartRoulette();
		display = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentItem != null && Input.GetButtonDown("Run")){
			currentItem.Activate();
			currentItem = null;
			display.sprite = null;
			
		}
		
		if (rouletteTimer > rouletteTiming)
			 return;
		
		float increment = Time.deltaTime * (Input.GetButton("Run")? runHoldMultiplier : 1);
		rouletteTimer += increment;
		spinTimer += Time.deltaTime;
		
		if (rouletteTimer > rouletteTiming){
			currentItem = items[index];
		}
		else if (spinTimer > currentTiming){
			index = NextIndex(index, items.Length);
			if (rouletteTimer > mediumAt)
				currentTiming = displayTimingMedium;
			if (rouletteTimer > longAt)
				currentTiming = displayTimingLong;
			spinTimer = 0;
			
			display.sprite = items[index].GetSprite();
		}
		
		
	}
	
	public void StartRoulette(){
		if (currentItem != null) return;
		
		index = Random.Range(0, items.Length);
		currentTiming = displayTiming;
		spinTimer = 0;
		rouletteTimer = 0;
	}
}
