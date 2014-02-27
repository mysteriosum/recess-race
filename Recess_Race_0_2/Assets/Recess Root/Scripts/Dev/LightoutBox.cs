using UnityEngine;
using System.Collections;

public class LightoutBox : MonoBehaviour {

	private Color color = Color.white;


	void Update () {
		GetComponent<SpriteRenderer>().color = color;
	}


	void OnTriggerEnter2D (Collider2D other){
		color = Color.green;
	}

    public void resetColor() {
        color = Color.white;
        GetComponent<SpriteRenderer>().color = color;
    }
}
