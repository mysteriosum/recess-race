using UnityEngine;
using System.Collections;

public class Garbage : MonoBehaviour {
	public Sprite[] garbages;
	// Use this for initialization
	void Start () {
		SpriteRenderer spr = GetComponent<SpriteRenderer>();
		spr.sprite = garbages[Random.Range (0, garbages.Length)];
	}
	
	// Update is called once per frame
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
			RecessCamera.cam.AddGarbage();
		}
		other.SendMessage("GarbagePickup");
		//gameObject.SetActive(false);
		Destroy(gameObject);
	}
	
	void OnTriggerExit2D (Collider2D other){
		GarbagePhysics garbageScript = GetComponent<GarbagePhysics>();
		bool ahrah = other.tag == "WindArea";
		
		if (ahrah){
			garbageScript.Deactivate();
			
		}
	}
}
