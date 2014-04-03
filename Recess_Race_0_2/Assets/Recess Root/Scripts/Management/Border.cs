using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour {
	
	
	public Rect border;

	// Use this for initialization
	void Start () {/*
		Transform top = null;
		Transform bottom = null;
		Transform right = null;
		Transform left = null;
		
		for (int i = 0; i < transform.childCount; i++) {
			Transform t = transform.GetChild(i);
			
			if (t.name.Equals("top", System.StringComparison.OrdinalIgnoreCase)){
				top = t;
			}else if (t.name.Equals("bottom", System.StringComparison.OrdinalIgnoreCase)){
				bottom = t;
			}else if (t.name.Equals("right", System.StringComparison.OrdinalIgnoreCase)){
				right = t;
			}else if (t.name.Equals("left", System.StringComparison.OrdinalIgnoreCase)){
				left = t;
			}
		}
		if (top != null && bottom != null && left != null && right != null){
			border = new Rect(left.position.x, bottom.position.y, 
				Mathf.Abs(right.position.x - left.position.x), Mathf.Abs(top.position.y - bottom.position.y));
			RecessCamera.cam.Border = border;
		}
		else{
			Debug.LogWarning("The border in your scene is not set correctly");
		}*/
		RecessCamera.cam.Border = border;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
