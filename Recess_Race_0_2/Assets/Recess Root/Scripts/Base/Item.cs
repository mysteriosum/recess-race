using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	protected SpriteRenderer sprite;
	
	protected virtual string MethodName{
		get { return "ChangeToFitz"; }
	}
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D (Collider2D other){	//TEMP solution
		if (other.GetComponent<Fitz>()){
			other.gameObject.SendMessage(MethodName);
			Destroy(gameObject);
		}
	}
	
	
	public Sprite GetSprite(){
		if (!sprite)
			sprite = GetComponent<SpriteRenderer>();
		return sprite.sprite;
	}
	
	public void Activate(){
		Fitz.fitz.SendMessage(MethodName);
	}
}
