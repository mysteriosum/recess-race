using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class InstructionCreationData : ScriptableObject{
	public InstructionCreationData nextInstructionCreationData;

	public void OnEnable() { hideFlags = HideFlags.HideAndDontSave; }
}
