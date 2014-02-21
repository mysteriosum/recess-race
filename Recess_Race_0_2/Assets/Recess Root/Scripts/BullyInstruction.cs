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

    private float maxHold = 1.5f;
    private float minHold = 0;
    private float mediumHold = 0.5f;

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

    public float getDirection() {
        switch (moveDirection) {
            case CommandEnum.left: 
                return -1f;
            case CommandEnum.right:
                return 1f;
            default:
                return 0;
        }
    }

    public float getTarget() {
        switch (jumpLength) {
            case LengthEnum.medium:
                return mediumHold;
            case LengthEnum.hold:
                return maxHold;
            case LengthEnum.tap:
                return minHold;
            default:
                return 0;
        }
   
    }

    public int getTargetPercentile(){
    switch (jumpLength){
		case LengthEnum.medium:
			return (int) (mediumHold / maxHold * 100);
		case LengthEnum.hold:
			return 99;
		case LengthEnum.tap:
			return 0;
        default:
            return 0;
		}
    }
}



public class BullyInstruction : MonoBehaviour {

	public BullyInstructionConfiguration configuration;

	private Couleur myColour;
	
	public bool IsAJumpCommand{
        get { return configuration.isAJump(); }
	}
	public float Direction{
        get { return configuration.getDirection(); }
	}
	
	public float MyTarget{
        get { return configuration.getTarget(); }
	}
	public int MyPercentile{
        get { return configuration.getTargetPercentile(); }
	}
	public int Difficulty{
        get { return (int)configuration.jumpDifficulty; }
	}

    public void setTo(BullyInstructionConfiguration config){
        configuration.jumpDifficulty = config.jumpDifficulty;
        configuration.moveDirection = config.moveDirection;
        configuration.jumpLength = config.jumpLength;
    }
	

	void Start () {
		
	}
	

	void Update () {
	
	}
	
	
	public LengthEnum GetImpulse () {
        return configuration.jumpLength;
	}

	
	void OnDrawGizmos(){
		Transform t = GetComponent<Transform>();
		BoxCollider2D derCollider = GetComponent<BoxCollider2D>();
		Color myColor;
		
		float alpha = 1;
		Vector3 size = Vector3.one;
		if (derCollider)
			size = (Vector3) derCollider.size;

        int index = (int)configuration.moveDirection + (configuration.jumpLength == LengthEnum.none ? 0 : 4);
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
