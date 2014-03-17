using System.Collections.Generic;
using UnityEngine;

public class PossibleJumpMaps {

    public static List<JumpRunCreationData> getPossible(int x, int y) {
		if (Mathf.Abs(x) >= possibles.GetLength(0) || getIndexFromY(y) >= yHeight || getIndexFromY(y) < 0) {
            Debug.LogError("On a pas jusqua" + x + "," + y);
            return null;
        }

		if (x < 0) {
			return possiblesInverse[-x, getIndexFromY(y)];
		} else {
			return possibles[x,getIndexFromY(y)];
		}
        
    }

	public static int yUpHeightIncludingZero = 5;
	public static int yDownHeight = 16;
	private static int yHeight = 5 + 16;
    public static List<JumpRunCreationData>[,] possibles = new List<JumpRunCreationData>[14,yHeight];
	public static List<JumpRunCreationData>[,] possiblesInverse = new List<JumpRunCreationData>[14,yHeight];

    static PossibleJumpMaps() {
		InstructionCreationData creationData;
        
		List<JumpRunCreationData> list6_3 = new List<JumpRunCreationData>();
		creationData = makeRunJump (direction: Direction.right, runDistance : 2f, moveHoldingLenght: 1f, jumpHoldingLenght: 2f);
		list6_3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x6_y3));
		possibles[6, getIndexFromY(3)] = list6_3;


		generateReversedJumps ();

    }

	private static int getIndexFromY(int y){
		return y + yDownHeight;
	}

	private static void generateReversedJumps(){
		for (int x = 0; x < possibles.GetLength(0); x++) {
			for (int y = 0; y < possibles.GetLength(1); y++) {
				if(possibles[x,y] != null){
					List<JumpRunCreationData> inverseList = cloneInverseList(possibles[x,y]);
					possiblesInverse[x, y] = inverseList;
				}
			}	
		}
	}

	private static InstructionCreationData makeRunJump(Direction direction, float runDistance, float jumpHoldingLenght, float moveHoldingLenght){
		JumpInstruction.CreationData jump = new JumpInstruction.CreationData ()
			{startingDirection=direction, holdLenght= jumpHoldingLenght, moveLenght = moveHoldingLenght};
		RunToInstruction.CreationData run = new RunToInstruction.CreationData (){runDistance = runDistance};
		run.nextInstructionCreationData = jump;
		return run;
	}

	private static InstructionCreationData makeDropOff(Direction direction, float runDistance, float moveHoldingLenght, Direction endDirection, float moveAgainAfterYMoved, float totalDropOff){
		DropOffInstruction.CreationData dropOff = new DropOffInstruction.CreationData ()
		{firstDirection=direction, moveXLenght=moveHoldingLenght,moveAgainAfterYMoved=moveAgainAfterYMoved, endDropDirection=endDirection, totalDrop=totalDropOff};
		RunToInstruction.CreationData run = new RunToInstruction.CreationData (){runDistance = runDistance};
		run.nextInstructionCreationData = dropOff;
		return run;
	}

	private static List<JumpRunCreationData> cloneInverseList(List<JumpRunCreationData> toClone){
		List<JumpRunCreationData> cloned = new List<JumpRunCreationData> ();
		//foreach (JumpRunCreationData item in toClone) {
			//JumpRunCreationData newJump = new JumpRunCreationData(item.jump,Direction.left, item.jumpHoldingLenght, item.moveHoldingLenght, item.jumpingPath.getXRevertedMap());
			//cloned.Add (newJump);
		//}
		return cloned;
	}
}