using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FitzDetectBox : GizmoDad {
	private GameObject parent;
	private Platformer pScript;
	private BoxCollider bc;
	
	private List<BoxCollider> colList = new List<BoxCollider>();
	
	
	// Use this for initialization
	void Start () {
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
		if (other.gameObject.layer == 31 || other.gameObject.layer == 30 || other.gameObject.layer == 29 && otherBC != null){
		
			colList.Remove(otherBC);
			BoxCollider toRemove;
			do {
				toRemove = null;
				foreach (BoxCollider boxo in colList){
					if (!boxo.bounds.Intersects(bc.bounds)){
						toRemove = boxo;
					}
				}
				if (toRemove)
					colList.Remove(toRemove);
			}
			while (toRemove != null);
			
			
			if (colList.Count == 0){
				pScript.DetectorExit(bc, otherBC);
			}
		}
	}
	
	void OnTriggerEnter (Collider other){
		if (!pScript) return;
		BoxCollider otherBC = other.GetComponent<BoxCollider>();
		if (otherBC != null){
			
			if (colList.Count == 0 || other.gameObject.layer == LayerMask.NameToLayer("danger") || other.tag == "checkpoint"){
				pScript.DetectorEnter(bc, otherBC);
			}
			
			if ((other.gameObject.layer == 31 || other.gameObject.layer == 30 || other.gameObject.layer == 29) && !colList.Contains(otherBC)){
				colList.Add(otherBC);
			}
		}
	}
	
	public bool KnowsOf(BoxCollider collision){
		return colList.Contains(collision);
	}
	
}
