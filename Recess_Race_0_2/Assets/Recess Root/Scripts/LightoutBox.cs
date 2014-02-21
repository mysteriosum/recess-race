using UnityEngine;
using System.Collections;

public class LightoutBox : MonoBehaviour {

	private Color color = Color.yellow;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other){
		color = Color.green;
	}

	void OnDrawGizmos(){
		Gizmos.color = color;
		Gizmos.DrawCube (this.transform.position, Vector2.one);
	}
}
