using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	
	private TextMesh mesh;
	private float time;
	
	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
		mesh.text = "00:00";
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		
		int minutes = (int)Mathf.Floor(time / 60);
		int seconds = (int)Mathf.Floor(time % 60);
		int centiseconds = (int)Mathf.Floor((time * 100) % 100);
		
		mesh.text = (minutes > 9? "" : "0") + minutes.ToString() + ":" + (seconds > 9? "" : "0") + seconds.ToString() + ":" + (centiseconds > 9? "" : "0") + centiseconds.ToString();
	}
	
	
	
}
