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
			int direction = (int) linked.data.direction;
            float finalX;
            float r =0 , g =0, b=0;
            float aproximativeJumpHeight;
            if (linked.data.jump) {
                finalX = linked.jumpStart.x +  direction * (linked.data.moveHoldingLenght + 2 - (linked.data.moveHoldingLenght / 13));
                aproximativeJumpHeight = 3 + 2 * (linked.data.jumpHoldingLenght / 13);
                v2 = new Vector3((finalX + linked.jumpStart.x) / 2, linked.jumpStart.y + aproximativeJumpHeight, 0);
                v3 = new Vector3(finalX, linked.plateform.transform.position.y, 0);
                g = 1;
                r = (linked.data.moveHoldingLenght / 13);
            } else {
                finalX = linked.jumpStart.x + direction * (linked.data.moveHoldingLenght + 0.5f);
                aproximativeJumpHeight = 0;
                v2 = new Vector3((finalX + linked.jumpStart.x) / 2, linked.jumpStart.y + aproximativeJumpHeight, 0);
                v3 = new Vector3(finalX, linked.plateform.transform.position.y, 0);
                b = 1;
                r = 1;
            }

            Gizmos.color = new Color(r, g, b, 0.8f);
            Gizmos.DrawSphere(linked.jumpStart, 0.1f);
            Gizmos.color = new Color(r - 0.1f, g - 0.1f, b - 0.1f, 0.8f);
            Gizmos.DrawLine(linked.jumpStart, v2);
            Gizmos.DrawSphere(linked.jumpStart, 0.1f);
            Gizmos.color = new Color(r - 0.4f, g - 0.4f, b - 0.4f, 0.8f);
            Gizmos.DrawLine(v2, v3);
            Gizmos.DrawSphere(v2, 0.12f);
            Gizmos.color = new Color(r - 0.7f, g - 0.7f, b - 0.7f, 0.8f);
            Gizmos.DrawSphere(v3, 0.14f);
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
