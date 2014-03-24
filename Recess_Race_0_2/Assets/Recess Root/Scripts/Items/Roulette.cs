using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Roulette : MonoBehaviour {
	
	public Item[] items;
	public Item[] secretItems;
	public bool bananaUnlocked;
	public Sprite[] bananaSprites;
	public bool ahrahUnlocked;
	
	private int bananaMax = 3;
	private int currentBanana = 3;
	
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
		
		List<Item> tempList = new List<Item>();
		
		if (bananaUnlocked){
			foreach (Item item in secretItems){
				if (item.name == "Banana"){
					tempList.Add(item);
				}
			}
		}
		if (ahrahUnlocked){
			foreach (Item item in secretItems){
				if (item.name == "Ahrah"){
					tempList.Add(item);
				}
			}
		}
		
		foreach (Item item in items){
			tempList.Add(item);
		}
		
		items = tempList.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentItem != null && Input.GetButtonDown("Run")){
			currentItem.Activate();
			Debug.Log("Current name is " + currentItem.name);
			if (currentItem.name == "Banana" && currentBanana > 0){
				currentBanana --;
				if (currentBanana > 0)
					display.sprite = bananaSprites[currentBanana - 1];
			} else {
				currentItem = null;
				display.sprite = null;
				currentBanana = bananaMax;
			}
			
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
