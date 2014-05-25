using UnityEngine;
using System.Collections;

public class FadeOut : ScreenEffect {
	
	private float maxLife;
	private Color color;
	
	public FadeOut(float maxLife) : base(maxLife){
		this.maxLife = maxLife;
		this.color = Color.black;
	}
	
	public FadeOut(float maxLife, Color color) : base(maxLife){
		this.maxLife = maxLife;
		this.color = color;
	}
	
	public override void show(){
		Texture2D blackBox = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		color.a = 1 - this.life / this.maxLife;
		blackBox.SetPixels(new Color[1] {color});
		blackBox.Apply ();
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox, ScaleMode.StretchToFill);
	}
	
}