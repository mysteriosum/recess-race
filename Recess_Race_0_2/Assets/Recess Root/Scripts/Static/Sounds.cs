using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour{
	
	public AudioClip cameraSound;
	public AudioClip birds;
	public AudioClip children ;
	public AudioClip gun;
	public AudioClip whistle;
	public AudioClip flag;
	
	public AudioClip song;
	public AudioClip losePower;
	
	void Start(){
		cameraSound = Resources.Load("Sounds/Camera 2") as AudioClip;
		birds = Resources.Load("Sounds/birds_001") as AudioClip;
		children = Resources.Load("Sounds/Children Cheer") as AudioClip;
		gun = Resources.Load("Sounds/Gun Hand Shot 3") as AudioClip;
		whistle = Resources.Load("Sounds/Whistle 2") as AudioClip;
		flag = Resources.Load("Sounds/Flag") as AudioClip;
		song = Resources.Load("Recess Race") as AudioClip;
		losePower = Resources.Load("Sounds/eatorb") as AudioClip;
	}
	
}
