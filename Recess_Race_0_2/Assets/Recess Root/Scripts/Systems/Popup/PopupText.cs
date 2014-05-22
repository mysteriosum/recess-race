using UnityEngine;
using System.Collections;

[System.Serializable]
public class PopupText {
	
	public PopupConfiguration popupConfiguration;
	public float life;
	public string text;
	private AnimationCurve xCurve;
	private AnimationCurve yCurve;
	private float sizeCurveLenght;

	public PopupText(PopupConfiguration popupConfiguration,string text){
		this.popupConfiguration = popupConfiguration;
		this.text = text;
		this.xCurve = new AnimationCurve ();
		this.yCurve = new AnimationCurve ();
		this.life = 0;
		//sizeCurveLenght = popupConfiguration.sizeCurve [popupConfiguration.sizeCurve.length - 1].time;
		sizeCurveLenght = 1;
	}
	
	public float getFontSize(){
		if (popupConfiguration.sizeCurve == null) return 8;
		return popupConfiguration.sizeCurve.Evaluate (life/popupConfiguration.lifetime * sizeCurveLenght);
	}
	
	public Color getColor(){
		if (popupConfiguration.gradient == null) return Color.black;
		return popupConfiguration.gradient.Evaluate(life/popupConfiguration.lifetime);
	}

	
	public bool isAtEndOfLife(){
		return life >= popupConfiguration.lifetime;
	}
	
	public Vector2 getPosition(){
		return new Vector2 (xCurve.Evaluate (life / popupConfiguration.lifetime), yCurve.Evaluate (life / popupConfiguration.lifetime));
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
	
	/*public PopupText clone(){
		PopupText clone = new PopupText ();
		clone.font = this.font;
		clone.sizeCurve = this.sizeCurve;
		clone.gradient = this.gradient;
		clone.xCurve = this.xCurve;
		clone.yCurve = this.yCurve;
		clone.text = this.text;
		clone.lifetime = this.lifetime;
		
		return clone;
	}*/
}
