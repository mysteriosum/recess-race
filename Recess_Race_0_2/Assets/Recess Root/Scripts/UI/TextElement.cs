using UnityEngine;
using System.Collections;

[System.Serializable]
public class TextElement : UIElement {
	
	public string text;
	public StyleEnum style;
	
	public GUIStyle Style {
		get{
			return GameManager.gm.hud.skin.customStyles[(int)style];
		}
	}
	
	public TextElement (Rect position, string text, StyleEnum style){
		this.position = position;
		this.text = text;
		this.style = style;
	}
	
	public override void Show(){
		GUIStyle tempStyle = new GUIStyle(Style);
		GUI.TextArea(PositionRect, text, tempStyle);
	}
}
