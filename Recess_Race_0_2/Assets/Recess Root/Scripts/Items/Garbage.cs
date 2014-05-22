using UnityEngine;
using System.Collections;

public class Garbage : MonoBehaviour {

	public Sprite[] garbages;
	public PopupConfiguration popupConfig;
	public Vector2 topLeftOffset;

	void Start () {
		SpriteRenderer spr = GetComponent<SpriteRenderer>();
		spr.sprite = garbages[Random.Range (0, garbages.Length)];
	}


	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other){
		
		if (other.tag.Equals("WindArea", System.StringComparison.OrdinalIgnoreCase)){
			GarbagePhysics garbageScript = gameObject.AddComponent<GarbagePhysics>();
		
			garbageScript.FollowTransform(other.transform);
			return;
		}
		
		Fitz fitz = other.GetComponent<Fitz>();
		if (fitz != null){
			GameManager.gm.AddGarbage();
		}
		other.SendMessage("GarbagePickup");
		//gameObject.SetActive(false);
		makePopup ();

		Destroy(gameObject);
	}

	void makePopup(){
		Vector2 garbagePositionInScreen = ScreenUtils.getPositionInScreen (this.transform.position);
		PopupText popup = PopupFactory.makeLinearPopup (popupConfig, "+1", garbagePositionInScreen, ScreenUtils.getPositionFromTopRight(topLeftOffset), 0.3f, 0.95f);
		PopupSystem.AddPopup(popup);
	}
	
	void OnTriggerExit2D (Collider2D other){
		GarbagePhysics garbageScript = GetComponent<GarbagePhysics>();
		bool ahrah = other.tag == "WindArea";
		
		if (ahrah){
			garbageScript.Deactivate();
			
		}
	}
}
