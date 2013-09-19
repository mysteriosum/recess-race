using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FitzDetectBox : GizmoDad {
	private GameObject parent;
	private Platformer pScript;
	private BoxCollider bc;
	
	private List<BoxCollider> colList = new List<BoxCollider>();
	private List<BoxCollider> hardTopList = new List<BoxCollider>();
	private List<BoxCollider> softTopList = new List<BoxCollider>();
	private List<BoxCollider>[] lists = new List<BoxCollider>[3];
	
	// Use this for initialization
	void Start () {
		lists[0] = colList;
		lists[1] = hardTopList;
		lists[2] = softTopList;
		parent = transform.parent.gameObject;
		pScript = parent.GetComponent<Platformer>();
		
		if (!pScript){
			Debug.LogWarning("Your detector box doesn't have a platformer for a parent! WTF!");
		}
		
		bc = GetComponent<BoxCollider>();
		bc.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnTriggerExit (Collider other){
		
		BoxCollider otherBC = other.GetComponent<BoxCollider>();
		int lay = other.gameObject.layer;
		if (lay == 31 || lay == 30 || lay == 29 && otherBC != null){
			
			bool removed = colList.Remove(otherBC);
			if (!removed && lay == 30){
				removed = hardTopList.Remove(otherBC);
			}
			else if (!removed && lay == 29){
				softTopList.Remove(otherBC);
			}
			BoxCollider toRemove;
			foreach (List<BoxCollider> list in lists){
				do {
					toRemove = null;
					foreach (BoxCollider boxo in list){
						if (!boxo.bounds.Intersects(bc.bounds)){
							toRemove = boxo;
						}
					}
					if (toRemove)
						list.Remove(toRemove);
				}
				while (toRemove != null);
			}
			
			
			if (lay == 31 && colList.Count == 0 && hardTopList.Count == 0){
				pScript.DetectorExit(bc, otherBC);
			}
			else if (lay == 30 && hardTopList.Count == 0 && colList.Count == 0){
				pScript.DetectorExit(bc, otherBC);
			}
			else if (lay == 29 && softTopList.Count == 0 && name == "topDetector"){
				pScript.DetectorExit(bc, otherBC);
			}
		}
	}
	
	void OnTriggerEnter (Collider other){
		if (!pScript) return;
		int lay = other.gameObject.layer;
		BoxCollider otherBC = other.GetComponent<BoxCollider>();
		if (otherBC != null){
			
			if (colList.Count == 0 || lay == LayerMask.NameToLayer("danger") || other.tag == "checkpoint"){
				pScript.DetectorEnter(bc, otherBC);
				
			}
			
			else if (pScript.Velocity.y < 0 && lay == LayerMask.NameToLayer("softBottom")){
				pScript.DetectorEnter(bc, otherBC);
			}
			
			if ((lay == 31) && !colList.Contains(otherBC)){
				colList.Add(otherBC);
			}
			else if (lay == 30 && !hardTopList.Contains(otherBC) && name == "downDetector"){
				hardTopList.Add(otherBC);
			}
			else if (lay == 29 && !softTopList.Contains(otherBC) && name == "topDetector"){
				softTopList.Add(otherBC);
			}
		}
	}
	
	public bool KnowsOf(BoxCollider collision){
		return colList.Contains(collision);
	}
	
}
