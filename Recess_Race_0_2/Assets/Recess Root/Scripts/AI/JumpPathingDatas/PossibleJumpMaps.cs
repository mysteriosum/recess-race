using System.Collections.Generic;
using UnityEngine;

public class PossibleJumpMaps {

    public static List<JumpRunCreationData> getPossible(int x, int y) {
        if (x >= possibles.GetLength(0) || y >= possibles.GetLength(1)) {
            Debug.Log("On a pas jusqua" + x + "," + y);
            return null;
        }
        return possibles[x,y];
    }

    public static List<JumpRunCreationData>[,] possibles = new List<JumpRunCreationData>[14,6];
    
    static PossibleJumpMaps(){
        List<JumpRunCreationData> list13_0 = new List<JumpRunCreationData>();
        list13_0.Add(new JumpRunCreationData(13f, 13f, JumpPathingMaps.jump_x12_y0));
        possibles[13, 0] = list13_0;

        List<JumpRunCreationData> list4_0 = new List<JumpRunCreationData>();
        list4_0.Add(new JumpRunCreationData(0f, 2f, JumpPathingMaps.jump_x4_y0));
        possibles[4, 0] = list4_0;

        List<JumpRunCreationData> list7_0 = new List<JumpRunCreationData>();
        list7_0.Add(new JumpRunCreationData(0f, 5f, JumpPathingMaps.jump_x7_y0));
        possibles[7, 0] = list7_0;

        List<JumpRunCreationData> list5_1 = new List<JumpRunCreationData>();
        list5_1.Add(new JumpRunCreationData(0f, 5f, JumpPathingMaps.jump_x5_y1));
        possibles[5, 1] = list5_1;
    }
}