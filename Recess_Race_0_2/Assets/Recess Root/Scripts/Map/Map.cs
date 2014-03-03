using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public Dimension mapDimension;
    public Dimension tileDimension;
    public BoolArray[] pathingMap;

    public bool[,] split(Vector3 startingPosition, Dimension dimension) {
        return split((int)startingPosition.x, (int)startingPosition.y, dimension);
    }

    public bool[,] split(int startX, int startY, Dimension dimension) {
        bool[,] array = new bool[dimension.width, dimension.height];
        int xNew = 0;
        int yNew = 0;
        for (int xThis = startX; xThis < startX + dimension.width; xThis++) {
            for (int yThis = startY; yThis < startY + dimension.height; yThis++) {
                if (xThis >= mapDimension.width || yThis >= mapDimension.height || xThis < 0 || yThis < 0) {
                    array[xNew,yNew] = true;
                } else {
                    array[xNew, yNew] = pathingMap[xThis][yThis];
                }
                yNew++;
            }
            yNew = 0;
            xNew++;
        }

        return array;
    }
}
