using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {
	bool finished = false;
	
	float midFlagBonusHeight = 3.8f;
	float topFlagBonusHeight = 5.8f;
	Rect box;
	// Use this for initialization `
	void Start () {
		BoxCollider2D boxCol = GetComponent<BoxCollider2D>(); 
		box = new Rect(transform.position.x + boxCol.center.x - boxCol.size.x/2, transform.position.y + boxCol.center.y - boxCol.size.y/2, boxCol.size.x, boxCol.size.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D (Collider2D other){
		Fitz fitz = other.gameObject.GetComponent<Fitz>();
		if (!finished && fitz){
			if (fitz.transform.position.y > (box.yMin + topFlagBonusHeight)){
				GameManager.gm.ExtraFlagTouchPoints(true);
			}else if(fitz.transform.position.y > (box.yMin + midFlagBonusHeight)){
				GameManager.gm.ExtraFlagTouchPoints(false);
			}
			fitz.FinishRace();
			GameManager.gm.FinishRace();
			GameManager.gm.PlaySound(GameManager.gm.sounds.children);
			finished = true;
			
			
		}
	}
}
