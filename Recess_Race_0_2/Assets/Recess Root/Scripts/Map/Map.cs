using UnityEngine;
using System.Collections;


public enum SplitDirection{
	TopLeft, TopRight, BottomLeft, BottomRight
}
public class Map : MonoBehaviour {

	public Dimension mapDimension;
    public Dimension tileDimension;
	public float backgroundYOffset;
    public BoolArray[] pathingMap;

	public bool[,] splitTo(SplitDirection direction, Vector3 startingPosition, Dimension dimension) {
		switch (direction) {
		case SplitDirection.BottomLeft : return splitToLeft(startingPosition,dimension);
		case SplitDirection.BottomRight : return splitToRight(startingPosition,dimension);
		case SplitDirection.TopLeft : return splitToTopLeft(startingPosition,dimension);
		case SplitDirection.TopRight : return splitToTopRight(startingPosition,dimension);
		default: return null;
		}
	}

	public bool[,] splitToRight(Vector3 startingPosition, Dimension dimension) {
		int forXStart = (int) startingPosition.x;
        int forXEnd = (int)startingPosition.x + dimension.width;
		int forYStart = (int) startingPosition.y;
        int forYEnd = (int)startingPosition.y + dimension.height;
       // Debug.Log("Right " + dimension.ToString());
		return split(forXStart, forXEnd, forYStart, forYEnd);
	}

	public bool[,] splitToLeft(Vector3 startingPosition, Dimension dimension) {
		int forXStart = (int) startingPosition.x - dimension.width+1;
		int forXEnd = (int) startingPosition.x + 1;
		int forYStart = (int) startingPosition.y;
        int forYEnd = (int)startingPosition.y + dimension.height;
       // Debug.Log("Left " + dimension.ToString());
		return split(forXStart, forXEnd, forYStart, forYEnd);
	}

	public bool[,] splitToTopRight(Vector3 startingPosition, Dimension dimension) {
		int forXStart = (int) startingPosition.x ;
		int forXEnd = (int) startingPosition.x + + dimension.width;
		int forYStart = (int) startingPosition.y - dimension.height+1;
        int forYEnd = (int)startingPosition.y + 1;
        //Debug.Log("TopRight " + dimension.ToString());
		return split(forXStart, forXEnd, forYStart, forYEnd);
	}

	public bool[,] splitToTopLeft(Vector3 startingPosition, Dimension dimension) {
		int forXStart = (int) startingPosition.x - dimension.width+1;
		int forXEnd = (int) startingPosition.x + 1;
		int forYStart = (int) startingPosition.y - dimension.height+1;
        int forYEnd = (int)startingPosition.y + 1;
        //Debug.Log("TopLeft " + dimension.ToString());
		return split(forXStart, forXEnd, forYStart, forYEnd);
	}

	private bool[,] split(int forXStart, int forXEnd, int forYStart, int forYEnd){
        Dimension dimension = new Dimension(forXEnd - forXStart, forYEnd - forYStart);
        //Debug.Log(dimension.ToString());
		bool[,] array = new bool[dimension.width, dimension.height];
		int xNew = 0;
		int yNew = 0;
		for (int xThis = forXStart; xThis < forXEnd; xThis++) {
			for (int yThis = forYStart; yThis < forYEnd; yThis++) {
				if (xThis >= mapDimension.width || yThis >= mapDimension.height || xThis < 0 || yThis < 0) {
					array[xNew,yNew] = true;
				} else {
                    //Debug.Log(xNew + "," + yNew + " - " + xThis + "," + yThis);
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
