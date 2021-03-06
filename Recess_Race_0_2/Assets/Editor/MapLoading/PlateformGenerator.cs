﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Linq;

public class PlateformGenerator {

	private List<Plateform> plateforms;

	private GameObject tileHoverPrefab;
	
	private Transform plateformGroupParent;

	private bool workingOnATile;
	private int tileMaxX;
	private int tileMinX;
	private int tileY;

    private int nextTileId = 0;

    private Dimension mapDimension;
    private Map map;

    public static bool debug = false;
	public static int removed = 0;

	public PlateformGenerator(Map map){
        this.mapDimension = map.mapDimension;
        this.map = map;
        tileHoverPrefab = Resources.Load<GameObject>("TilePlateformHover");
		plateforms = new List<Plateform> ();
	}

	public void setGameObjectParent(Transform parent){
		GameObject plateformGameObject = new GameObject();
		plateformGameObject.name = "Plateforms";
		plateformGameObject.transform.parent = parent;
		plateformGroupParent = plateformGameObject.transform;
	}

	public void addTile(int x, int y, int id){
        this.map.pathingMap[x][y] = true;

		if (!workingOnATile) {
            prepareNextTile(x, y);
        } else if (tileMaxX + 1 == x && tileY == y) { //right after where we are
            if (tileOver(x, y)) {
                addTileHover();
                prepareNextTile(x, y);
            } else {
                tileMaxX++;
            }
        } else {
            addTileHover();
            prepareNextTile(x, y);
		}
	}

    private bool tileOver(int x, int y){
		return (y != mapDimension.height-1 && map.pathingMap[x][y + 1]) || (y != mapDimension.height-2 && map.pathingMap[x][y + 2]);
    }

    private void prepareNextTile(int x, int y)
    {
        if (tileOver(x, y)) {
            this.workingOnATile = false;
        } else {
            this.tileMinX = x;
            this.tileMaxX = x;
            this.tileY = y;
            this.workingOnATile = true;
        }
    }

	
	public void doneLoadingTiles(){
        addTileHover();
		removeUselessPlateform ();
	}


    private void addTileHover(){
        if (!workingOnATile) return;
		GameObject newTileHover = (GameObject)GameObject.Instantiate (this.tileHoverPrefab);
        newTileHover.name = "Plateform " + nextTileId;
		newTileHover.transform.parent = plateformGroupParent;
		float width = tileMaxX - tileMinX;
		float center = width/2;
		newTileHover.transform.Translate (tileMinX + center, tileY + 1, 0);
		newTileHover.transform.localScale = new Vector3(width + 1, 1, 0);

		Plateform plateform = newTileHover.GetComponent<Plateform> ();
        plateform.id = nextTileId++;
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
        //Debug.Log("To remove " + plateformsToRemove.Count + " plateforms.");
		foreach (Plateform p in plateformsToRemove) {
            this.plateforms.RemoveAll(item => item.id == p.id);
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



	public void loadAIPlateformRemove(XElement listToRemove, Map map){
		if (listToRemove == null)return;
		removed = 0;
		int tileHeight = map.tileDimension.height;
		foreach (var item in listToRemove.Elements ()) {
			float x = Int32.Parse(item.Attribute("x").Value) / (float)tileHeight;
			float y = map.mapDimension.height - Int32.Parse(item.Attribute("y").Value) / (float)tileHeight - 1;
			removePlateform(x, y);
		}
		verbose ("plateform removed " + removed + " / " + listToRemove.Elements().Count());
	}

	private void removePlateform(float x, float y){
		int idToRemove = -1;
		Bounds bound = new Bounds(new Vector3(x,y,0), new Vector3(2,2,0));
		for (int i = 0; i < this.plateforms.Count; i++) {
			Bounds plateformBound = this.plateforms[i].getBound();
			if(bound.Intersects(plateformBound)){
				idToRemove = i;
				GameObject.DestroyImmediate(this.plateforms[i].gameObject);
			}
		}
		if (idToRemove != -1) {
			removed ++;
			this.plateforms.RemoveAt(idToRemove);		
		}
	}



	public void loadWaypoints(XElement waypoints, Map map){
		var objects = waypoints.Elements ();
		int tileHeight = map.tileDimension.height;
		foreach (var item in objects) {
			int x = Int32.Parse(item.Attribute("x").Value) / tileHeight;
			int y = map.mapDimension.height - Int32.Parse(item.Attribute("y").Value) / tileHeight - 1;
			try{
				var propertyId = item.Descendants().First (e => e.Name == "property" && e.Attribute("name").Value == "id");
				int id = Int32.Parse(propertyId.Attribute("value").Value);
				createWaypoint(x, y, id, objects.Count());
			}catch(InvalidOperationException){
				Debug.LogError ("Invalid Waypoint : Missing property id");
			}

		}

	}

	private void createWaypoint(int x, int y, int id, int nbWayPoints){
		Bounds bound = new Bounds(new Vector3(x,y,0), new Vector3(2,2,0));
		foreach (var plateform in this.plateforms) {
			Bounds plateformBound = plateform.getBound();
			if(bound.Intersects(plateformBound)){
				var spriteRenderer = plateform.transform.gameObject.GetComponent<SpriteRenderer>();
				plateform.transform.gameObject.name = "Plateform " + plateform.id + " - wp #" + id + "";
				plateform.waypointId = id;
				spriteRenderer.color = new Color(((float) id) / nbWayPoints,0,0, 0.6f);
				if(id==nbWayPoints){
					plateform.isLastWayPoint = true;
				}
			}
		}
	}

    public void hideAllPlatefoms() {
        foreach (var plateform in this.plateforms) {
            GameObject.DestroyImmediate(plateform.GetComponent<SpriteRenderer>());
            plateform.showGismos = false;
        }
    }

	public void linkPlateforms(){
		foreach (var plateform in this.plateforms) {
            foreach (var p2 in this.plateforms) {
                if (plateform.Equals(p2)) continue;

                if (isInMaximumJumpPosibility(plateform, p2)) {
                    findReachablePlateform(plateform, p2);
                }
            }
		}
	}

	public void linkPlateform(Plateform plateform){
		foreach (var p2 in this.plateforms) {
			if (plateform.Equals(p2)) continue;
			
			if (isInMaximumJumpPosibility(plateform, p2)) {
				findReachablePlateform(plateform, p2);
			}
		}
	}

	private void findReachablePlateform(Plateform fromPlateform, Plateform toPlateform){
        for (float x = fromPlateform.getLeftCornerPosition().x; x < fromPlateform.getLeftCornerPosition().x + fromPlateform.getWidth(); x++) {
            Vector3 from = new Vector3(x, fromPlateform.transform.position.y, fromPlateform.transform.position.z);
            for (float x2 = toPlateform.getLeftCornerPosition().x; x2 < toPlateform.getLeftCornerPosition().x + toPlateform.getWidth(); x2++) {
                Vector3 to = new Vector3(x2, toPlateform.transform.position.y, toPlateform.transform.position.z);
                int distanceX = (int)(to.x - from.x);
                int distanceY = (int)(to.y - from.y);                 
				if (distanceX == 0 || Mathf.Abs(distanceX) > 13 || distanceY >= PossibleJumpMaps.yUpHeightIncludingZero 
					|| distanceY < -PossibleJumpMaps.yDownHeight) continue;

				List<JumpRunCreationData> possibleJumps = PossibleJumpMaps.getPossible(distanceX, distanceY);
				if (possibleJumps == null) continue;
				foreach (JumpRunCreationData jump in possibleJumps) {
					bool[,] pathingMap;
					SplitDirection splitDirection;
					SplitDirection checkDirection;
					if (from.y > to.y) { // Drop down
						if(jump.instruction.type.Equals(InstructionCreationData.InstructionType.Jump)){
							splitDirection = (from.x < to.x) ? SplitDirection.BottomRight : SplitDirection.BottomLeft;
							checkDirection = (from.x < to.x) ? SplitDirection.BottomLeft : SplitDirection.BottomRight;
							Vector3 vJump = new Vector3(from.x, from.y - Math.Abs(distanceY) , 0);
							pathingMap = this.map.splitTo(splitDirection, vJump, new Dimension(14, Math.Abs(distanceY) + 7));
						}else{
							splitDirection = (from.x < to.x) ? SplitDirection.TopRight : SplitDirection.TopLeft;
							checkDirection = (from.x < to.x) ? SplitDirection.TopLeft : SplitDirection.TopRight;
							Vector3 vDrop = new Vector3(from.x, from.y +1, 0);
							pathingMap = this.map.splitTo(splitDirection, vDrop, new Dimension(14, Math.Abs(distanceY) + 2));
						}
					}else{ // Going up
						splitDirection = (from.x < to.x) ? SplitDirection.BottomRight : SplitDirection.BottomLeft;
						checkDirection = (from.x < to.x) ? SplitDirection.BottomLeft : SplitDirection.BottomRight;
						pathingMap = this.map.splitTo(splitDirection, from, new Dimension(14, 7));
					}


					if(jump.instruction.needRunCharge){
						if (from.x < to.x){
							if(!canOverRunFromLeft(x,from.y))  continue;
						}else{
							if(!canOverRunFromRight(x,from.y)) continue;
						}
					}
					try{
						if (!jump.jumpingPath.collideWith(checkDirection, pathingMap)) {
							fromPlateform.addLinksToPlateform(toPlateform, jump.direction, from, jump.instruction);
						}	
					}catch(IndexOutOfRangeException){
						//Debug.LogError("Plateform " + fromPlateform.name + " to " + toPlateform.name + " - To close of map edge");
					}

				}
            }
        }
			
	}

	private bool canOverRunFromLeft(float x, float y){
		if(x-1 < 0 || map.pathingMap[(int)x-1][(int)y]) return false; 
		if(y-1 < 0 || !map.pathingMap[(int)x-1][(int)y-1]) return false;
		return true;
	}
	private bool canOverRunFromRight(float x, float y){
		if(x+1 > map.pathingMap.Length || map.pathingMap[(int)x+1][(int)y]) return false; 
		if(y-1 < 0 || !map.pathingMap[(int)x+1][(int)y-1]) return false;
		return true;
	}

	private bool isInMaximumJumpPosibility(Plateform from, Plateform to){
		Vector3 vFromLeft, vToLeft, vFromRight, vToRight;
		vFromLeft = from.getLeftCornerPosition ();
		vFromRight = from.getRightCornerPosition ();
		vToLeft = to.getLeftCornerPosition ();
		vToRight = to.getRightCornerPosition ();
		
		return isJummpable (vFromRight, vToLeft) || isJummpable (vFromRight, vToRight) || isJummpable(vFromLeft, vToLeft) || isJummpable (vFromLeft, vToRight);
	}

	private bool isJummpable(Vector3 v1, Vector3 v2){
        return Math.Abs(v1.x - v2.x) <= 14 && v2.y - v1.y <= 4;
	}

    private void print(string str) {
        if (debug) {
            Debug.Log(str);
        }
	}
	
	private void verbose(string str) {
		if (MapLoader.verbose) {
			Debug.Log("PlateformGenerator : " + str);
		}
	}

}
