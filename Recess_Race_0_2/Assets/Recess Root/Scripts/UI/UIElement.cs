using UnityEngine;
using System.Collections;

public enum StyleEnum {
	time,
	numbers,
	main,
	pause,
	levelSelect,
}
[System.Serializable]
public abstract class UIElement {
	
	public Rect position = new Rect (0.4f, 0.4f, 0.2f, 0.2f);
	[HideInInspector]
	public float scale = 1f;
	
	public static Vector2 MousePosition {
		get{
			Vector2 pos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			return pos;
		}
	}
	
	public float OneMinusScale {
		get{
			return 1f - scale;
		}
	}
	public virtual Rect PositionRect {
		get{
			if (scale != 1f){
				RectOffset offset = new RectOffset((int) (OneMinusScale * position.width / 2), (int) (OneMinusScale * position.height / 2), (int) (OneMinusScale * position.width / 2), (int) (OneMinusScale * position.height / 2));
				return offset.Add(MainMenu.MultiplyRectByScreenDimensions(position));
			}
			return MainMenu.MultiplyRectByScreenDimensions(position);
		}
	}
	
	// Use this for initialization
	public abstract void Show ();
	
}
