using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Plateform : MonoBehaviour, IComparable<Plateform> {

	public List<Plateform> linkedPlateform;

    public int id;

    private static int nextId = 0;
    
    public Plateform(){
        id = nextId++;
    }


	void Start () {
	
	}
	

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

    public int CompareTo(Plateform other)
    {
        return this.id - other.id;
    }
}
