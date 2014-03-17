using System;
using UnityEngine;

[Serializable]
public class JumpRunCreationData {
	public InstructionCreationData instruction;
    public PathingMap jumpingPath;
	public Direction direction;

	public JumpRunCreationData(Direction direction, InstructionCreationData creationData, PathingMap jumpingPath) {
		this.instruction = creationData;
		this.direction = direction;
		this.jumpingPath = jumpingPath;
	}
}