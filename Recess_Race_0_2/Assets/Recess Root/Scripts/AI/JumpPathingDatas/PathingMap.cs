using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PathingMap {

    public BoolArray[] pathingMap;

    public PathingMap(bool[,] map) {
       int width = map.GetLength(0);
       int height = map.GetLength(1);
       this.pathingMap = BoolArray.generateBoolArrayArray(width, height);
       for (int i = 0; i < width; i++) {
           for (int j = 0; j < height; j++) {
               pathingMap[i][j] = map[i, j];
           }
       }
    }

    public bool collideWith(bool[,] pathing) {
        for (int i = 0; i < this.pathingMap.Length; i++) {
            for (int j = 0; j < this.pathingMap[0].Length; j++) {
                if (pathingMap[i][j] == true && pathing[i, j] == true) {
                    return true;
                }
            }
        }
        return false;
    }
}   
