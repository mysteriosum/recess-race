using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Plateform : MonoBehaviour, IComparable<Plateform> {

    public List<LinkedJumpPlateform> linkedJumpPlateform;

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
        foreach (var linked in linkedJumpPlateform) {
            if (linked == null || !linked.plateform) continue;
			Vector3 v2,v3;
            float finalX = linked.jumpStart.x + linked.data.moveHoldingLenght + 2 - (linked.data.moveHoldingLenght / 13);
            float aproximativeJumpHeight = 3 + 2* (linked.data.jumpHoldingLenght/13);
            v2 = new Vector3((finalX + linked.jumpStart.x) / 2, linked.jumpStart.y + aproximativeJumpHeight, 0);
            v3 = new Vector3(finalX, linked.plateform.transform.position.y, 0);

            Gizmos.color = Color.green;
			Gizmos.DrawLine (linked.jumpStart, v2);
            Gizmos.DrawLine(v2, v3);
		}
	}

	public Vector3 getRightCornerPosition(){
		return this.transform.position + this.transform.localScale/2 - new Vector3(0.5f,0.5f,0);
	}
	public Vector3 getLeftCornerPosition(){
		return this.transform.position - this.transform.localScale/2 + new Vector3(0.5f,0.5f,0);
	}

    public int getWidth() {
        return (int)this.transform.localScale.x;
    }
}

[Serializable]
public class LinkedJumpPlateform {
    public Vector3 jumpStart;
    public Plateform plateform;
    public JumpRunCreationData data;

    public LinkedJumpPlateform(Vector3 jumpStart, Plateform plateform, JumpRunCreationData data) {
        this.jumpStart = jumpStart;
        this.plateform = plateform;
        this.data = data;
    }
}
