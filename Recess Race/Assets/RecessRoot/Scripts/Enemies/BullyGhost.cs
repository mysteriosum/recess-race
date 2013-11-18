using UnityEngine;
using System.Collections;

public class BullyGhost : MonoBehaviour {
	
	public int speed = 1;
	
	private Transform t;
	// Use this for initialization
	void Start () {
		t = transform;
	}
	
	// Update is called once per frame
	void Update () {
		t.Translate(Vector3.up * speed * Time.deltaTime);
	}
}
