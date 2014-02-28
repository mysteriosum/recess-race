using UnityEngine;
using System.Collections;

public class LightoutBox : MonoBehaviour {

	private Color color = Color.white;
    public bool triggered { get; private set; }


	void Update () {
		GetComponent<SpriteRenderer>().color = color;
	}


	void OnTriggerEnter2D (Collider2D other){
        triggered = true;
		color = Color.green;
	}

    public void resetColor() {
        triggered = false;
        color = Color.white;
        GetComponent<SpriteRenderer>().color = color;
    }
}
