using UnityEngine;
using System.Collections;

[System.Serializable]
public class InstructionCreationData{

	public enum InstructionType{Run, Jump, DropOff}
	public InstructionType type;
	public bool needRunCharge = false;

	public float jumpHoldingLenght;
	public float moveHoldingLenght;
	public Direction direction = Direction.right;
	
	public float distanceToStartRunningAgain;
	public Direction endDirection = Direction.right;
	public float totalDistanceAfterMoveAgain;

	public InstructionCreationData cloneRevesedDirections(){
		InstructionCreationData newI = new InstructionCreationData ();
		newI.type = type;
		newI.needRunCharge = needRunCharge;
		newI.jumpHoldingLenght = jumpHoldingLenght;
		newI.moveHoldingLenght = moveHoldingLenght;
		newI.direction = (direction.Equals (Direction.left)) ? Direction.right : Direction.left;
		newI.distanceToStartRunningAgain = distanceToStartRunningAgain;
		newI.endDirection = (endDirection.Equals (Direction.left)) ? Direction.right : Direction.left;
		newI.totalDistanceAfterMoveAgain = totalDistanceAfterMoveAgain;
		return newI;
	}
}