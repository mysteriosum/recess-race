using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	private int rayOffset = 6;
	private float rayLength;
	private int rayNumber;
	
	private Transform t;
	private tk2dSprite sprite;
	
	// Use this for initialization
	void Start () {
		damage = (int) size;
		t = transform;
		rayNumber = (int) collider.bounds.size.y/rayOffset;
		rayLength = collider.bounds.size.x/2 + Mathf.Abs(speed);
		Debug.Log("Ray stuff is: Offset " + rayOffset + " and length " + rayLength);
		
		sprite = GetComponent<tk2dSprite>();
		sprite.scale = new Vector3(facingRight? 1 : -1, 1, 1);
		Invoke("A_Splode", 5.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
		//raycasts
		List<Ray> rays = new List<Ray>();
		for (int i = 0; i <= rayNumber; i ++){
			Vector3 v3_rayOffset = i == rayNumber? Vector3.down * collider.bounds.size.y/2 : Vector3.up * collider.bounds.size.y/2 + Vector3.down * i * rayOffset;
			rays.Add(new Ray(t.position + v3_rayOffset, t.right * speed));
		}
		
		RaycastHit hit;
		bool hitSomething;
		
		foreach (Ray ray in rays){
			hitSomething = Physics.Raycast(ray, out hit, rayLength);
			if (hitSomething){
				bool cont = HitSomething(hit.collider);
				
				if (cont)
					hitSomething = false;
				
				else
					return;
				
			}
		}
		
		
		t.Translate(speed, 0, 0);
		
	}
	
	private bool HitSomething(Collider other) {
		
		if (other.gameObject.layer == LayerMask.NameToLayer("Default") 
			|| other.gameObject.layer == LayerMask.NameToLayer("softBottom") 
			|| other.gameObject.layer == LayerMask.NameToLayer("softTop")) return true;
		
		if (other.gameObject.layer == RecessManager.Instance.docLayer){
			Doc doc = other.GetComponent<Doc>();
			if (doc){
				doc.Hurt(damage);
			}
			Destroy(gameObject);
		}
		else if (other.gameObject.layer == RecessManager.Instance.brickLayer){
				
			BrickBlock brick = other.GetComponent<BrickBlock>();
			if (brick){
				brick.SendMessage(size == BoogerSize.small? "Crack" : "Explode");
				
			}
		}
		
		if (size != BoogerSize.large){
			Destroy(gameObject);
			return false;
		}
		else{
			return true;
		}
	}
	
	
	private void A_Splode(){ 
		Destroy(gameObject);
	}
}
