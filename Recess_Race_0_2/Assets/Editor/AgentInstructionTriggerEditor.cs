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
		trigger.data.type = (InstructionCreationData.InstructionType) EditorGUILayout.EnumPopup ("Instruction type",trigger.data.type);

		switch (trigger.data.type) {
		case InstructionCreationData.InstructionType.Run 		: makeRunAi(trigger);	break;
		case InstructionCreationData.InstructionType.Jump 		: makeJumpAi(trigger);	break;
		case InstructionCreationData.InstructionType.DropOff 	: makeDropOff(trigger);	break;
		}
	}


	private void makeRunAi(AgentInstructionTrigger target){
		target.data.direction = (Direction) EditorGUILayout.EnumPopup ("Direction", target.data.direction);
		target.data.moveHoldingLenght = EditorGUILayout.FloatField ("move distance (x)", target.data.moveHoldingLenght);


	}

	private void makeJumpAi(AgentInstructionTrigger target){	
		target.data.direction = (Direction) EditorGUILayout.EnumPopup ("Direction", target.data.direction);
		target.data.moveHoldingLenght = EditorGUILayout.FloatField ("hold move lenght (x)", target.data.moveHoldingLenght);
		target.data.jumpHoldingLenght = EditorGUILayout.FloatField ("hold jump lenght (x)", target.data.jumpHoldingLenght);

		target.data.distanceToStartRunningAgain = EditorGUILayout.FloatField ("Move Again After Y Moved (y)", target.data.distanceToStartRunningAgain);
		target.data.endDirection = (Direction) EditorGUILayout.EnumPopup ("Move Again Direction", target.data.endDirection);
		target.data.totalDistanceAfterMoveAgain = EditorGUILayout.FloatField ("Move Again distance(x)", target.data.totalDistanceAfterMoveAgain);

		string needRunCharge = (target.runBeforeInstruction != 0)?"true":"false";
		string code = "new InstructionCreationData() {type=InstructionCreationData.InstructionType.Jump,direction=Direction." + target.data.direction.ToString () 
			+ ", distanceToStartRunningAgain ="+target.data.distanceToStartRunningAgain+"f, endDirection=Direction."+ target.data.endDirection.ToString () 
				+ ", totalDistanceAfterMoveAgain=" + target.data.totalDistanceAfterMoveAgain + "f" + ", jumpHoldingLenght="
				+target.data.jumpHoldingLenght+"f, moveHoldingLenght="+target.data.moveHoldingLenght+"f, needRunCharge="+needRunCharge+"};" ;
		EditorGUILayout.TextArea (code);

	}

	private void makeDropOff(AgentInstructionTrigger target){
		target.data.moveHoldingLenght = EditorGUILayout.FloatField ("hold move lenght (x)", target.data.moveHoldingLenght);
		target.data.direction = (Direction) EditorGUILayout.EnumPopup ("Direction", target.data.direction);

		target.data.distanceToStartRunningAgain = EditorGUILayout.FloatField ("Drop Lenght To Start Moving Again (x)", target.data.distanceToStartRunningAgain);
		target.data.endDirection = (Direction) EditorGUILayout.EnumPopup ("End Drop Direction", target.data.endDirection);
		target.data.totalDistanceAfterMoveAgain = EditorGUILayout.FloatField ("Total Drop Off (y)", target.data.totalDistanceAfterMoveAgain);

		string needRunCharge = (target.runBeforeInstruction != 0)?"true":"false";
		string code = "new InstructionCreationData() {type=InstructionCreationData.InstructionType.DropOff,direction=Direction." + target.data.direction.ToString () 
			+ ", distanceToStartRunningAgain ="+target.data.distanceToStartRunningAgain+"f, endDirection=Direction."+ target.data.endDirection.ToString () 
				+ ", totalDistanceAfterMoveAgain=" + target.data.totalDistanceAfterMoveAgain + "f" + ", jumpHoldingLenght="
				+target.data.jumpHoldingLenght+"f, moveHoldingLenght="+target.data.moveHoldingLenght+"f, needRunCharge="+needRunCharge+"};" ;
		EditorGUILayout.TextArea (code);
	}
}
