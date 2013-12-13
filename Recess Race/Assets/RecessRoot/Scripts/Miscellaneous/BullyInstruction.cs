using UnityEngine;
using System.Collections;

public enum HeightEnum{
	none, low, medium, high
}

public enum CommandEnum{
	middle = 0,
	left = 1, 
	right = 2,
}

public enum DifficultyEnum{
	assured = 200,
	easy = 100,
	medium = 80,
	difficult = 70,
	crazy = 60,
}

public class BullyInstruction : MonoBehaviour {
	
	public HeightEnum jumpHeight = HeightEnum.none;
	public CommandEnum moveDirection = CommandEnum.middle;
	public DifficultyEnum jumpDifficulty = DifficultyEnum.assured;
	
	private Couleur myColour;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public float GetDirection (){
		switch (moveDirection){
		case CommandEnum.left:
			return -1;
		case CommandEnum.right:
			return 1;
		default:
			return 0;
		}
	}
	
	public HeightEnum GetImpulse () {
		return jumpHeight;
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
	
	void OnDrawGizmos(){
		Transform t = GetComponent<Transform>();
		BoxCollider derCollider = GetComponent<BoxCollider>();
		Color myColor;
		
		float alpha = 1;
		Vector3 size = Vector3.one;
		if (derCollider)
			size = derCollider.size;
		
		int index = (int) moveDirection + (jumpHeight == HeightEnum.none? 0 : 4);
		myColour = (Couleur)index;
		
		switch (myColour){
			
		case Couleur.black:
			myColor = new Color(0, 0, 0, alpha);
			break;
			
		case Couleur.white:
			myColor = new Color(255, 255, 255, alpha);
			break;
			
		case Couleur.red:
			myColor = new Color(255, 0, 0, alpha);
			break;
			
		case Couleur.blue:
			myColor = new Color(0, 0, 255, alpha);
			break;
			
		case Couleur.green:
			myColor = new Color(0, 255, 0, alpha);
			break;
			
		case Couleur.yellow:
			myColor = new Color(255, 255, 0, alpha);
			break;
			
		case Couleur.magenta:
			myColor = new Color(255, 0, 255, alpha);
			break;
			
		case Couleur.cyan:
			myColor = new Color(0, 255, 255, alpha);
			break;
			
		default:
			myColor = Color.black;
			break;
		}
		Gizmos.color = myColor;
		Gizmos.DrawCube (t.position, size);
	}
}
