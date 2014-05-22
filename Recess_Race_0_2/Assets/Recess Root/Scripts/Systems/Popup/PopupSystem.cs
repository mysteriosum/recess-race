using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupSystem : MonoBehaviour {

	private static List<PopupText> popupTexts;
	private static List<PopupText> popupTextsToRemove;


	void Start () {
		PopupSystem.popupTexts = new List<PopupText> ();
		PopupSystem.popupTextsToRemove = new List<PopupText> ();
	}
	
	public static void AddPopup(PopupText popup){
		if (popupTexts == null) {
			Debug.Log("PopupSystem in no GameObject!");
			return;		
		}
		popupTexts.Add (popup);
	}
	
	
	void OnGUI  () {
		foreach(PopupText p in popupTexts){
			drawPopup(p);
			p.life += Time.deltaTime;
			if(p.isAtEndOfLife()){
				popupTextsToRemove.Add(p);
			}
		}
		foreach (PopupText pToRemove in popupTextsToRemove) {
			popupTexts.Remove(pToRemove);
		}
		popupTextsToRemove.Clear();
	}
	
	
	void drawPopup(PopupText p){
		GUIStyle style = new GUIStyle ();
		style.font = p.popupConfiguration.font;
		style.fontStyle = FontStyle.Bold;
		int fontSize = (int) p.getFontSize();
		style.fontSize = fontSize;
		style.normal.textColor = p.getColor();
		
		Vector2 textSize = style.CalcSize(new GUIContent(p.text));
		float x = p.getPosition().x - textSize.x / 2;
		float y = p.getPosition().y - textSize.y / 2;
		Rect rect = new Rect (x, y, textSize.x, textSize.y);
		GUI.Label (rect, p.text ,style );
	}
}
