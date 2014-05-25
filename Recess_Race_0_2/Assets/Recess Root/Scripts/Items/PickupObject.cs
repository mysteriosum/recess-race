using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupObject : MonoBehaviour {
	
	public ItemInfo[] items;
	
	private List<Sprite> sprites = new List<Sprite>();
	private SpriteRenderer sr;
	
	private float itemRotateSpeed = 0.85f;
	private float rotateTimer = 0;
	private int currentItem = 0;
	// Use this for initialization
	void Start () {
		items = RecessManager.ItemsUnlocked();
		sr = GetComponent<SpriteRenderer>();
		for (int i = 0; i < items.Length; i ++){
			Sprite toAdd = (Resources.Load("Sprites/" + items[i].name) as GameObject).GetComponent<SpriteRenderer>().sprite;
			sprites.Add(toAdd);
			Debug.Log("Adding sprite:" + items[i].name);
		}
		sr.sprite = sprites[currentItem];
	
	}
	
	// Update is called once per frame
	void Update () {
		rotateTimer += Time.deltaTime;
		
		if (rotateTimer > itemRotateSpeed){
			currentItem ++;
			if (currentItem == items.Length)
				currentItem = 0;
			sr.sprite = sprites[currentItem];
			rotateTimer = 0;
		}
	}
	
	void OnTriggerEnter2D (Collider2D other){
		Fitz fitz = other.GetComponent<Fitz>();
		
		if (fitz != null){
			items[currentItem].function();
			
			Destroy(gameObject);
		}
	}
}
