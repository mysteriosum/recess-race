using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	public Transform toFollow;
	
	private Transform box;
	Transform trans;
	
	private Rect border;
	private Rect effectiveBorder;
	public Rect Border {
		get{ return border; }
		set{ border = value; }
	}
	
	// parallax members
    [HideInInspector]
    public List<Transform> parallaxes = new List<Transform>();
    [HideInInspector]
    public float furthestParalaxZ;
	private float farDistance;
	
	private float lerpAmount = 0.1f;
	private float maxParallax = 0.3f;
	
	
	public static CameraFollow cam;
	
	// Use this for initialization
	
	void Awake () {
		if (cam == null){
			cam = this;
		} else {
			Destroy(this);
		}
		trans = transform;
	}
	
	void Start () {
		if (toFollow == null){
			
			try{
				toFollow = GameObject.Find("Fitzwilliam").GetComponentInChildren<GizmoDad>().transform;
			}
			catch{
				Debug.LogError ("There's no follow object assigned in the inspector, and there's no Fitzwilliam. FAIL");
			}
			
		}
		effectiveBorder = new Rect(border);
		effectiveBorder = new RectOffset((int) camera.orthographicSize * Screen.width/Screen.height,(int)  camera.orthographicSize * Screen.width/Screen.height,
		                                 (int) camera.orthographicSize,(int)  camera.orthographicSize).Remove(effectiveBorder);

		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		Vector3 forepos = trans.position;
		if (toFollow != null && GameManager.gm.RaceBegun){
			Vector3 target = new Vector3(toFollow.position.x , toFollow.position.y , trans.position.z);
			trans.position = Vector3.Lerp(trans.position, target, lerpAmount);
			
			if (effectiveBorder.width > 1 && !effectiveBorder.Contains((Vector2)trans.position)){
				trans.position = new Vector3(Mathf.Clamp(trans.position.x, effectiveBorder.xMin, effectiveBorder.xMax),
										Mathf.Clamp(trans.position.y, effectiveBorder.yMin, effectiveBorder.yMax), trans.position.z);
			}
		}
		Vector3 postPos = trans.position;
		
		foreach (Transform tran in parallaxes){
			//Never Used
			//float parAmount = tran.position.z * maxParalax / furthestParalaxZ;		//figure out how much to move each object. Easy because player z = 0 always
			
			tran.Translate((postPos - forepos) * (maxParallax * tran.position.z / furthestParalaxZ), Space.World);
		}
	}
	
	public void EndRace () {
		toFollow.parent = null;
		
	}
}
