using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenEffectSystem : MonoBehaviour {

	private static List<ScreenEffect> screenEffects;
	private static List<ScreenEffect> screenEffectsToAdd;
	private static List<ScreenEffect> screenEffectsToRemove;
	
	
	void Start () {
		ScreenEffectSystem.screenEffects = new List<ScreenEffect> ();
		ScreenEffectSystem.screenEffectsToAdd = new List<ScreenEffect> ();
		ScreenEffectSystem.screenEffectsToRemove = new List<ScreenEffect> ();
	}

	public static void AddScreenEffect(ScreenEffect screenEffect){
		if (screenEffects == null) {
			Debug.LogError("PopupSystem in no GameObject! (The maploader should have loaded that.");
			return;
		}
		screenEffectsToAdd.Add (screenEffect);
	}

	void OnGUI  () {
		addNewEffect ();
		activateAllEffect ();
		removeOldEffects ();
		addNewEffect ();
	}

	private void addNewEffect(){
		foreach(ScreenEffect s in screenEffectsToAdd){
			screenEffects.Add(s);
		}
		screenEffectsToAdd.Clear();
	}

	private void activateAllEffect(){
		foreach(ScreenEffect s in screenEffects){
			s.OnGui(Time.deltaTime);
			if(s.done){
				screenEffectsToRemove.Add(s);
			}
		}
	}

	private void removeOldEffects(){
		foreach (ScreenEffect toRemove in screenEffectsToRemove) {
			screenEffects.Remove(toRemove);
		}
		screenEffectsToRemove.Clear();
	}


}
