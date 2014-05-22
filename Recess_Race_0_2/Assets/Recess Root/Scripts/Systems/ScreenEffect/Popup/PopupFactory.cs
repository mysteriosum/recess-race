using UnityEngine;
using System.Collections;

public class PopupFactory : MonoBehaviour {

	public static PopupText makeFixPopup(PopupConfiguration popupConfiguration,string text, Vector2 position){
		PopupText popupText = new PopupText (popupConfiguration,text);
		popupText.setPositionCurveToConstant(position);
		popupText.text = text;
		return popupText;
	}
	
	
	public static PopupText makeLinearPopup(PopupConfiguration popupConfiguration,string text, Vector2 startPosition, Vector2 endPosition, float tStartMoving = 0, float tStopMoving = 1){
		PopupText popupText = new PopupText (popupConfiguration,text);
		popupText.emptyCurves();
		popupText.addPositionToCurve(0,startPosition);
		if (tStartMoving != 0) {
			popupText.addPositionToCurve(tStartMoving,startPosition);
		}
		if (tStopMoving != 1) {
			popupText.addPositionToCurve(tStopMoving,endPosition);
		}
		popupText.addPositionToCurve(1,endPosition);
		popupText.text = text;
		return popupText;
	}
}
