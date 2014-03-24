using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Plateform : MonoBehaviour, IComparable<Plateform> {

    public List<LinkedPlateform> linkedJumpPlateform;

    public Color color;
    public int id;
	public int waypointId;
    public bool showGismos = true;

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
        if (!showGismos) return;

        foreach (var linked in linkedJumpPlateform) {
            if (linked == null || !linked.plateform) continue;
		
			InstructionCreationData data = linked.instruction;
			if(linked.instruction.type.Equals(InstructionCreationData.InstructionType.Jump)){
				Gizmos.color = new Color(1, 1, 0, 0.8f);
				int direction = (int)data.direction;
				float finalX = linked.startLocation.x +  direction * (data.moveHoldingLenght + data.totalDistanceAfterMoveAgain + 1);
				//float aproximativeJumpHeight = 3 + 2 * (data.jumpHoldingLenght / 13);

				Vector3 v3 = new Vector3(finalX, linked.plateform.transform.position.y, 0);
				Gizmos.DrawLine(linked.startLocation, v3);

			} else if( linked.instruction.type.Equals(InstructionCreationData.InstructionType.Run)) {
				Gizmos.color = new Color(1, 0, 1, 0.8f);
				Vector3 v2 = new Vector3(linked.startLocation.x + data.moveHoldingLenght, linked.startLocation.y, 0);
				Gizmos.DrawLine(linked.startLocation, v2);
			} else if(linked.instruction.type.Equals(InstructionCreationData.InstructionType.DropOff)){
				Gizmos.color = new Color(0, 1, 1, 0.8f);
				Gizmos.DrawLine(linked.startLocation, linked.plateform.transform.position);
			}

			Gizmos.DrawSphere(linked.startLocation, 0.15f);
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
public class LinkedPlateform {
    public Vector3 startLocation;
    public Plateform plateform;
	public InstructionCreationData instruction;
	public Direction startingDirection;

	public LinkedPlateform(Direction startingDirection, Vector3 startLocation, Plateform plateform, InstructionCreationData instruction) {
        this.startLocation = startLocation;
        this.plateform = plateform;
        this.instruction = instruction;
		this.startingDirection = startingDirection;
    }
}
