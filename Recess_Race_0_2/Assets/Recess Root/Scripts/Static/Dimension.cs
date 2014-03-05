using UnityEngine;
using System.Collections;

[System.Serializable]
public class Dimension {

	public int width;
	public int height;

	public Dimension(){
	
	}


	public Dimension(int width, int height){
		this.width = width;
		this.height = height;
	}

    public override string ToString() {
        return "Dimension ( width= " + width + ", height= " + height + " )";
    }
}
