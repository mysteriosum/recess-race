using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	
	public bool Collected {
		get { return t.position == initPosition; }
		set {
			t.position = new Vector3(initPosition.x, initPosition.y, value? 10000 : initPosition.z);
		}
	}
	
	protected Vector3 initPosition;
	protected Transform t;
	// Use this for initialization
	void Start () {
		t = transform;
		
		initPosition = t.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
