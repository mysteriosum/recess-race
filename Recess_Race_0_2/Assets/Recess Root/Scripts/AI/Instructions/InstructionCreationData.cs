using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class InstructionCreationData{
	public InstructionCreationData nextInstructionCreationData;
	public bool needRunCharge = false;
}