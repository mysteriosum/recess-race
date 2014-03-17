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

			Vector3 v2,v3;

			if(linked.instruction is JumpInstruction.CreationData){
				JumpInstruction.CreationData jump = (JumpInstruction.CreationData) linked.instruction;
				Gizmos.color = new Color(1, 0, 0, 0.8f);
				int direction = (int)jump.startingDirection;
				float finalX = linked.startLocation.x +  direction * (jump.moveLenght);
				float aproximativeJumpHeight = 3 + 2 * (jump.holdLenght / 13);

				v2 = new Vector3((finalX + linked.startLocation.x) / 2, linked.startLocation.y + aproximativeJumpHeight, 0);
				v3 = new Vector3(finalX, linked.plateform.transform.position.y, 0);
				Gizmos.DrawLine(linked.startLocation, v2);
				Gizmos.DrawLine(v2, v3);

			} else if( linked.instruction is RunToInstruction.CreationData) {
				RunToInstruction.CreationData run = (RunToInstruction.CreationData) linked.instruction;
				Gizmos.color = new Color(1, 1, 0, 0.8f);

				v2 = new Vector3(linked.startLocation.x + run.runDistance, linked.startLocation.y, 0);
				Gizmos.DrawLine(linked.startLocation, v2);
			}

			Gizmos.DrawSphere(linked.startLocation, 0.1f);
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
