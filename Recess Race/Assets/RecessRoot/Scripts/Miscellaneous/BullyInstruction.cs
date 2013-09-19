using UnityEngine;
using System.Collections;

public enum HeightEnum{
	low, medium, high
}

public class BullyInstruction : GizmoDad {
	
	public HeightEnum jumpHeight = HeightEnum.medium;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Flip () {
		switch (tag){
		case "moveRight":
			tag = "moveLeft";
			break;
		case "moveLeft":
			tag = "moveRight";
			break;
		case "jumpRight":
			tag = "jumpLeft";
			break;
		case "jumpLeft":
			tag = "jumpRight";
			break;
			
		}
	}
}
