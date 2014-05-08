using UnityEngine;
using System.Collections;

public class ScreenUtils {
	
	public static Vector2 getPositionInScreen(Vector3 positionInWorld){
		Vector3 screenPoint = getCamera().WorldToScreenPoint(positionInWorld); 
		return new Vector2(screenPoint.x, Screen.height - screenPoint.y);
	}
	
	public static Vector2 getPositionFromTopRight(Vector2 topLeftPosition){
		return new Vector2 (Screen.width - topLeftPosition.x, topLeftPosition.y);
	}

	public static Camera getCamera(){
		return RecessCamera.cam.GetComponent<Camera> ();;
	}

	
	public static Vector2 getCenterFromScreen(){
		return new Vector2(Screen.width/2, Screen.height/2);
	}
}
