using UnityEngine;
using System.Collections;

public class AgentInstructionTrigger : MonoBehaviour {

	public float jumpHoldingLenght = 13;
	public float moveHoldingLenght = 13;

	public Direction direction = Direction.right;

	public Instruction getInstruction(Agent agent){
		Vector3 runto = this.transform.position;
		runto.x += (int)direction;

		return InstructionFactory.makeRunJump(agent, runto, direction, jumpHoldingLenght, moveHoldingLenght) ;
	}


	void OnDrawGizmos(){
		Transform t = GetComponent<Transform> ();
		BoxCollider2D derCollider = GetComponent<BoxCollider2D> ();
		Color myColor;

		float alpha = 1;
		Vector3 size = Vector3.one;
		if (derCollider)
			size = (Vector3)derCollider.size;

		if (direction.Equals (Direction.left)) {
			myColor = new Color (this.jumpHoldingLenght / 13f , 255, 0, alpha);
		} else {
            myColor = new Color(255, 0, this.jumpHoldingLenght / 13f, alpha);
		
		}

		Gizmos.color = myColor;
		Gizmos.DrawCube (t.position, size);
	}
}
