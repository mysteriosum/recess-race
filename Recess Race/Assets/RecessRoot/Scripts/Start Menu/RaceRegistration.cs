using UnityEngine;
using System.Collections;

public class RaceRegistration : GizmoDad {
	
	private SpeechBubble speechy;
	private GameObject speechgo;
	private BoxCollider bc;
	
	public string myText = "You'd better go read the rules before it starts!";
	
	// Use this for initialization
	void Start () {
		bc = GetComponent<BoxCollider>();
		
		//speechy = Instantiate (Resources.Load("res_speechBubble"), bc.bounds.center + bc.size/2, transform.rotation) as SpeechBubble;
		speechgo = Instantiate (Resources.Load("res_speechBubble"), bc.bounds.center + bc.size/2, transform.rotation) as GameObject;
		
		speechy = speechgo.GetComponent<SpeechBubble>();
		
		speechy.transform.parent = transform;
		speechy.Text = myText;
		speechy.Active = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider fitz){
		speechy.Text = myText;
		speechy.Active = true;
	}
	
	void OnTriggerExit (Collider fitz){
		speechy.Active = false;
	}
	
	void OnTriggerStay(Collider fitz){
		
		if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Run")){
			Application.LoadLevel("room_main");
		}
	}
}
