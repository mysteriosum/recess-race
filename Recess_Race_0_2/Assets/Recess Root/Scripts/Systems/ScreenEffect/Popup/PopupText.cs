using UnityEngine;
using System.Collections;

[System.Serializable]
public class PopupText : ScreenEffect{
	
	public PopupConfiguration popupConfiguration;
	public string text;
	private AnimationCurve xCurve;
	private AnimationCurve yCurve;
	private float sizeCurveLenght;

	GUIStyle style;

	public PopupText(PopupConfiguration popupConfiguration,string text):base(popupConfiguration.lifetime){
		this.popupConfiguration = popupConfiguration;
		this.text = text;
		this.xCurve = new AnimationCurve ();
		this.yCurve = new AnimationCurve ();
		//sizeCurveLenght = popupConfiguration.sizeCurve [popupConfiguration.sizeCurve.length - 1].time;
		sizeCurveLenght = 1;

		style = new GUIStyle ();
		style.font = popupConfiguration.font;
		style.fontStyle = FontStyle.Bold;
	}
	
	public void setPositionCurveToConstant(Vector2 p){
		emptyCurves ();
		xCurve.AddKey (0,p.x);
		yCurve.AddKey (0,p.y);
	}
	
	public void addPositionToCurve(float time, Vector2 p){
		xCurve.AddKey (time, p.x);
		yCurve.AddKey (time, p.y);
	}
	
	public void emptyCurves(){
		emptyCurve (xCurve);
		emptyCurve (yCurve);
	}
	
	public void emptyCurve(AnimationCurve c){
		while (c.length != 0) {
			c.RemoveKey(0);		
		}
	}
	
	public override void show(){
		style.fontSize 			= getFontSize();
		style.normal.textColor 	= getColor();
		
		Vector2 textSize = style.CalcSize(new GUIContent(text));
		float x = xCurve.Evaluate (1-life / popupConfiguration.lifetime) - textSize.x / 2;
		float y = yCurve.Evaluate (1-life / popupConfiguration.lifetime) - textSize.y / 2;
		Rect rect = new Rect (x, y, textSize.x, textSize.y);
		GUI.Label (rect, text ,style );
	}

	
	
	private int getFontSize(){
		if (popupConfiguration.sizeCurve == null) return 8;
		return (int)popupConfiguration.sizeCurve.Evaluate (1-life/popupConfiguration.lifetime * sizeCurveLenght);
	}
	
	private Color getColor(){
		if (popupConfiguration.gradient == null) return Color.black;
		return popupConfiguration.gradient.Evaluate(1-life/popupConfiguration.lifetime);
	}
}
