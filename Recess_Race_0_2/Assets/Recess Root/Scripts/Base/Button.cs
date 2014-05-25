using UnityEngine;
using System.Collections;

[System.Serializable]
public class Button {
	
	private Rect rect;
	
	public delegate void ButtonDelegate();
	public ButtonDelegate buttonFunction;
	
	public ImageElement imageElement;
	public TextElement textElement;
	private UIElement butt;
	
	private float hoverScale = 1.1f;
	private float clickScale = 0.95f;
	private Sounds sounds;
	private bool mouseWasOn;
	private bool mouseIsOn;
	
	// Use this for initialization
	public void Init () {
		sounds = new Sounds();
		if (textElement.text == ""){
			butt = imageElement;
			rect = imageElement.PositionRect;
		} else{
			butt = textElement;
			rect = textElement.PositionRect;
		}
	}
	
	// Update is called once per frame
	public void Show () {
		
		mouseIsOn = rect.Contains(UIElement.MousePosition);
		
		if (mouseIsOn && !mouseWasOn){
			OnMouseEnter();
		} else if (mouseWasOn && !mouseIsOn){
			OnMouseExit();
		}
		if (mouseIsOn && Input.GetMouseButton(0)){
			OnMouseDrag();
		}
		if (mouseIsOn && Input.GetMouseButtonUp(0)){
			OnMouseUp();
		}
		
		if (imageElement.texture != null){
			imageElement.Show();
		}
		if (textElement.text != ""){
			textElement.Show();
		}
		
		mouseWasOn = mouseIsOn;
	}
	
	void OnMouseEnter () {
		Debug.Log("On mouse enter: " + textElement.text);
		butt.scale = hoverScale;
		GameManager.gm.PlaySound(sounds.menuSelect, 0.1f);
	}
	void OnMouseExit () {
		Debug.Log("On mouse exit: " + textElement.text);
		butt.scale = 1f;
	}
	
	void OnMouseDrag () {
		Debug.Log("On mouse drag: " + textElement.text);
		butt.scale = clickScale;
	}
	
	void OnMouseUp () {
		Debug.Log("On mouse up: " + textElement.text);
		butt.scale = 1f;
		buttonFunction();
	}
	
}
