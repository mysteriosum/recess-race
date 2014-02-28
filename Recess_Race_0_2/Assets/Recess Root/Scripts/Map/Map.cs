using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public Dimension mapDimension;
    public Dimension tileDimension;
    public BoolArray[] pathingMap;

	/*// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

    public bool[,] split(Vector3 startingPosition, Dimension dimension) {
        return split((int)startingPosition.x, (int)startingPosition.y, dimension);
    }

    public bool[,] split(int startX, int startY, Dimension dimension) {
        bool[,] array = new bool[dimension.height, dimension.width];

        int i = 0;
        int j = 0;
        for (int x = startX; x < startX + dimension.width; x++) {
            for (int y = startY; y < startY + dimension.height; y++) {
                if (x > mapDimension.width || y > mapDimension.height) {
                    array[i,j] = true;
                } else {
                    array[i, j] = pathingMap[x][y];
                }
                i++;
            }
            i = 0;
            j++;
        }

        return array;
    }
}
