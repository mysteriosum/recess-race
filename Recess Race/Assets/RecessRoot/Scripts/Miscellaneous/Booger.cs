using UnityEngine;
using System.Collections;

public enum BoogerSize {
	small = 2,
	medium = 5,
	large = 9
}

public class Booger : MonoBehaviour {
	
	public BoogerSize size = BoogerSize.small;
	
	private int damage;
	private Doc docScript;
	
	
	
	private float speed = 3.5f;
	
	private bool facingRight = true;
	public bool FacingRight {
		get { return facingRight; }
		set {
			facingRight = value;
			speed = facingRight? Mathf.Abs(speed) : -Mathf.Abs(speed);
			Debug.Log(value);
		}
	}
	
	
	private Transform t;
	private tk2dSprite sprite;
	
	// Use this for initialization
	void Start () {
		damage = (int) size;
		t = transform;
		
		sprite = GetComponent<tk2dSprite>();
		sprite.scale = new Vector3(facingRight? 1 : -1, 1, 1);
		Invoke("A_Splode", 5.5f);
	}
	
	// Update is called once per frame
	void Update () {
		t.Translate(speed, 0, 0);
		
	}
	
	void OnTriggerEnter(Collider other) {
		switch (other.gameObject.layer){
		case 28:
			Doc doc = other.GetComponent<Doc>();
			if (doc){
				doc.Hurt(damage);
			}
			Destroy(gameObject);
			break;
		}
	}
	
	private void A_Splode(){ 
		Destroy(gameObject);
	}
}
