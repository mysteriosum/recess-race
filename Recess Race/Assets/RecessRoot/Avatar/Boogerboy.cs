using UnityEngine;
using System.Collections;

public class Boogerboy : Character {
	
	public float dashSpeed;
	public float dashTiming;
	public float wallJumpLockTime;
	
	
	
	private float dashTimer;
	void Awake() {
		base.Awake();
		name = "Fitzwilliam";
	}
	// Use this for initialization
	void Start () {
		base.Start();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		base.Update();
	}
	
	void Dash () {
		Debug.Log("Dash!");
	}
}
