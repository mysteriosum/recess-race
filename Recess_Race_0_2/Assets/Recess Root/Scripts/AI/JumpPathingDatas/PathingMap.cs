using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

	public PathingMap getXRevertedMap(){
		bool[,] boolmap = new bool[pathingMap.Length,pathingMap[0].boolArray.Length];

		int x = this.pathingMap.Length - 1;
		foreach (var item in this.pathingMap) {
			int y = 0;
			foreach (bool collisionValue in item.boolArray) {
				boolmap[x,y++] = collisionValue;
			}
			x--;
		}

		return new PathingMap(boolmap);
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

	public bool collideWithFromRightSide(bool[,] pathing) {
		int thisLastIndex = this.pathingMap.Length - 1;
		int otherLastIndex = pathing.GetLength(0) -1;
		for (int x = 0; x < this.pathingMap.Length; x++) {
			for (int y = 0; y < this.pathingMap[0].Length; y++) {
				if (pathingMap[thisLastIndex - x][y] == true && pathing[otherLastIndex - x, y] == true) {
					return true;
				}
			}
		}
		return false;
	}

	public bool collideWith(SplitDirection direction, bool[,] pathing){
		switch (direction) {
		case SplitDirection.BottomLeft : return collideWithFromBottomLeft(pathing);
		case SplitDirection.BottomRight : return collideWithFromBottomRight(pathing);
		case SplitDirection.TopLeft : return collideWithFromTopLeft(pathing);
		case SplitDirection.TopRight : return collideWithFromTopRight(pathing);
		default: return false;
		}
	}

	public bool collideWithFromBottomLeft(bool[,] pathing) {
		for (int x = 0; x < this.pathingMap.Length; x++) {
			for (int y = 0; y < this.pathingMap[0].Length; y++) {
				if (pathingMap[x][y] == true && pathing[x, y] == true) {
					return true;
				}
			}
		}
		return false;
	}

	public bool collideWithFromBottomRight(bool[,] pathing) {
		int thisLastIndex = this.pathingMap.Length - 1;
		int otherLastIndex = pathing.GetLength(0) -1;
		for (int x = 0; x < this.pathingMap.Length; x++) {
			for (int y = 0; y < this.pathingMap[0].Length; y++) {
				if (pathingMap[thisLastIndex - x][y] == true && pathing[otherLastIndex - x, y] == true) {
					return true;
				}
			}
		}
		return false;
	}

	public bool collideWithFromTopRight(bool[,] pathing) {
		int thisLastIndex = this.pathingMap.Length - 1;
		int otherLastIndex = pathing.GetLength(0) -1;
		int thisLastIndexY = this.pathingMap[0].Length - 1;
		int otherLastIndexY = pathing.GetLength(1) -1;
		for (int x = 0; x < this.pathingMap.Length; x++) {
			for (int y = 0; y < this.pathingMap[0].Length; y++) {
				if (pathingMap[thisLastIndex - x][thisLastIndexY - y] == true && pathing[otherLastIndex - x, otherLastIndexY - y] == true) {
					return true;
				}
			}
		}
		return false;
	}

	public bool collideWithFromTopLeft(bool[,] pathing) {
		int thisLastIndexY = this.pathingMap[0].Length - 1;
		int otherLastIndexY = pathing.GetLength(1) -1;
		for (int x = 0; x < this.pathingMap.Length; x++) {
			for (int y = 0; y < this.pathingMap[0].Length; y++) {
				if (pathingMap[x][thisLastIndexY - y] == true && pathing[x, otherLastIndexY - y] == true) {
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
