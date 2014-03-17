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
        
		List<JumpRunCreationData> list7_0 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =1.75f, endDirection=Direction.right, totalDistanceAfterMoveAgain=4.54f, jumpHoldingLenght=0f, moveHoldingLenght=0f, needRunCharge=true};
		list7_0.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_y0));
		possibles[7, getIndexFromY(0)] = list7_0;

		List<JumpRunCreationData> list7_4 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=2.23f, moveHoldingLenght=6.51f, needRunCharge=true};
		list7_4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_y4));
		possibles[7, getIndexFromY(4)] = list7_4;

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

	private static List<JumpRunCreationData> cloneInverseList(List<JumpRunCreationData> toClone){
		List<JumpRunCreationData> cloned = new List<JumpRunCreationData> ();
		//foreach (JumpRunCreationData item in toClone) {
			//JumpRunCreationData newJump = new JumpRunCreationData(item.jump,Direction.left, item.jumpHoldingLenght, item.moveHoldingLenght, item.jumpingPath.getXRevertedMap());
			//cloned.Add (newJump);
		//}
		return cloned;
	}
}