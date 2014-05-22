using UnityEngine;
using System.Collections;

[System.Serializable]
public class ImageElement : UIElement {
	
	public Texture texture;
	public ScaleMode scaleMode;
	
	public ImageElement (Rect position, Texture texture, ScaleMode scaleMode){
		this.position = position;
		this.texture = texture;
		this.scaleMode = scaleMode;
	}
	
	public override void Show(){
		if (texture == null)
			return;
		GUI.DrawTexture(PositionRect, texture, scaleMode);
	}
}
