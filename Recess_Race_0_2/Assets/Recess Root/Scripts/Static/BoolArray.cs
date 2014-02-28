using UnityEngine;
using System.Collections;

[System.Serializable]
public class BoolArray  {

    public bool[] boolArray = new bool[0];

    public bool this[int index] {
        get {
            return boolArray[index];
        }

        set {
            boolArray[index] = value;
        }
    }

    public int Length {
        get {
            return boolArray.Length;
        }
    }

    public long LongLength {
        get {
            return boolArray.LongLength;
        }
    }

    public static BoolArray[] generateBoolArrayArray(int width, int height) {
        BoolArray[] array = new BoolArray[width];
        for (int i = 0; i < width; i++) {
            array[i] = new BoolArray();
            array[i].boolArray = new bool[height];
        }

        return array;
    }
}
