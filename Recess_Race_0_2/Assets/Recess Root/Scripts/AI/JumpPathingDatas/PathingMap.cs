using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class PathingMap {

    public BoolArray[] pathingMap;

    public PathingMap(bool invertedXY, bool[,] map) {
        
       int width = map.GetLength(1);
       int height = map.GetLength(0);
       if (invertedXY) {
           width = map.GetLength(0);
           height = map.GetLength(1);
       }
       this.pathingMap = BoolArray.generateBoolArrayArray(height, width);
       for (int x = 0; x < height; x++) {
           for (int y = 0; y < width; y++) {
               if (invertedXY) {
                   pathingMap[x][width - 1 - y] = map[y, x];
               } else {
                   pathingMap[x][y] = map[x, y];
               }
           }
       }
    }

    public PathingMap(bool[,] map) : this(false, map){
    }

    public bool collideWith(bool[,] pathing) {
        for (int x = 0; x < this.pathingMap.Length; x++) {
            for (int y = 0; y < this.pathingMap[0].Length; y++) {
                if (pathingMap[x][y] == true && pathing[x, y] == true) {
                    return true;
                }
            }
        }
        return false;
    }

    public string ToString() {
        string outStr = "";
        for (int y = pathingMap[0].boolArray.Length - 1; y >= 0 ; y--) {
            for (int x = 0; x < pathingMap.Length; x++) {
                outStr += pathingMap[x].boolArray[y] + ",";
            }
            outStr += "\n";
        }
       
        return outStr;
    }

    public string ToStringWithNumbers() {
        string outStr = "";
        for (int y = pathingMap[0].boolArray.Length - 1; y >= 0; y--) {
            for (int x = 0; x < pathingMap.Length; x++) {

                outStr += (pathingMap[x].boolArray[y]? "1" : "0") + ",";
            }
            outStr += "\n";
        }

        return outStr;
    }
}   
