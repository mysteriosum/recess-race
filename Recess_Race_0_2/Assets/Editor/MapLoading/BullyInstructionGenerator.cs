using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Linq;

public class BullyInstructionGenerator {

	private List<Plateform> plateforms;

	private GameObject tileHoverPrefab;
	private GameObject bullyInstructionPrefab;

	private Transform parent;

	private bool workingOnATile;
	private int tileMaxX;
	private int tileMinX;
	private int tileY;

	private bool[,] pathingMap;

	public BullyInstructionGenerator(Dimension mapDimension){
		tileHoverPrefab = Resources.Load<GameObject> ("TilePlateformHover");
		plateforms = new List<Plateform> ();
		pathingMap = new bool[mapDimension.width,mapDimension.height];
	}

	public void setGameObjectParent(Transform parent){
		this.parent = parent;
	}

	public void addTile(int x, int y, int id){
		this.pathingMap [x, y] = true;
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

	
	public void doneLoadingTiles(){
		addTileHover ();
		removeUselessPlateform ();
	}


	private void addTileHover(){
		GameObject newTileHover = (GameObject)GameObject.Instantiate (this.tileHoverPrefab);
		newTileHover.name = "Plateform";
		newTileHover.transform.parent = parent;
		float width = tileMaxX - tileMinX;
		float center = width/2;
		newTileHover.transform.Translate (tileMinX + center, tileY, 0);
		newTileHover.transform.localScale = new Vector3(width + 1, 1, 0);

		Plateform plateform = newTileHover.GetComponent<Plateform> ();
		this.plateforms.Add (plateform);
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
		Bounds b1 = p1.getBound ();
		Bounds b2 = p2.getBound ();
		for (float x = b1.min.x; x <= b1.max.x; x++) {

			float yOver = p1.transform.position.y + 0.5f;
			//Debug.Log (x + "," + yOver);
			Bounds bound = new Bounds (new Vector3(x, yOver, 0), Vector3.one);			
			if(!bound.Intersects(b2)){
				return false;
			}
		}
		return true;
	}


	public void loadWaypoints(XElement waypoints, Map map){
		var objects = waypoints.Elements ();
		int tileHeight = map.tileDimension.height;
		foreach (var item in objects) {
			int x = Int32.Parse(item.Attribute("x").Value) / tileHeight;
			int y = map.mapDimension.height - Int32.Parse(item.Attribute("y").Value) / tileHeight - 2;
			var propertyId = item.Descendants().First (e => e.Name == "property" && e.Attribute("name").Value == "id");
			int id = Int32.Parse(propertyId.Attribute("value").Value);
			createWaypoint(x, y, id, objects.Count());
		}

	}

	private void createWaypoint(int x, int y, int id, int nbWayPoints){
		foreach (var plateform in this.plateforms) {
			Bounds bound = new Bounds(new Vector3(x,y,0), new Vector3(2,2,0));
			Bounds plateformBound = plateform.getBound();
			if(bound.Intersects(plateformBound)){
				var spriteRenderer = plateform.transform.gameObject.GetComponent<SpriteRenderer>();
				plateform.transform.gameObject.name = "Plateform (wp #" + id + ")";
				spriteRenderer.color = new Color(((float) id) / nbWayPoints,0,0, 0.6f);
			}
		}
	}


	public void linkPlateforms(){
		/*foreach (var plateform in this.plateforms) {

		}*/
	}

}
