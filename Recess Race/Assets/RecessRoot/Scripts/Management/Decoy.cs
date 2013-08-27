using UnityEngine;
using System.Collections;

public class Decoy : MonoBehaviour {
	
	public GameObject original;
	public GameObject representative;
	// Use this for initialization
	void Start () {
		
		if (Application.loadedLevel != 0){
			Activate ();
		}
		if (original == null){
			Debug.LogWarning("You didn't assign an original to this decoy!");
			Destroy (gameObject);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Activate (){
		representative = Instantiate (original, transform.position, transform.rotation) as GameObject;
		representative.transform.localScale = transform.localScale;
		renderer.enabled = false;
	}
	
	public void Deactivate () {
		Destroy (representative);
		renderer.enabled = true;
	}
	
}
