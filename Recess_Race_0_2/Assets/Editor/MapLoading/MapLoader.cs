using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Linq;

public class MapLoader {

    private static MapLoader instance = new MapLoader();
    private MapLoader(){
    }
    public static void loadFromFile(string file)
    {
        string text = System.IO.File.ReadAllText(file);
        instance.load(text);
    }

	private Sprite[] sprites;
	private GameObject worldRootGameObject;
	private Map map;
	private GameObject tilesGameObject;
	private GameObject bullyInstructionGameObject;

	private GameObject tilePrefab;

	private BullyInstructionGenerator bullyInstructionGenerator;


    private void load(string mapText)
	{	
		loadAssets ();
		createEmptyWorld ();

		XDocument document = XDocument.Parse (mapText);
		XElement mapElement = document.Elements ().First();
		XElement tilesLayer = document.Elements ().Descendants().First (e => e.Name == "layer");
		XElement waypoints = document.Elements().Descendants().First(e => e.Name == "objectgroup" && e.Attribute("name").Value == "Waypoints");

		loadMapSettings (mapElement);
		bullyInstructionGenerator = new BullyInstructionGenerator (this.map.mapDimension);
		bullyInstructionGenerator.setGameObjectParent (bullyInstructionGameObject.transform);

		loadTiles (tilesLayer);
		bullyInstructionGenerator.doneLoadingTiles ();
		bullyInstructionGenerator.loadWaypoints (waypoints, this.map);
		bullyInstructionGenerator.linkPlateforms ();
	}

	private void loadAssets(){
		sprites = Resources.LoadAll<Sprite> ("tileSets/testTileSet");
		tilePrefab = Resources.Load<GameObject> ("BasicTile");
	}

	private void createEmptyWorld(){
		worldRootGameObject = new GameObject();
		worldRootGameObject.name = "World";
		worldRootGameObject.AddComponent<Map> ();
		this.map = worldRootGameObject.GetComponent<Map> ();
		
		tilesGameObject = new GameObject();
		tilesGameObject.name = "Tiles";
		tilesGameObject.transform.parent = worldRootGameObject.transform;
		
		bullyInstructionGameObject = new GameObject();
		bullyInstructionGameObject.name = "Bully Instructions";
		bullyInstructionGameObject.transform.parent = worldRootGameObject.transform;
	}


	public void loadMapSettings(XElement mapElement){
//		Debug.Log (mapElement.Name);
		int width = Int32.Parse(mapElement.Attribute ("width").Value);
		int height = Int32.Parse(mapElement.Attribute ("height").Value);
		int tileWidth = Int32.Parse(mapElement.Attribute ("tilewidth").Value);
		int tileHeight = Int32.Parse(mapElement.Attribute ("tileheight").Value);
		this.map.mapDimension = new Dimension (width,height);
		this.map.tileDimension = new Dimension (tileWidth,tileHeight);
	}


	private void loadTiles(XElement layer){
		string tilesCSV = layer.Elements ().First ().Value;
		int height = Int32.Parse(layer.Attribute("height").Value);
		string[] tilesLines = tilesCSV.Split(new string[] { "\n\r", "\r\n", "\n", "\r" }, StringSplitOptions.None);

		int y = height;
		for (int i = 0; i < height; i++) {
			y--;
			loadLayerLine(y, tilesLines[i]);
		}
	}

    private void loadLayerLine(int y, string tileLine)
    {
		string[] tiles = tileLine.Split(new char[] { ',' }, StringSplitOptions.None);
		int x = 0;
		foreach (string tileId in tiles) {

			if(tileId.Equals("0") || tileId.Equals("") || tileId == null){

			} else {
				int id = Int32.Parse(tileId) - 1;
				GameObject newTile = (GameObject)GameObject.Instantiate (this.tilePrefab);
				SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
				spriteRenderer.sprite = this.sprites[id];
				newTile.transform.parent = this.tilesGameObject.transform;
				newTile.transform.Translate (x, y, 0);
				bullyInstructionGenerator.addTile(x,y,id);
			}
			x++;
		}

    }
}
