using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour {
	
	
	private AudioSource source;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void PlaySound(AudioClip clip){
		source.clip = clip;
		source.Play ();
	}
}
