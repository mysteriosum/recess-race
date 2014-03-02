﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class JumpRunCreationData {
    public float jumpHoldingLenght = 13;
    public float moveHoldingLenght = 13;
    public Direction direction;
    public PathingMap jumpingPath;

    public JumpRunCreationData(Direction direction, float jumpHoldingLenght, float moveHoldingLenght, PathingMap jumpingPath) {
        this.direction = direction;
        this.jumpHoldingLenght = jumpHoldingLenght;
        this.moveHoldingLenght = moveHoldingLenght;
        this.jumpingPath = jumpingPath;
    }

    public JumpRunCreationData(float jumpHoldingLenght, float moveHoldingLenght, PathingMap jumpingPath) {
        this.direction = Direction.right;
        this.jumpHoldingLenght = jumpHoldingLenght;
        this.moveHoldingLenght = moveHoldingLenght;
        this.jumpingPath = jumpingPath;

    }
}