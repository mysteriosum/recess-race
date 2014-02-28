using UnityEngine;
using System.Collections;

public class fellow : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (target != null) {
            this.transform.position = this.target.position;
            this.transform.Translate(0, 0, -2);
        }
		
	}
}
