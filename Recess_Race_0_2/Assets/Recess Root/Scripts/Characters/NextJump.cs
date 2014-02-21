using UnityEngine;
using System.Collections;

public class NextJump {


	public float walkDistance;
	public float holdLength;
	public bool onEnter;
	public bool onExit;
	public bool onCentre;
	
	public BullyInstructionConfiguration instruction;
	
	
	public NextJump(BullyInstructionConfiguration instruction, float holdLength)
	{
		this.holdLength = holdLength;
		this.onCentre = true;
		this.onExit = false;
		this.onEnter = false;
		this.instruction = instruction;
	}
	
	public NextJump(BullyInstructionConfiguration instruction, float holdLength, bool onEnter)
	{
		this.holdLength = holdLength;
		this.onEnter = onEnter;
		this.onExit = !onEnter;
		this.onCentre = false;
		this.instruction = instruction;
	}
	
	public NextJump(BullyInstructionConfiguration instruction, float holdLength, bool onCentre, bool onEnter, bool onExit)
	{
		this.holdLength = holdLength;
		this.onEnter = onEnter;
		this.onExit = onExit;
		this.onCentre = onCentre;
		this.instruction = instruction;
	}
}
