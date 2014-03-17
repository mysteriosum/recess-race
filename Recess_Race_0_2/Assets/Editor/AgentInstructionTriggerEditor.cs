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

		target.distanceToStartRunningAgain = EditorGUILayout.FloatField ("Move Again After Y Moved (y)", target.distanceToStartRunningAgain);
		target.endDirection = (Direction) EditorGUILayout.EnumPopup ("Move Again Direction", target.endDirection);
		target.totalDistanceAfterMoveAgain = EditorGUILayout.FloatField ("Move Again distance(x)", target.totalDistanceAfterMoveAgain);

		string needRunCharge = (target.runBeforeInstruction != 0)?"true":"false";
		string code = "new JumpInstruction.CreationData () {startingDirection=Direction." + target.direction.ToString () 
			+ ", moveAgainAfterYMoved ="+target.distanceToStartRunningAgain+"f, endDirection="+ target.endDirection.ToString () 
			+ ", moveAgainMoveLenght=" + target.totalDistanceAfterMoveAgain + "f"
			+ ", holdLenght="+target.jumpHoldingLenght+"f, moveLenght="+target.moveHoldingLenght+"f, needRunCharge="+needRunCharge+"};" ;
		EditorGUILayout.TextArea (code);

	}

	private void makeDropOff(AgentInstructionTrigger target){
		target.moveHoldingLenght = EditorGUILayout.FloatField ("hold move lenght (x)", target.moveHoldingLenght);
		target.direction = (Direction) EditorGUILayout.EnumPopup ("Direction", target.direction);

		target.distanceToStartRunningAgain = EditorGUILayout.FloatField ("Drop Lenght To Start Moving Again (x)", target.distanceToStartRunningAgain);
		target.endDirection = (Direction) EditorGUILayout.EnumPopup ("End Drop Direction", target.endDirection);
		target.totalDistanceAfterMoveAgain = EditorGUILayout.FloatField ("Total Drop Off (y)", target.totalDistanceAfterMoveAgain);

		string needRunCharge = (target.runBeforeInstruction != 0)?"true":"false";
		string code = "new DropOffInstruction.CreationData (){firstDirection=Direction." + target.direction.ToString () + ", moveXLenght=" 
			+ target.moveHoldingLenght + "f,moveAgainAfterYMoved=" + target.distanceToStartRunningAgain + "f, endDropDirection=Direction." 
				+ target.endDirection.ToString () + ", totalDrop=" + target.totalDistanceAfterMoveAgain + "f, needRunCharge="+needRunCharge+"};" ;
		EditorGUILayout.TextArea (code);
	}
}
