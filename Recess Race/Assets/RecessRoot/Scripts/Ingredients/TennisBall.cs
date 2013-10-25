using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TennisBall : MonoBehaviour {
	
	public float speed;
	private Vector2 velocity;
	private Rigidbody rb;
	private Transform t;
	private bool hasCollid = false;
	private bool hasGravity = false;
	private float gravity = 0.1f;
	private bool disappearing = false;
	private int timer = 0;
	private Renderer r;
	private List<Vector3> collidedAt = new List<Vector3>();
	
	
	// Use this for initialization
	void Start () {
		t = transform;
		rb = GetComponent<Rigidbody>();
		r = renderer;
		velocity = new Vector2(speed * Mathf.Cos(t.rotation.eulerAngles.z * Mathf.Deg2Rad), speed * Mathf.Sin(t.rotation.eulerAngles.z * Mathf.Deg2Rad));
		t.rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
		t.Translate(velocity, Space.Self);
		
		if (hasGravity){
			if (!disappearing){
				velocity -= new Vector2(0, gravity);
				
			}
			else{
				r.enabled = Fitz.blinkArray[timer];
				timer ++;
				if (timer == Fitz.blinkArray.Length){
					Destroy(gameObject);
				}
			}
			
		}
		/*
		//shoot some raycasts
		for (int i = 0; i < 4; i ++){
			RaycastHit hit;
			bool line = 
				Physics.Linecast(
					t.position + new Vector3(Mathf.Sin(i * 90 * Mathf.Deg2Rad) * collider.bounds.size.x / 2, Mathf.Cos(i * 90 * Mathf.Deg2Rad) * collider.bounds.size.y / 2, 
					t.position + new Vector3(Mathf.Cos(i * 90 * Mathf.Deg2Rad) * (Mathf.Abs(velocity.x) + collider.bounds.size.x / 2), 
											Mathf.Sin(i * 90 * Mathf.Deg2Rad) * (Mathf.Abs(velocity.y) + collider.bounds.size.x / 2), 0), 
					out hit
			);
			
			if (line){
				Debug.Log("velocity is " + velocity + " and normal is " + hit.normal);
				velocity = VectorFunctions.Bounce(velocity, hit.normal);
				break;
			}
		}*/
	}
	
	void LateUpdate () {
		collidedAt.Clear();
	}
	
	void OnCollisionEnter (Collision other){
		if (collidedAt.Contains(other.contacts[0].normal)) return;
		velocity = VectorFunctions.Bounce(velocity, other.contacts[0].normal);
		
		collidedAt.Add(other.contacts[0].normal);
		
		if (other.gameObject.GetComponent<Fitz>() != null){
			hasGravity = true;
		}
		
		if (hasGravity){
			velocity -= new Vector2(velocity.x / 6, velocity.y / 3);
			if (velocity.y < gravity * 3 && velocity.y > -gravity * 3){
				disappearing = true;
				velocity = new Vector2(velocity.x, 0);
			}
		}
		
		
	}
	
}
