using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Profile : MonoBehaviour {

	public virtual string Cheer {
		get {
			int n = cheers.Length;
			int max = n * (n + 1) / 2;
			int roll = UnityEngine.Random.Range(0, max);
			int index = (int) (Mathf.Sqrt(8 * roll + 1) - 1) /2;
			return cheers[index];
		}
	}
	
	protected String[] cheers = new String[] {
		"You can do it!",
		"Go go go!",
		"Go Brandon!",
		"Go Ashley!",
		"Go Romney!",
		"Don't let Fitz win!",
		"Don't give up!",
		"I believe in you!",
		"Don't forget the tennis balls!",
	};
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
