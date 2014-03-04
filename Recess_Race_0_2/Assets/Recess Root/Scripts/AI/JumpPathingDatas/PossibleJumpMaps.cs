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

	public static int yUpHeightIncludingZero = 4;
	public static int yDownHeight = 7;
	private static int yHeight = 4 + 7;
    public static List<JumpRunCreationData>[,] possibles = new List<JumpRunCreationData>[14,yHeight];
	public static List<JumpRunCreationData>[,] possiblesInverse = new List<JumpRunCreationData>[14,yHeight];
    
    static PossibleJumpMaps(){
		List<JumpRunCreationData> list2_m4 = new List<JumpRunCreationData>();
		list2_m4.Add(new JumpRunCreationData(13f, 13f, JumpPathingMaps.jump_x2_yMinus4));
		possibles[2, getIndexFromY(-4)] = list2_m4;

		List<JumpRunCreationData> list13_0 = new List<JumpRunCreationData>();
        list13_0.Add(new JumpRunCreationData(13f, 13f, JumpPathingMaps.jump_x12_y0));
		possibles[13, getIndexFromY(0)] = list13_0;

        List<JumpRunCreationData> list4_0 = new List<JumpRunCreationData>();
        list4_0.Add(new JumpRunCreationData(0f, 2f, JumpPathingMaps.jump_x4_y0));
		possibles[4, getIndexFromY(0)] = list4_0;

        List<JumpRunCreationData> list7_0 = new List<JumpRunCreationData>();
        list7_0.Add(new JumpRunCreationData(0f, 5f, JumpPathingMaps.jump_x7_y0));
		possibles[7, getIndexFromY(0)] = list7_0;

        List<JumpRunCreationData> list5_1 = new List<JumpRunCreationData>();
        list5_1.Add(new JumpRunCreationData(0f, 2.8f, JumpPathingMaps.jump_x5_y1));
		possibles[5, getIndexFromY(1)] = list5_1;

		List<JumpRunCreationData> list12_1 = new List<JumpRunCreationData>();
		list12_1.Add(new JumpRunCreationData(13f, 13f, JumpPathingMaps.jump_x12_y1));
		possibles[12, getIndexFromY(1)] = list12_1;

        List<JumpRunCreationData> list5_2 = new List<JumpRunCreationData>();
        list5_2.Add(new JumpRunCreationData(0f, 2.5f, JumpPathingMaps.jump_x5_y2));
		possibles[5, getIndexFromY(2)] = list5_2;

        List<JumpRunCreationData> list6_2 = new List<JumpRunCreationData>();
        list6_2.Add(new JumpRunCreationData(0f, 3.5f, JumpPathingMaps.jump_x6_y2));
		possibles[6, getIndexFromY(2)] = list6_2;

		List<JumpRunCreationData> list9_2 = new List<JumpRunCreationData>();
		list9_2.Add(new JumpRunCreationData(3f, 8f, JumpPathingMaps.jump_x9_y2));
		possibles[9, getIndexFromY(2)] = list9_2;

		List<JumpRunCreationData> list10_2 = new List<JumpRunCreationData>();
		list10_2.Add(new JumpRunCreationData(3.5f, 9f, JumpPathingMaps.jump_x10_y2));
		possibles[10, getIndexFromY(2)] = list10_2;

		List<JumpRunCreationData> list11_2 = new List<JumpRunCreationData>();
		list11_2.Add(new JumpRunCreationData(7f, 10f, JumpPathingMaps.jump_x11_y2));
		possibles[11, getIndexFromY(2)] = list11_2;

		List<JumpRunCreationData> list6_3 = new List<JumpRunCreationData>();
		list6_3.Add(new JumpRunCreationData(1f, 4f, JumpPathingMaps.jump_x6_y3));
		possibles[6, getIndexFromY(3)] = list6_3;

		List<JumpRunCreationData> list8_4 = new List<JumpRunCreationData>();
		list8_4.Add(new JumpRunCreationData(5f, 6f, JumpPathingMaps.jump_x8_y4));
		possibles[8, getIndexFromY(4)] = list8_4;

		List<JumpRunCreationData> list9_4 = new List<JumpRunCreationData>();
		list9_4.Add(new JumpRunCreationData(13f, 10f, JumpPathingMaps.jump_x9_y4));
		possibles[9, getIndexFromY(4)] = list9_4;

		for (int x = 0; x < possibles.GetLength(0); x++) {
			for (int y = 0; y < possibles.GetLength(1); y++) {
				if(possibles[x,y] != null){
					List<JumpRunCreationData> inverseList = cloneInverseList(possibles[x,y]);
					possiblesInverse[x, y] = inverseList;
				}
			}	
		}

    }

	private static int getIndexFromY(int y){
		return y + yDownHeight;
	}

	private static List<JumpRunCreationData> cloneInverseList(List<JumpRunCreationData> toClone){
		List<JumpRunCreationData> cloned = new List<JumpRunCreationData> ();
		foreach (JumpRunCreationData item in toClone) {
			Debug.Log(item.jumpingPath.getXRevertedMap().ToStringWithNumbers());
			JumpRunCreationData newJump = new JumpRunCreationData(Direction.left, item.jumpHoldingLenght, item.moveHoldingLenght, item.jumpingPath.getXRevertedMap());
			cloned.Add (newJump);
		}
		return cloned;
	}
}