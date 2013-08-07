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
	
	private float speed = 2.5f;
	
	private bool facingRight = true;
	public bool FacingRight {
		get { return facingRight; }
		set {
			facingRight = value;
			sprite.scale = new Vector3(facingRight? 1 : -1, 1, 1);
			speed = facingRight? Mathf.Abs(speed) : -Mathf.Abs(speed);
		}
	}
	
	
	private Transform t;
	private tk2dSprite sprite;
	
	// Use this for initialization
	void Start () {
		damage = (int) size;
		speed = facingRight? speed : -speed;
		t = transform;
		
		sprite = GetComponent<tk2dSprite>();
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
}
