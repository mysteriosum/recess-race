using UnityEngine;
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
		} else if (tileMaxX + 1 == x) { //right after where we are
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

    private bool tileOver(int x, int y)
    {
        return y != mapDimension.height-1 && map.pathingMap[x][y + 1];
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
		if (MapLoader.verbose) {
			Debug.Log("Generated " + this.plateforms.Count + " plateforms.");		
		}
		removeUselessPlateform ();
	}


    private void addTileHover()
    {
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
				Debug.Log ("Invalid Waypoint : Missing property id");
			}

		}

	}

	private void createWaypoint(int x, int y, int id, int nbWayPoints){
		foreach (var plateform in this.plateforms) {
			Bounds bound = new Bounds(new Vector3(x,y,0), new Vector3(2,2,0));
			Bounds plateformBound = plateform.getBound();
			if(bound.Intersects(plateformBound)){
				var spriteRenderer = plateform.transform.gameObject.GetComponent<SpriteRenderer>();
				plateform.transform.gameObject.name = "Plateform " + plateform.id + " - wp #" + id + "";
				plateform.waypointId = id;
				spriteRenderer.color = new Color(((float) id) / nbWayPoints,0,0, 0.6f);
				/*if(id == 1){
					BullyInstructionConfiguration con = new BullyInstructionConfiguration(LengthEnum.none, CommandEnum.right, DifficultyEnum.assured);
					BullyInstruction bi = MapElementHelper.generateInstructionOnCentered(con, plateform, this.bullyInstructionParent);
					bi.gameObject.transform.localScale = plateform.gameObject.transform.localScale;
					bi.gameObject.name = "Starting instruction";
				}*/
			}
		}
	}


	public void linkPlateforms(){
		foreach (var plateform in this.plateforms) {
			findReachablePlateform(plateform);
            //BullyInstructionConfiguration con = new BullyInstructionConfiguration(LengthEnum.hold, CommandEnum.right, DifficultyEnum.assured);
            //MapElementHelper.generateInstructionOnRight(con, plateform, this.bullyInstructionParent);

		}
	}

	private void findReachablePlateform(Plateform fromPlateform){
		foreach (var p in this.plateforms) {
			if(fromPlateform.Equals(p)) continue;

			if(canReach(fromPlateform, p)){
                for (float x = fromPlateform.getLeftCornerPosition().x; x < fromPlateform.getLeftCornerPosition().x + fromPlateform.getWidth(); x++) {
                    Vector3 from = new Vector3(x, fromPlateform.transform.position.y, fromPlateform.transform.position.z);
                    
                    for (float x2 = p.getLeftCornerPosition().x; x2 < p.getLeftCornerPosition().x + p.getWidth(); x2++) {
                        Vector3 to = new Vector3(x2, p.transform.position.y, p.transform.position.z);
                        int distanceX = (int)(to.x - from.x);
                        int distanceY = (int)(to.y - from.y);                        
						if (distanceX == 0 || Mathf.Abs(distanceX) > 13 || distanceY >= PossibleJumpMaps.yUpHeightIncludingZero 
						    || distanceY < PossibleJumpMaps.yDownHeight) continue;

						bool[,] pathingMap;
						SplitDirection splitDirection;
						if(from.x > to.x){
							splitDirection = (from.y > to.y)? SplitDirection.TopLeft : SplitDirection.TopLeft;
						}else{
							splitDirection = (from.y > to.y)? SplitDirection.TopRight : SplitDirection.TopRight;
						}
						pathingMap = this.map.splitTo(splitDirection, from, new Dimension(13, distanceY + 2));


                        //Debug.Log(distanceX + "," + distanceY);
                        List<JumpRunCreationData> possibleJumps = PossibleJumpMaps.getPossible(distanceX, distanceY);
                        if (possibleJumps == null) continue;
                        foreach (var jump in possibleJumps) {
							if(from.x > to.x){
								if (!jump.jumpingPath.collideWithFromRightSide(pathingMap)) {
									fromPlateform.linkedJumpPlateform.Add(new LinkedJumpPlateform(from, p, jump));
								}
							}else{
								if (!jump.jumpingPath.collideWith(pathingMap)) {
									fromPlateform.linkedJumpPlateform.Add(new LinkedJumpPlateform(from, p, jump));
								}
							}

                        }
                    }
                }
			}
		}
	}

	private bool canReach(Plateform from, Plateform to){
		if(isInMaximumJumpPosibility(from, to)){
			return true;
		}
		return false;
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
		return Math.Abs (v1.x - v2.x) <= 13 && Math.Abs (v1.y - v2.y) <= 4;
	}

}
