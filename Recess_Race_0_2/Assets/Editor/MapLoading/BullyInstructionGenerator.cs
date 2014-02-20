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
	private Transform bullyInstructionParent;
	private Transform plateformGroupParent;

	private bool workingOnATile;
	private int tileMaxX;
	private int tileMinX;
	private int tileY;

	private bool[,] pathingMap;
    private Dimension mapDimension;

	public BullyInstructionGenerator(Dimension mapDimension){
        this.mapDimension = mapDimension;
        tileHoverPrefab = Resources.Load<GameObject>("TilePlateformHover");
        bullyInstructionPrefab = Resources.Load<GameObject>("BullyInstruction");
		plateforms = new List<Plateform> ();
		pathingMap = new bool[mapDimension.width,mapDimension.height];
	}

	public void setGameObjectParent(Transform parent){
		this.parent = parent;

		GameObject bullyInstructions = new GameObject();
		bullyInstructions.name = "Bully Instructions";
		bullyInstructions.transform.parent = parent;
		bullyInstructionParent = bullyInstructions.transform;

		GameObject plateformGameObject = new GameObject();
		plateformGameObject.name = "Plateforms";
		plateformGameObject.transform.parent = parent;
		plateformGroupParent = plateformGameObject.transform;
	}

	public void addTile(int x, int y, int id){
        this.pathingMap[x, y] = true;

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
        return y != mapDimension.height-1 && pathingMap[x, y + 1];
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
        Debug.Log("Generated " + this.plateforms.Count + " plateforms.");
		removeUselessPlateform ();
        //Debug.Log("Finale " + this.plateforms.Count + " plateforms.");
	}


    private void addTileHover()
    {
        if (!workingOnATile) return;
		GameObject newTileHover = (GameObject)GameObject.Instantiate (this.tileHoverPrefab);
		newTileHover.name = "Plateform";
		newTileHover.transform.parent = plateformGroupParent;
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
				plateform.waypointId = id;
				spriteRenderer.color = new Color(((float) id) / nbWayPoints,0,0, 0.6f);
				if(id == 1){
					BullyInstructionConfiguration con = new BullyInstructionConfiguration(LengthEnum.none, CommandEnum.right, DifficultyEnum.assured);
					BullyInstruction bi = MapElementHelper.generateInstructionOnCentered(con, plateform, this.bullyInstructionParent);
					bi.gameObject.transform.localScale = plateform.gameObject.transform.localScale;
					bi.gameObject.name = "Starting instruction";
				}
			}
		}
	}


	public void linkPlateforms(){
		foreach (var plateform in this.plateforms) {
            BullyInstructionConfiguration con = new BullyInstructionConfiguration(LengthEnum.hold, CommandEnum.right, DifficultyEnum.assured);
            MapElementHelper.generateInstructionOnRight(con, plateform, this.bullyInstructionParent);

		}
	}

	/*private bool[,] getSplited(){
		//bool[,] plathingPart = bool[7,];
	}*/
    

}
