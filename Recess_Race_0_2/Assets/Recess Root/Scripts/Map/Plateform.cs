using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plateform : MonoBehaviour {

	public List<Plateform> linkedPlateform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isUnder(Plateform p2){
		Vector3 v1 = this.transform.position; 
		Vector3 v2 = p2.transform.position;
		
		return (v2.y > v1.y) || (v1.y == v2.y && (v2.x < v1.x));
	}

	public Bounds getBound(){
		return this.renderer.bounds;
		//BoxCollider2D b = this.GetComponent<BoxCollider2D> ();
		//return b.renderer.bounds;
	}
}
