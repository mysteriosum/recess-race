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
        
		List<JumpRunCreationData> list1_m1 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=1f, jumpHoldingLenght=10f, moveHoldingLenght=1f, needRunCharge=false};
		list1_m1.Add (new JumpRunCreationData (Direction.right, creationData, JumpPathingMaps.jump_x1_ym1));
		possibles[1, getIndexFromY(-1)] = list1_m1;

		List<JumpRunCreationData> list1_m2 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=2f, jumpHoldingLenght=10f, moveHoldingLenght=1f, needRunCharge=false};
		list1_m2.Add (new JumpRunCreationData (Direction.right, creationData, JumpPathingMaps.jump_x1_ym2));
		possibles[1, getIndexFromY(-2)] = list1_m2;

		List<JumpRunCreationData> list1_m3 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=3f, jumpHoldingLenght=10f, moveHoldingLenght=1f, needRunCharge=false};
		list1_m3.Add (new JumpRunCreationData (Direction.right, creationData, JumpPathingMaps.jump_x1_ym3));
		possibles[1, getIndexFromY(-3)] = list1_m3;

		List<JumpRunCreationData> list1_m4 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0.15f, endDirection=Direction.left, totalDistanceAfterMoveAgain=4f, jumpHoldingLenght=2.23f, moveHoldingLenght=0f, needRunCharge=true};
		list1_m4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x1_ym4_Boumerang));
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0.58f, endDirection=Direction.right, totalDistanceAfterMoveAgain=4f, jumpHoldingLenght=0f, moveHoldingLenght=0.85f, needRunCharge=false};
		list1_m4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x1_ym4));
		possibles[1, getIndexFromY(-4)] = list1_m4;

		List<JumpRunCreationData> list1_m5 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=5f, jumpHoldingLenght=10f, moveHoldingLenght=1f, needRunCharge=false};
		list1_m5.Add (new JumpRunCreationData (Direction.right, creationData, JumpPathingMaps.jump_x1_ym5));
		possibles[1, getIndexFromY(-5)] = list1_m5;

        List<JumpRunCreationData> list1_m6 = new List<JumpRunCreationData>();
        creationData = new InstructionCreationData() { type = InstructionCreationData.InstructionType.DropOff, direction = Direction.right, distanceToStartRunningAgain = 0.58f, endDirection = Direction.right, totalDistanceAfterMoveAgain = 6f, jumpHoldingLenght = 0f, moveHoldingLenght = 0.85f, needRunCharge = false };
        list1_m6.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x1_ym6));
        possibles[1, getIndexFromY(-6)] = list1_m6;

		List<JumpRunCreationData> list1_3 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=0f, moveHoldingLenght=1f, needRunCharge=false};
		list1_3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x1_y3));
		possibles[1, getIndexFromY(3)] = list1_3;

		List<JumpRunCreationData> list1_4 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =3f, endDirection=Direction.right, totalDistanceAfterMoveAgain=1f, jumpHoldingLenght=4f, moveHoldingLenght=0f, needRunCharge=false};
		list1_4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x1_y4_noRun));
		possibles[1, getIndexFromY(4)] = list1_4;

		List<JumpRunCreationData> list2_1 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =5f, endDirection=Direction.right, totalDistanceAfterMoveAgain=2f, jumpHoldingLenght=0f, moveHoldingLenght=0f, needRunCharge=true};
		list2_1.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_y1));
		possibles[2, getIndexFromY(1)] = list2_1;

		List<JumpRunCreationData> list2_2 = new List<JumpRunCreationData>();
        creationData = new InstructionCreationData() { type = InstructionCreationData.InstructionType.Jump, direction = Direction.right, distanceToStartRunningAgain = 2f, endDirection = Direction.right, totalDistanceAfterMoveAgain = 1.8f, jumpHoldingLenght = 0f, moveHoldingLenght = 0f, needRunCharge = false };
		list2_2.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_y2));
		possibles[2, getIndexFromY(2)] = list2_2;


        List<JumpRunCreationData> list2_3 = new List<JumpRunCreationData>();
        creationData =new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =2f, endDirection=Direction.right, totalDistanceAfterMoveAgain=1.8f, jumpHoldingLenght=0.2f, moveHoldingLenght=0f, needRunCharge=false};
        list2_3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_y3));
        possibles[2, getIndexFromY(3)] = list2_3;

        List<JumpRunCreationData> list2_4 = new List<JumpRunCreationData>();
        creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =2f, endDirection=Direction.right, totalDistanceAfterMoveAgain=1.8f, jumpHoldingLenght=0.38f, moveHoldingLenght=0f, needRunCharge=false};
        list2_4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_y4));
        possibles[2, getIndexFromY(4)] = list2_4;

        List<JumpRunCreationData> list2_m3 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =1f, endDirection=Direction.right, totalDistanceAfterMoveAgain=3f, jumpHoldingLenght=1.72f, moveHoldingLenght=0.5f, needRunCharge=true};
		list2_m3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym3));
		possibles[2, getIndexFromY(-3)] = list2_m3;

		List<JumpRunCreationData> list2_m4 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =1f, endDirection=Direction.right, totalDistanceAfterMoveAgain=4f, jumpHoldingLenght=1.72f, moveHoldingLenght=0.5f, needRunCharge=true};
		list2_m4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym4));

		possibles[2, getIndexFromY(-4)] = list2_m4;

		List<JumpRunCreationData> list2_m7 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =1f, endDirection=Direction.right, totalDistanceAfterMoveAgain=7f, jumpHoldingLenght=1.72f, moveHoldingLenght=0.5f, needRunCharge=true};
		list2_m7.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym7));
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=7f, jumpHoldingLenght=3f, moveHoldingLenght=1f, needRunCharge=false};
		list2_m7.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym7));
		possibles[2, getIndexFromY(-7)] = list2_m7;

		List<JumpRunCreationData> list2_m8 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =1f, endDirection=Direction.right, totalDistanceAfterMoveAgain=8f, jumpHoldingLenght=1.72f, moveHoldingLenght=0.5f, needRunCharge=true};
		list2_m8.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym8));
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=8f, jumpHoldingLenght=3f, moveHoldingLenght=1f, needRunCharge=false};
		list2_m8.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym8));
		possibles[2, getIndexFromY(-8)] = list2_m8;

		List<JumpRunCreationData> list2_m10 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=10f, jumpHoldingLenght=3f, moveHoldingLenght=0.25f, needRunCharge=true};
		list2_m10.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym10));
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=10f, jumpHoldingLenght=3f, moveHoldingLenght=1f, needRunCharge=false};
		list2_m10.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym10));
		possibles[2, getIndexFromY(-10)] = list2_m10;

		List<JumpRunCreationData> list2_m11 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=11f, jumpHoldingLenght=3f, moveHoldingLenght=0.25f, needRunCharge=true};
		list2_m11.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym11));
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=11f, jumpHoldingLenght=3f, moveHoldingLenght=1f, needRunCharge=false};
		list2_m11.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym11));
		possibles[2, getIndexFromY(-11)] = list2_m11;

		List<JumpRunCreationData> list2_m12 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =1f, endDirection=Direction.right, totalDistanceAfterMoveAgain=12f, jumpHoldingLenght=1.72f, moveHoldingLenght=0.5f, needRunCharge=true};
		list2_m12.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym12));
		possibles[2, getIndexFromY(-12)] = list2_m12;
		
		List<JumpRunCreationData> list2_m14 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =1f, endDirection=Direction.right, totalDistanceAfterMoveAgain=14f, jumpHoldingLenght=1.72f, moveHoldingLenght=0.5f, needRunCharge=true};
		list2_m14.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym14));
		possibles[2, getIndexFromY(-14)] = list2_m14;
		
		List<JumpRunCreationData> list2_m16 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =1f, endDirection=Direction.right, totalDistanceAfterMoveAgain=16f, jumpHoldingLenght=1.72f, moveHoldingLenght=0.5f, needRunCharge=true};
		list2_m16.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x2_ym16));
		possibles[2, getIndexFromY(-16)] = list2_m16;

		List<JumpRunCreationData> list3_m1 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=3f, moveHoldingLenght=3f, needRunCharge=true};
		list3_m1.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x3_ym1));
		possibles[3, getIndexFromY(-1)] = list3_m1;

		List<JumpRunCreationData> list3_m2 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0.16f, endDirection=Direction.right, totalDistanceAfterMoveAgain=1.28f, jumpHoldingLenght=3f, moveHoldingLenght=0.5f, needRunCharge=true};
		list3_m2.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x3_ym2));
		possibles[3, getIndexFromY(-2)] = list3_m2;

		List<JumpRunCreationData> list4_m3 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =4f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=10f, moveHoldingLenght=3f, needRunCharge=true};
		list4_m3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x4_ym3));
		possibles[4, getIndexFromY(-3)] = list4_m3;

		List<JumpRunCreationData> list4_1 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=0f, moveHoldingLenght=4f, needRunCharge=false};
		list4_1.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x4_y1_noRun));
		possibles[4, getIndexFromY(1)] = list4_1;

		List<JumpRunCreationData> list4_2 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=0f, moveHoldingLenght=4f, needRunCharge=false};
		list4_2.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x4_y2_norun));
		possibles[4, getIndexFromY(2)] = list4_2;

		List<JumpRunCreationData> list4_3 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=1f, moveHoldingLenght=3f, needRunCharge=false};
		list4_3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x4_y3));
		possibles[4, getIndexFromY(3)] = list4_3;

		List<JumpRunCreationData> list4_4 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=1.72f, moveHoldingLenght=3.3f, needRunCharge=false};
		list4_4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x4_y4));
		possibles[4, getIndexFromY(4)] = list4_4;
		
		List<JumpRunCreationData> list5_m2 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=3f, moveHoldingLenght=3f, needRunCharge=true};
		list5_m2.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x5_ym2));
		possibles[5, getIndexFromY(-2)] = list5_m2;

		List<JumpRunCreationData> list5_2 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=0.6f, moveHoldingLenght=5f, needRunCharge=false};
		list5_2.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x5_y2));
		possibles[5, getIndexFromY(2)] = list5_2;
	
		List<JumpRunCreationData> list5_3 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=3f, moveHoldingLenght=3f, needRunCharge=false};
		list5_3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x5_y3));
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =2.94f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0.28f, jumpHoldingLenght=0.19f, moveHoldingLenght=1f, needRunCharge=true};;
		list5_3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x5_y3_loopedBetter));
		possibles[5, getIndexFromY(3)] = list5_3;


		List<JumpRunCreationData> list7_m1 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =2.5f, endDirection=Direction.right, totalDistanceAfterMoveAgain=5f, jumpHoldingLenght=0f, moveHoldingLenght=0f, needRunCharge=true};
		list7_m1.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_ym1));
		possibles[7, getIndexFromY(-1)] = list7_m1;

		List<JumpRunCreationData> list8_4 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =3.5f, endDirection=Direction.right, totalDistanceAfterMoveAgain=3.58f, jumpHoldingLenght=3f, moveHoldingLenght=2.09f, needRunCharge=true};
		list8_4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x8_y4));
		possibles[8, getIndexFromY(4)] = list8_4;

		List<JumpRunCreationData> list7_0 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =1.75f, endDirection=Direction.right, totalDistanceAfterMoveAgain=4.54f, jumpHoldingLenght=0f, moveHoldingLenght=0f, needRunCharge=true};
		list7_0.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_y0));
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=0f, moveHoldingLenght=13f, needRunCharge=true};
		list7_0.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_y0_headbump));
		possibles[7, getIndexFromY(0)] = list7_0;

		List<JumpRunCreationData> list7_2 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =3.5f, endDirection=Direction.right, totalDistanceAfterMoveAgain=2.5f, jumpHoldingLenght=2f, moveHoldingLenght=1.5f, needRunCharge=true};
		list7_2.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_y2));
		possibles[7, getIndexFromY(2)] = list7_2;

		List<JumpRunCreationData> list7_3 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=0f, moveHoldingLenght=5.5f, needRunCharge=true};
		list7_3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_y3));
		possibles[7, getIndexFromY(3)] = list7_3;

		List<JumpRunCreationData> list7_4 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=2.23f, moveHoldingLenght=6.51f, needRunCharge=true};
		list7_4.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x7_y4));
		possibles[7, getIndexFromY(4)] = list7_4;

		List<JumpRunCreationData> list8_0 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=8f, moveHoldingLenght=8f, needRunCharge=false};
		list8_0.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x8_y0_noRun));
		possibles[8, getIndexFromY(0)] = list8_0;

		List<JumpRunCreationData> list10_3 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=10f, moveHoldingLenght=10f, needRunCharge=true};
		list10_3.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x10_y3));
		possibles[10, getIndexFromY(3)] = list10_3;

		List<JumpRunCreationData> list11_0 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=4.2f, moveHoldingLenght=10.5f, needRunCharge=true};
		list11_0.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x11_y0));
		possibles[11, getIndexFromY(0)] = list11_0;

		List<JumpRunCreationData> list12_0 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=8f, moveHoldingLenght=12f, needRunCharge=true};
		list12_0.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x12_y0));
		possibles[12, getIndexFromY(0)] = list12_0;

		List<JumpRunCreationData> list12_1 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=13f, moveHoldingLenght=13f, needRunCharge=true};
		list12_1.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x12_y1));
		possibles[12, getIndexFromY(1)] = list12_1;

		List<JumpRunCreationData> list13_0 = new List<JumpRunCreationData>();
		creationData = new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction.right, distanceToStartRunningAgain =0f, endDirection=Direction.right, totalDistanceAfterMoveAgain=0f, jumpHoldingLenght=13f, moveHoldingLenght=13f, needRunCharge=true};
		list13_0.Add(new JumpRunCreationData(Direction.right, creationData, JumpPathingMaps.jump_x13_y0));
		possibles[13, getIndexFromY(0)] = list13_0;



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
			JumpRunCreationData newJump = new JumpRunCreationData(Direction.left, data, item.jumpingPath.getXRevertedMap());
			cloned.Add (newJump);
		}
		return cloned;
	}
}