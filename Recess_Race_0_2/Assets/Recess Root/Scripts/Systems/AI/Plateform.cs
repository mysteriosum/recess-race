using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Plateform : MonoBehaviour, IComparable<Plateform> {

	public List<LinksToPlateform> linksToPlateforms;

    public Color color;
    public int id;
	public int waypointId;
	public bool isLastWayPoint;
    public bool showGismos = true;
	
	public Color pointColor;
	public Vector3 pointLocation = Vector3.zero;
	public Vector3 pointToLocation = Vector3.zero;
	public bool pointArrowUp;
	public bool pointArrowRight;

	public bool isUnder(Plateform p2){
		Vector3 v1 = this.transform.position; 
		Vector3 v2 = p2.transform.position;
		
		return (v2.y > v1.y) || (v1.y == v2.y && (v2.x < v1.x));
	}

	public bool isLeftOf(Plateform p2){
		return this.transform.position.x < p2.transform.position.x;
	}

	public void showPoint(Vector3 from, Vector3 to,Color color, bool right, bool up){
		this.pointColor = color;
		this.pointLocation = from;
		this.pointToLocation = to;
		this.pointArrowUp = up;
		this.pointArrowRight = right;
	}

	public void hidePoint(){
		this.pointLocation = Vector3.zero;
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

		if (pointLocation != Vector3.zero) {
			Gizmos.color = pointColor;
			Gizmos.DrawSphere(pointLocation, 0.4f);
			int up = (pointArrowUp) ? 1 : -1;
			int right = (pointArrowRight) ? 1 : -1;
			Vector3 to = new Vector3(pointLocation.x + right, pointLocation.y + up, 0);
			Gizmos.DrawLine(pointLocation, pointToLocation);
			Gizmos.DrawLine(pointLocation, to);
		}

        foreach (var linked in linksToPlateforms) {
			foreach(var jump in linked.jumps){

				InstructionCreationData data = jump.instruction;
				if(data.type.Equals(InstructionCreationData.InstructionType.Jump)){
					Gizmos.color = new Color(1, 1, 0, 0.8f);
					int direction = (int)data.direction;
					float finalX = jump.startLocation.x +  direction * (data.moveHoldingLenght + data.totalDistanceAfterMoveAgain + 1);
					//float aproximativeJumpHeight = 3 + 2 * (data.jumpHoldingLenght / 13);

					Vector3 v3 = new Vector3(finalX, linked.plateform.transform.position.y, 0);
					Gizmos.DrawLine(jump.startLocation, v3);

				} else if( data.type.Equals(InstructionCreationData.InstructionType.Run)) {
					Gizmos.color = new Color(1, 0, 1, 0.8f);
					Vector3 v2 = new Vector3(jump.startLocation.x + data.moveHoldingLenght, jump.startLocation.y, 0);
					Gizmos.DrawLine(jump.startLocation, v2);
				} else if( data.type.Equals(InstructionCreationData.InstructionType.DropOff)){
					Gizmos.color = new Color(0, 1, 1, 0.8f);
					Vector3 v2 = new Vector3(jump.startLocation.x + ((float)jump.startingDirection), linked.plateform.transform.position.y, 0);
					Gizmos.DrawLine(jump.startLocation, v2);
				}

				Gizmos.DrawSphere(jump.startLocation, 0.15f);
			}
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

	public void addLinksToPlateform(Plateform toPlateform, Direction startingDirection, Vector3 startLocation, InstructionCreationData instruction){
		foreach(LinksToPlateform linked in this.linksToPlateforms){
			if(toPlateform.id == linked.plateform.id){
				linked.jumps.Add (new PreciseJumpConfig(startingDirection,startLocation, instruction));
				return;
			}
		}
		this.linksToPlateforms.Add (new LinksToPlateform(toPlateform));
	}

	public LinksToPlateform getLinksTo(Plateform to){
		foreach (var item in linksToPlateforms) {
			if(item.plateform.id == to.id){
				return item;
			}
		}
		return null;
	}
}

[Serializable]
public class LinksToPlateform{
	public Plateform plateform;
	public List<PreciseJumpConfig> jumps;

	public LinksToPlateform(Plateform toPlaform){
		this.plateform = toPlaform;
		jumps = new List<PreciseJumpConfig>();
	}
}

[Serializable]
public class PreciseJumpConfig{
	public Vector3 startLocation;
	public InstructionCreationData instruction;
	public Direction startingDirection;
	
	public PreciseJumpConfig(Direction startingDirection, Vector3 startLocation, InstructionCreationData instruction) {
		this.startLocation = startLocation;
		this.instruction = instruction;
		this.startingDirection = startingDirection;
	}
}