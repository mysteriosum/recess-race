using UnityEngine;
using System.Collections;

public enum StunDurations{
	light = 1,
	medium = 2,
	harsh = 3
}

public class DamageScript : MonoBehaviour {
	
	public StunDurations severity;
	
	public int StunDuration {
		get { return (int) severity; }
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
