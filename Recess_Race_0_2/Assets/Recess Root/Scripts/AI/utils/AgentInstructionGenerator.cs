using UnityEngine;
using System.Collections;

public class AgentInstructionGenerator {

    private static float RunToEpsilon = 0.2f;
    private static float JumpToEpsilon = 3f;

    private static Map map;


    public static Instruction findInstruction(Agent agent, Plateform from, Plateform to) {
        if (map == null) map = (Map) GameObject.FindObjectOfType<Map>();

        bool[,] pathing = map.split(from.getRightCornerPosition(), new Dimension(13, 7));
        // printBoolArray(pathing);
        Instruction instruction = null;

        if(distanceEgal(from,to,12) /*&& doesntInterfer(pathing, JumpPathingMaps.jump_x12_y0)*/){
            instruction = new RunToInstruction(agent, from.getRightCornerPosition(), RunToEpsilon);
			Direction direction = (to.transform.position.x > from.transform.position.x) ? Direction.left : Direction.right;
            Instruction jump = new JumpInstruction(agent, direction, JumpToEpsilon);
            instruction.nextInstruction = jump;
        }
        
        return instruction;
    }

    private static bool distanceEgal(Plateform from, Plateform to, int distance) {
        return Mathf.Abs(from.getRightCornerPosition().x - to.getLeftCornerPosition().x) == distance;
    }

    private static bool doesntInterfer(bool[,] pathing, bool[,] jumpPathing) {
        for (int i = 0; i < pathing.GetLength(0); i++) {
            for (int j = 0; j < pathing.GetLength(1); j++) {
                if (jumpPathing[i, j] == true && pathing[i,j] == true) {
                    return false;
                }
            }
        }
        return true;
    }


    private static void printBoolArray(bool[,] array) {
        int rowLength = array.GetLength(0);
        int colLength = array.GetLength(1);
        string line = "";

        for (int i = 0; i < rowLength; i++) {
            for (int j = 0; j < colLength; j++) {
                line += array[i, j] + ",";
            }
            Debug.Log(line);
            line = "";
        }
    }
}
