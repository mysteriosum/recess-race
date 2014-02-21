using UnityEngine;
using System.Collections;

public class LightoutBox : MonoBehaviour {

	private Color color = Color.yellow;


	void Update () {
		GetComponent<SpriteRenderer>().color = color;
	}


	void OnTriggerEnter2D (Collider2D other){
		color = Color.green;
	}
}
