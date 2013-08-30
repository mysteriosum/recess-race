using UnityEngine;
using System.Collections;

public enum PowerEnum {
	Otto = 0,
	Pinky = 1,
	BoogerBoy = 2,
}

public class Pickup : Platformer {
	
	public bool cycles;
	public uint cycleTiming;
	private uint cycleTimer;
	
	private MovementVariables motor = new MovementVariables(
		
			1.25f, 				//wSpeed
			2.25f,  			//rSpeed
			3f,  				//sSpeed
			0.09375f,  			//accel
			0.0625f,  			//decel
			0.15625f,  			//skidDecel
			0.3125f,  			//running SkidDecel
			3.0f,  				//jHeight
			5.5f,  				//jExtraHeight
			1.25f,  			//airSpeedH
			4.8125f,  			//airSpeedInit
			4f,  				//fallSpeedMax
			0.1875f,  			//gravity
			0.375f 				//gravityPlus
		);
	public PowerEnum power;
	
	
	// Use this for initialization
	void Start () {
		if (cycles){
			cycleTimer = cycleTiming;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (cycleTimer > 0){
			cycleTimer --;
			if (cycleTimer == 0){
				
				power ++;
				Debug.Log("Yay it worked!");
			}
		}
	}
}
