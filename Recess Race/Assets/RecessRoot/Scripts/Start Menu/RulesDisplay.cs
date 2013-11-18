using UnityEngine;
using System.Collections;

public class RulesDisplay : GizmoDad {
	
	
	public Renderer rulesPage;
	
	private bool showing = false;
	
	// Use this for initialization
	void Start () {
		rulesPage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (showing && Input.anyKeyDown){
			rulesPage.enabled = false;
			showing = false;
			Time.timeScale = 1;
		}
	}
	
	
	
	void OnTriggerEnter (Collider fitz){
		Time.timeScale = 0;
		rulesPage.enabled = true;
		showing = true;
	}
}
