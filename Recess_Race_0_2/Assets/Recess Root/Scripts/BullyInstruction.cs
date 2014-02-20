using UnityEngine;
using System.Collections;

public enum LengthEnum{
	none = -1, tap = 0, medium = 1, hold = 2
}

public enum CommandEnum{
	middle = 0,
	left = 1, 
	right = 2,
}

public enum DifficultyEnum{
	assured = -100,
	simple = -20,
	easy = 0,
	medium = 10,
	difficult = 20,
	crazy = 40,
}

[System.Serializable]
public class BullyInstructionConfiguration{
    public LengthEnum jumpLength = LengthEnum.none;
    public CommandEnum moveDirection = CommandEnum.middle;
    public DifficultyEnum jumpDifficulty = DifficultyEnum.assured;

    public BullyInstructionConfiguration(LengthEnum jumpLength, CommandEnum moveDirection, DifficultyEnum jumpDifficulty) {
        this.jumpLength = jumpLength;
        this.moveDirection = moveDirection;
        this.jumpDifficulty = jumpDifficulty;
    }

    public BullyInstructionConfiguration()
    {
        
    }

	public bool isAJump(){
		return !jumpLength.Equals (LengthEnum.none);
	}
}



public class BullyInstruction : MonoBehaviour {

	public BullyInstructionConfiguration configuration;
	public LengthEnum jumpLength = LengthEnum.none;
	public CommandEnum moveDirection = CommandEnum.middle;
	public DifficultyEnum jumpDifficulty = DifficultyEnum.assured;
	
	private float myTarget;
	private float maxHold = 1.5f;
	private float minHold = 0;
	private float mediumHold = 0.5f;
	
	private bool isAJumpCommand;
	
	private int targetPercentile;
	private float direction;
	
	private Couleur myColour;
	
	public bool IsAJumpCommand{
		get { return jumpLength != LengthEnum.none; }
	}
	public float Direction{
		get { return direction; }
	}
	
	public float MyTarget{
		get { return myTarget; }
	}
	public int MyPercentile{
		get { return targetPercentile; }
	}
	public int Difficulty{
		get { return (int) jumpDifficulty; }
	}

    public void setTo(BullyInstructionConfiguration config){
        this.jumpDifficulty = config.jumpDifficulty;
        this.moveDirection = config.moveDirection;
        this.jumpLength = config.jumpLength;
    }
	
	// Use this for initialization
	void Start () {
		switch (moveDirection){
		case CommandEnum.left:
			direction = -1f;
			break;
		case CommandEnum.right:
			direction = 1f;
			break;
		default:
			direction = 0;
			break;
		}
		
		switch (jumpLength){
		case LengthEnum.medium:
			myTarget = mediumHold;
			targetPercentile = (int) (mediumHold / maxHold * 100);
			break;
		case LengthEnum.hold:
			myTarget = maxHold;
			targetPercentile = 99;
			break;
		case LengthEnum.tap:
			myTarget = minHold;
			targetPercentile = 0;
			break;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public LengthEnum GetImpulse () {
		return jumpLength;
	}

	
	void OnDrawGizmos(){
		Transform t = GetComponent<Transform>();
		BoxCollider2D derCollider = GetComponent<BoxCollider2D>();
		Color myColor;
		
		float alpha = 1;
		Vector3 size = Vector3.one;
		if (derCollider)
			size = (Vector3) derCollider.size;
		
		int index = (int) moveDirection + (jumpLength == LengthEnum.none? 0 : 4);
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
