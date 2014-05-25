using UnityEngine;
using System.Collections;

public class Bully : MonoBehaviour {
	//public string message = "Get off me, gaylord!";
	
	
	public string[] messages;
	public string monitorMessage;
	private float showMessageTiming = 2.5f;
	
	private Speaker speaker;
	private static bool speaking = false;
	private bool threatening = true;
	private DamageScript damager;
	
	private Vector3 fireAttackOffset = new Vector3(0.25f, 1.15f, -1f);
	
	Collider2D monitorCol = null;
	void Start(){
		speaker = GetComponent<Speaker>();
		damager = GetComponent<DamageScript>();
	}
	
	void Update(){
		if (monitorCol != null && !monitorCol.OverlapPoint(transform.position)){
			threatening = true;
			if (damager != null)
				damager.enabled = true;
			monitorCol = null;
		}
	}
	
	void CollideWithFitz(){
		if (speaking) return;
		int i = Random.Range (0, messages.Length - 1);
		string willSay = threatening? messages[i] : monitorMessage;
		speaker.Speak (name + ": " + willSay, showMessageTiming, Done);
		speaking = true;
	}
	
	void Done(){
		speaking = false;
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (threatening && other.GetComponent<SafeZone>()){
			threatening = false;
			if (damager)
				damager.enabled = false;
			
			monitorCol = other;
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		SafeZone zone = other.GetComponent<SafeZone>();
		if (zone != null){
			threatening = true;
			damager.enabled = true;
			Debug.Log ("Threatening is true");
		}
	}
	
	public void FlameEffect () {
		Object flamer = Resources.Load("FireAttack");
		GameObject flobject = Instantiate(flamer) as GameObject;
		flobject.transform.parent = transform;
		flobject.transform.localPosition = fireAttackOffset;
	}
	/*
	void OnGUI(){
		if (showMessageTimer > 0){
			Rect messageRect = 
				new Rect(Screen.width/2 - (Screen.width * (messageWidthPercent)/2), Screen.height - (Screen.height * messageHeightPercent), 
				Screen.width * messageWidthPercent, Screen.height * messageHeightPercent);
			GUIStyle style = new GUIStyle(RecessCamera.cam.hud.skin.box);
			//style.contentOffset = new Vector2(messageRect.width * 0.15f, messageRect.height * 0.15f);
			style.wordWrap = true;
			style.fontSize = fontSize;
			GUI.Box(messageRect, showMessage, style);
		}
	}*/
}
