using UnityEngine;
using System.Collections;

public class RecessCamera : MonoBehaviour {
	private Transform t;
	private Transform fitzNode;
	private Transform box;
	
	public static RecessCamera cam;
	const int scrnHeight = 480;
	const int scrnWidth = 640;
	
	public Transform[] paralaxes;
	
	private float lerpAmount = 0.1f;
	private float maxParalax = 0.3f;
	private float furthestParalaxZ;
	
	// Use this for initialization
	void Awake () {
		if (cam != null){
			Destroy (gameObject);
		}
		else{
			cam = this;
		}
	}
	void Start () {
		t = transform;
		//Fitz fitzScript = GameObject.FindObjectOfType(typeof(Fitz)) as Fitz;
		try{
			fitzNode = GameObject.Find("Fitzwilliam").GetComponentInChildren<GizmoDad>().transform;
		}
		catch{
			Debug.LogError ("There's no 'Fitzwilliam' in the scene, the camera doesn't like");
		}
		
		foreach(Transform tr in paralaxes){
			if (tr.position.z > furthestParalaxZ){
				furthestParalaxZ = tr.position.z;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 forepos = t.position;
		if (fitzNode != null){
			Vector3 target = new Vector3(fitzNode.position.x , fitzNode.position.y , t.position.z);
			t.position = Vector3.Lerp(t.position, target, lerpAmount);
		}
		Vector3 postPos = t.position;
		
		foreach (Transform tran in paralaxes){
			//Never Used
			//float parAmount = tran.position.z * maxParalax / furthestParalaxZ;		//figure out how much to move each object. Easy because player z = 0 always
			
			tran.Translate((postPos - forepos) * maxParalax, Space.World);
		}
		
		RecessManager.CurrentTime += Time.deltaTime;
	}
	
	void OnGUI(){
		GUI.Box(new Rect(0, 0, 100, 50), RecessManager.Score.ToString());
		GUI.Box(new Rect(0, Screen.height - 50, 100, 50), RecessManager.TimeString);
	}
}
