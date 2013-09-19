using UnityEngine;
using System.Collections;

public class Checkpoint : GizmoDad {
	
	private static int total = 0;
	private int index;
	public int Index {
		get { return index; }
	}
	
	// Use this for initialization
	void Start () {
		index = total;
		total ++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Enter (){
		RecessManager.Instance.NextLevel (index, transform);
	}
}
