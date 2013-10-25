using UnityEngine;
using System.Collections;

public class BrickBlock : MonoBehaviour {
	
	int hp = 2;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x, transform.position.y, -1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Explode () {
		gameObject.SetActive(false);
	}
	
	void Crack () {
		hp --;
		if (hp == 0){
			Explode();
		}
	}
}
