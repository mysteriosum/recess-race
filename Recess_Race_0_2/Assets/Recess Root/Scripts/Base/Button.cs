using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	public string Text{
		get{
			return tm.text;
		}
		set{
			if (tm == null){
				tm = gameObject.GetComponent<TextMesh>();
			}
			
			tm.text = value;
			
			Destroy(bc);
			bc = gameObject.AddComponent<BoxCollider>();
		}
	}
	
	public Vector2 camOffset = new Vector2(0, 0);
	private Vector2 fuckOffPos = new Vector2(-10000, -10000);
	
	public delegate void ButtonDelegate();
	public ButtonDelegate buttonFunction;
	
	private SpriteRenderer sr;
	private Transform t;
	private TextMesh tm;
	private BoxCollider bc;
	private float hoverScale = 1.1f;
	private float clickScale = 0.9f;
	private Sounds sounds;
	
	public bool Active {
		get {
			return t.position.x > 0;
		}
		set {
			if (t == null)
				t = transform;
			if (value){
				t.localPosition = new Vector3(camOffset.x, camOffset.y, 5);
			}
			else{
				t.localPosition = new Vector3(fuckOffPos.x, fuckOffPos.y, -5);
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		t = transform;
		sounds = new Sounds();
	}
	
	// Update is called once per frame
	void Update () {
		//HACK to make sure I have the right positions on stuff. This should be removed by the end.
		if (Active){
			Active = true;
		}
	}
	
	void OnMouseEnter () {
		t.localScale = Vector3.one * hoverScale;
		RecessCamera.cam.PlaySound(sounds.menuSelect, 0.1f);
	}
	void OnMouseExit () {
		t.localScale = Vector3.one;
	}
	
	void OnMouseDrag () {
		t.localScale = Vector3.one * clickScale;
	}
	
	void OnMouseUp () {
		t.localScale = Vector3.one;
		buttonFunction();
	}
	
}
