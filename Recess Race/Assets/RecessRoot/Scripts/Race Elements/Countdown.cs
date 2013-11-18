using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {
	
	private tk2dSprite sprite;
	
	public GameObject fitzwilliam;
	public GameObject placeholder;
	public GameObject bullyInstructions;
	
	private int goAt = 4;
	private int lightIndex = 1;
	private float timer = 0f;
	private bool started = false;
	
	// Use this for initialization
	void Start () {
		if (!fitzwilliam){
			Debug.Log("Fitz didn't show up, so we're canceling the race");
			Destroy(gameObject);
		}
		sprite = GetComponent<tk2dSprite>();
		fitzwilliam.SetActive(false);
		placeholder = Instantiate (placeholder, fitzwilliam.transform.position, fitzwilliam.transform.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (!started){
			timer += Time.deltaTime;
			
			if (timer > lightIndex){
				if (lightIndex == goAt){
					Destroy(placeholder);
					fitzwilliam.SetActive(true);
					started = true;
					
				}
				else {
					sprite.SetSprite("lights_" + lightIndex.ToString());
					lightIndex ++;
				}
			}
		}
		
	}
}
