using UnityEngine;
using System.Collections;

public class Sounds {
	
	public AudioClip cameraSound;
	public AudioClip birds;
	public AudioClip children ;
	public AudioClip gun;
	public AudioClip whistle;
	public AudioClip flag;
	
	public AudioClip song;
	public AudioClip losePower;
	public AudioClip run;
	public AudioClip flap;
	public AudioClip roll;
	public AudioClip wallJump;
	public AudioClip jump;
	public AudioClip land;
	public AudioClip collect;
	public AudioClip chirp;
	public AudioClip menuSelect;
	public AudioClip stun;
	
	public Sounds(){
		cameraSound = Resources.Load("Sounds/Camera 2") as AudioClip;
		birds = Resources.Load("Sounds/birds_001") as AudioClip;
		children = Resources.Load("Sounds/Children Cheer") as AudioClip;
		gun = Resources.Load("Sounds/Gun Hand Shot 3") as AudioClip;
		whistle = Resources.Load("Sounds/Whistle 2") as AudioClip;
		flag = Resources.Load("Sounds/Flag") as AudioClip;
		song = Resources.Load("Recess Race") as AudioClip;
		losePower = Resources.Load("Sounds/eatorb") as AudioClip;
		run = Resources.Load("Sounds/Sounds-Run") as AudioClip;
		roll = Resources.Load("Sounds/Sounds-Roll") as AudioClip;
		flap = Resources.Load("Sounds/Sound-Pinky") as AudioClip;
		jump = Resources.Load("Sounds/Sound-Jump") as AudioClip;
		wallJump = Resources.Load("Sounds/Sound-WallJump") as AudioClip;
		land = Resources.Load("Sounds/Sound-Land") as AudioClip;
		collect = Resources.Load("Sounds/Sound-Collect") as AudioClip;
		chirp = Resources.Load("Sounds/Sound-RetroChirp") as AudioClip;
		menuSelect = Resources.Load("Sounds/Sound-MenuSelect") as AudioClip;
		stun = Resources.Load ("Sounds/Sound-Stunned") as AudioClip;
	}
	
}
