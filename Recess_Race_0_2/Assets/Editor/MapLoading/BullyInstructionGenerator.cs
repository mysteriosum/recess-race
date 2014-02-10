using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BullyInstructionGenerator : MonoBehaviour {

	private List<Plateform> plateforms;

	private GameObject tileHover;

	private Transform parent;

	private bool workingOnATile;
	private int tileMaxX;
	private int tileMinX;
	private int tileY;

	public BullyInstructionGenerator(){
		tileHover = Resources.Load<GameObject> ("TileHover");
		plateforms = new List<Plateform> ();
	}

	public void setGameObjectParent(Transform parent){
		this.parent = parent;
	}

	public void addTile(int x, int y, int id){
		if (!workingOnATile) {
			this.tileMinX = x;
			this.tileMaxX = x;
			this.tileY = y;
			this.workingOnATile = true;
		} else if (tileMaxX + 1 == x) { //right after where we are
			tileMaxX ++;
		} else {
			this.workingOnATile = false;

			addTileHover();

			this.tileMinX = x;
			this.tileMaxX = x;
			this.tileY = y;
			this.workingOnATile = true;
		}
	}

	private void addTileHover(){
		GameObject newTileHover = (GameObject)GameObject.Instantiate (this.tileHover);
		newTileHover.transform.parent = parent;
		float width = tileMaxX - tileMinX;
		float center = width/2;
		newTileHover.transform.Translate (tileMinX + center, tileY, 0);
		newTileHover.transform.localScale = new Vector3(width + 1, 1, 0);

		Bounds bound = ((SpriteRenderer)newTileHover.GetComponent<SpriteRenderer> ()).bounds;
		Plateform plateform = new Plateform (newTileHover.transform, bound);
		this.plateforms.Add (plateform);
	}

	public void done(){
		addTileHover ();
		removeUselessPlateform ();
	}

	
	public void removeUselessPlateform(){
		List<Plateform> plateformsToRemove = new List<Plateform> ();
		foreach (Plateform plateformChecking in this.plateforms) {
			foreach (Plateform plateformOver in this.plateforms) {
				if(plateformChecking.isUnder(plateformOver)){
					if(isPlateformCompletlyCovered(plateformChecking,plateformOver)){
						plateformsToRemove.Add(plateformChecking);
						break;
					}
				}else{
					break;
				}
			}		
		}
		foreach (Plateform p in plateformsToRemove) {
			this.plateforms.Remove(p);
			GameObject.DestroyImmediate(p.transform.gameObject);
		}
	}

	private bool isPlateformCompletlyCovered(Plateform p1, Plateform p2){
		//Debug.Log ("--");
		for (float x = p1.bound.min.x; x <= p1.bound.max.x; x++) {

			float yOver = p1.transform.position.y + 0.5f;
			//Debug.Log (x + "," + yOver);
			Bounds bound = new Bounds (new Vector3(x, yOver, 0), Vector3.one);			
			if(!bound.Intersects(p2.bound)){
				return false;
			}
		}
		return true;
	}
}
