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
		
		Fitz fitz = other.GetComponent<Fitz>();
		if (fitz != null){
			RecessCamera.cam.AddGarbage();
			RecessCamera.cam.PlaySound(RecessCamera.cam.sounds.cameraSound);
		}
		//gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
