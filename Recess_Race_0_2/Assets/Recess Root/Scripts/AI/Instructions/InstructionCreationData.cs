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
}