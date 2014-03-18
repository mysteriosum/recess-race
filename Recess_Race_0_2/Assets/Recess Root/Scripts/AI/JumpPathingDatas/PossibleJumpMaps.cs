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
        
		List<JumpRunCreationData> list1_m4 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0.15f, endDirection=Direction.left, totalDistanceAfterMoveAgain=4f, jumpHoldingLenght=2.23f, moveHoldingLenght=0f, needRunCharge=true};
		list1_m4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x1_ym4));
		possibles[1, getIndexFromY(-4)] = list1_m4;

		List<JumpRunCreationData> list5_2 = new List<JumpRunCreationData>();
		creationData =new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =2.38f, endDirection=Direction.right, totalDistanceAfterMoveAgain=3.03f, jumpHoldingLenght=0f, moveHoldingLenght=0f, needRunCharge=true};
		list5_2.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x5_y2));
		possibles[5, getIndexFromY(2)] = list5_2;



		List<JumpRunCreationData> list7_m1 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =2.5f, endDirection=Direction.right, totalDistanceAfterMoveAgain=5f, jumpHoldingLenght=0f, moveHoldingLenght=0f, needRunCharge=true};
		list7_m1.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_ym1));
		possibles[7, getIndexFromY(-1)] = list7_m1;



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
		foreach (JumpRunCreationData item in toClone) {
			InstructionCreationData data = item.instruction.cloneRevesedDirections();
			JumpRunCreationData newJump = new JumpRunCreationData(item.direction, data, item.jumpingPath);
			cloned.Add (newJump);
		}
		return cloned;
	}
}