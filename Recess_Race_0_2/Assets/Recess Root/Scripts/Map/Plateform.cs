using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Plateform : MonoBehaviour, IComparable<Plateform> {

	public List<Plateform> linkedPlateform;

    public int id;
	public int waypointId;

	public bool isUnder(Plateform p2){
		Vector3 v1 = this.transform.position; 
		Vector3 v2 = p2.transform.position;
		
		return (v2.y > v1.y) || (v1.y == v2.y && (v2.x < v1.x));
	}

	public bool isLeftOf(Plateform p2){
		return this.transform.position.x < p2.transform.position.x;
	}

	public Bounds getBound(){
		return this.renderer.bounds;
	}

    public int CompareTo(Plateform other)
    {
        return this.id - other.id;
    }

	void OnDrawGizmos(){
		foreach (var plateform in linkedPlateform) {
			if(!plateform) continue;
			Vector3 v1,v2;
			if(this.isLeftOf(plateform)){
				v1 = this.getRightCornerPosition();
				v2 = plateform.getLeftCornerPosition();
			}else{
				v1 = this.getLeftCornerPosition();
				v2 = plateform.getRightCornerPosition();
			}	
			Gizmos.DrawLine (v1, v2);
		}
	}

	public Vector3 getRightCornerPosition(){
		return this.transform.position + this.transform.localScale/2 - new Vector3(0.5f,0.5f,0);
	}
	public Vector3 getLeftCornerPosition(){
		return this.transform.position - this.transform.localScale/2 + new Vector3(0.5f,0.5f,0);
	}
}
