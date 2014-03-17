using UnityEngine;
using System.Collections;
using UnityEditor;


	
[System.Serializable]
[CustomEditor(typeof(AgentInstructionTrigger))]
public class AgentInstructionTriggerEditor : Editor {


	public override void OnInspectorGUI() {
		AgentInstructionTrigger trigger = (AgentInstructionTrigger)this.target;

		trigger.runBeforeInstruction = EditorGUILayout.FloatField ("Run Before The Instruction (x)", trigger.runBeforeInstruction);
		EditorGUILayout.Separator ();
		trigger.instructionType = (AgentInstructionTrigger.InstructionType) EditorGUILayout.EnumPopup ("Instruction type",trigger.instructionType);

		switch (trigger.instructionType) {
		case AgentInstructionTrigger.InstructionType.Run 		: makeRunAi(trigger);	break;
		case AgentInstructionTrigger.InstructionType.Jump 		: makeJumpAi(trigger);	break;
		case AgentInstructionTrigger.InstructionType.DropOff 	: makeDropOff(trigger);	break;
		}

	}


	private void makeRunAi(AgentInstructionTrigger target){
		target.direction = (Direction) EditorGUILayout.EnumPopup ("Direction", target.direction);
		target.moveHoldingLenght = EditorGUILayout.FloatField ("move distance (x)", target.moveHoldingLenght);


	}

	private void makeJumpAi(AgentInstructionTrigger target){
		target.direction = (Direction) EditorGUILayout.EnumPopup ("Direction", target.direction);
		target.moveHoldingLenght = EditorGUILayout.FloatField ("hold move lenght (x)", target.moveHoldingLenght);
		target.jumpHoldingLenght = EditorGUILayout.FloatField ("hold jump lenght (x)", target.jumpHoldingLenght);

		if (target.runBeforeInstruction != 0) {
			string code = "makeRunJump (direction: Direction." + target.direction.ToString () + ", runDistance : " + target.runBeforeInstruction 
					+ "f, moveHoldingLenght: " + target.moveHoldingLenght + "f, jumpHoldingLenght: " + target.jumpHoldingLenght + "f);";		
			EditorGUILayout.TextArea (code);
		} else {
			string code = "new JumpInstruction.CreationData () {startingDirection=Direction." + target.direction.ToString () 
				+ ", holdLenght="+target.jumpHoldingLenght+"f, moveLenght="+target.moveHoldingLenght+"f};" ;
			EditorGUILayout.TextArea (code);
		}

	}

	private void makeDropOff(AgentInstructionTrigger target){
		target.moveHoldingLenght = EditorGUILayout.FloatField ("hold move lenght (x)", target.moveHoldingLenght);
		target.direction = (Direction) EditorGUILayout.EnumPopup ("Direction", target.direction);

		target.distanceToStartRunningAgain = EditorGUILayout.FloatField ("Drop Lenght To Start Moving Again (x)", target.distanceToStartRunningAgain);
		target.endDirection = (Direction) EditorGUILayout.EnumPopup ("End Drop Direction", target.endDirection);

		target.totalDropOff = EditorGUILayout.FloatField ("Total Drop Off (y)", target.totalDropOff);

		if (target.runBeforeInstruction != 0) {
			string code = "makeDropOff(Direction." + target.direction.ToString () + ", " + target.runBeforeInstruction 
				+ "f, " + target.moveHoldingLenght + "f, Direction." + target.endDirection.ToString () 
					+ ", " + target.distanceToStartRunningAgain + "f, " + target.totalDropOff + "f);";
			EditorGUILayout.TextArea (code);
		} else {
			string code = "new DropOffInstruction.CreationData (){firstDirection=Direction." + target.direction.ToString () 
				+ ", moveXLenght=" + target.moveHoldingLenght + "f,moveAgainAfterYMoved=" + target.distanceToStartRunningAgain 
					+ "f, endDropDirection=Direction." + target.endDirection.ToString () + ", totalDrop=" + target.totalDropOff + "f};" ;
			EditorGUILayout.TextArea (code);
		}
	}
}
