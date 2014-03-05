using System;
using UnityEngine;

[Serializable]
public class JumpRunCreationData {
	public bool jump;
    public float jumpHoldingLenght = 13;
    public float moveHoldingLenght = 13;
    public Direction direction;
    public PathingMap jumpingPath;

	public JumpRunCreationData(bool jump, Direction direction, float jumpHoldingLenght, float moveHoldingLenght, PathingMap jumpingPath) {
		this.jump = jump;
		this.direction = direction;
		this.jumpHoldingLenght = jumpHoldingLenght;
		this.moveHoldingLenght = moveHoldingLenght;
		this.jumpingPath = jumpingPath;
	}

    public JumpRunCreationData(Direction direction, float jumpHoldingLenght, float moveHoldingLenght, PathingMap jumpingPath) {
		this.jump = true;
		this.direction = direction;
        this.jumpHoldingLenght = jumpHoldingLenght;
        this.moveHoldingLenght = moveHoldingLenght;
        this.jumpingPath = jumpingPath;
    }

    public JumpRunCreationData(float jumpHoldingLenght, float moveHoldingLenght, PathingMap jumpingPath) {
		this.jump = true;
		this.direction = Direction.right;
        this.jumpHoldingLenght = jumpHoldingLenght;
        this.moveHoldingLenght = moveHoldingLenght;
        this.jumpingPath = jumpingPath;

    }
}