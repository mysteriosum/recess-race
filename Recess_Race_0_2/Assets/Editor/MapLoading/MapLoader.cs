using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Linq;
using System.IO;


public class MapLoader {

	public static bool useTestBackground = false;

    private static MapLoader instance = new MapLoader();
    private MapLoader(){
    }

    public static void loadFromFile(string file)
    {
        string text = System.IO.File.ReadAllText(file);
        instance.load(text);
    }
	
	private List<TileData> tilesData;
	private Sprite[] banana;

	private Map map;
	private GameObject worldRootGameObject;
	private GameObject tilesGameObject;
	private GameObject aiGroupGameObject;

    private GameObject tilePrefab;

	private BullyInstructionGenerator bullyInstructionGenerator;

    private XDocument document;


    private void load(string mapText){	
		loadAssets ();
		createEmptyWorld ();

		document = XDocument.Parse (mapText);
		XElement mapElement = document.Elements ().First();
		XElement tilesLayer = document.Elements ().Descendants().First (e => e.Name == "layer");
        XElement waypoints = document.Elements().Descendants().First(e => e.Name == "objectgroup" && e.Attribute("name").Value == "Waypoints");

		loadTileset(mapElement.Descendants().Where (e => e.Name == "tileset"));
		loadMapSettings (mapElement);
		bullyInstructionGenerator = new BullyInstructionGenerator (this.map);
		bullyInstructionGenerator.setGameObjectParent (aiGroupGameObject.transform);

		loadTiles (tilesLayer);
		bullyInstructionGenerator.loadWaypoints (waypoints, this.map);
		bullyInstructionGenerator.linkPlateforms ();

        loadTennisBalls();
        loadGarbage();
	}

    private void loadAssets() {
        banana = Resources.LoadAll<Sprite>("background/testBanana");
        tilePrefab = Resources.Load<GameObject>("BasicTile");
    }

    private void loadTennisBalls() {
        GameObject tennisBallPrefab = Resources.Load<GameObject>("Objects/TennisBall");
        IEnumerable<XElement> tennisBalls = document.Elements().Descendants().Where(e => e.Name == "object" &&  e.Attribute("type") != null && e.Attribute("type").Value.Equals("TennisBall"));

        GameObject tennisBallParent = GameObjectFactory.createGameObject("Tennis Ball Group", worldRootGameObject);
        foreach (var element in tennisBalls) {
            int x = parse(element.Attribute("x").Value) / map.tileDimension.width;
            int y = map.mapDimension.height - parse(element.Attribute("y").Value) / map.tileDimension.height;
            GameObject tennisBall = GameObjectFactory.createCopyGameObject(tennisBallPrefab, "Tennis Ball", tennisBallParent);
            tennisBall.transform.Translate(x,y,0);
            string value = element.Elements().Descendants().First(e => e.Name == "property" && e.Attribute("name").Value.Equals("direction")).Attribute("value").Value;
            DirectionsEnum direction = DirectionsEnum.up;
            if (value == "up") {
                direction = DirectionsEnum.up;
            } else if (value == "left") {
                direction = DirectionsEnum.left;
            } else if (value == "right") {
                direction = DirectionsEnum.right;
            } else if (value == "down") {
                direction = DirectionsEnum.down;
            }
            TennisBall ballScript = (TennisBall)tennisBall.GetComponent<TennisBall>();
            ballScript.direction = direction;
        }
    }

    private void loadGarbage() {
        GameObject garbagePrefab = Resources.Load<GameObject>("Objects/Garbage");
        IEnumerable<XElement> garbages = document.Elements().Descendants().Where(e => e.Name == "object" && e.Attribute("type") != null && e.Attribute("type").Value.Equals("Garbage"));

        GameObject GarbageParent = GameObjectFactory.createGameObject("Garbage Group", worldRootGameObject);
        foreach (var element in garbages) {
            int x = parse(element.Attribute("x").Value) / map.tileDimension.width;
            int y = map.mapDimension.height - parse(element.Attribute("y").Value) / map.tileDimension.height;
            GameObject garbage = GameObjectFactory.createCopyGameObject(garbagePrefab, "Garbage", GarbageParent);
            garbage.transform.Translate(x, y, 0);
        }
    }

	private void createEmptyWorld(){
		worldRootGameObject = GameObjectFactory.createGameObject ("World", null);
		worldRootGameObject.AddComponent<Map> ();
		this.map = worldRootGameObject.GetComponent<Map> ();

		tilesGameObject = GameObjectFactory.createGameObject ("Tiles", worldRootGameObject);
		aiGroupGameObject = GameObjectFactory.createGameObject ("Ai Group", worldRootGameObject);
	}

	private void loadTileset(IEnumerable<XElement> tileSetElements){
		this.tilesData = new List<TileData> ();
		foreach(XElement element in tileSetElements){
			string name = element.Attribute("name").Value;
			int firstGridId = Int32.Parse(element.Attribute("firstgid").Value);
			string source = element.Descendants().First (e => e.Name == "image").Attribute("source").Value;
			string tilesetName = Path.GetFileName(source).Split(new char[] { '.' })[0];
			Sprite[] sprites = Resources.LoadAll<Sprite> ("tileSets/" + tilesetName);
			if(sprites.Count() == 0){
				Debug.LogError("Map containts a error in tilesets : tileset "+  name + " (" + source + ") does'nt existe.");
			}else{
				foreach(Sprite sprite in sprites){
					TileData data = new TileData(sprite);
					this.tilesData.Add (data);
				}
			}
		}
	}


	public void loadMapSettings(XElement mapElement){
		// Debug.Log (mapElement.Name);
		int width = Int32.Parse(mapElement.Attribute ("width").Value);
		int height = Int32.Parse(mapElement.Attribute ("height").Value);
		int tileWidth = Int32.Parse(mapElement.Attribute ("tilewidth").Value);
		int tileHeight = Int32.Parse(mapElement.Attribute ("tileheight").Value);
		this.map.mapDimension = new Dimension (width,height);
		this.map.tileDimension = new Dimension (tileWidth,tileHeight);
        this.map.pathingMap = BoolArray.generateBoolArrayArray(width,height);
	}


	private void loadTiles(XElement layer){
		string tilesCSV = layer.Elements ().First ().Value;
		int height = Int32.Parse(layer.Attribute("height").Value);
		string[] tilesLines = tilesCSV.Split(new string[] { "\n\r", "\r\n", "\n", "\r" }, StringSplitOptions.None);
		int y = height;
		for (int i = 1; i <= height; i++) {
			y--;
			loadLayerLine(y, tilesLines[i]);
		}
		
		bullyInstructionGenerator.doneLoadingTiles ();
	}

    private void loadLayerLine(int y, string tileLine){
		string[] tiles = tileLine.Split(new char[] { ',' }, StringSplitOptions.None);
		int x = 0;
		foreach (string tileId in tiles) {

			if(tileId.Equals("0") || tileId.Equals("") || tileId == null){

			} else {
				int id = parse(tileId) - 1;
				addTile(id, x, y);
			}
			x++;
		}

    }

	private int parse(string intStr){
		try{
			int id = Int32.Parse(intStr);
			return id;
		}catch (OverflowException overflow){
			Debug.LogError(intStr + " overflow the memory :(");
		}
		return -1;
	}

	private void addTile(int id, int x, int y){
		GameObject newTile = GameObjectFactory.createCopyGameObject (this.tilePrefab, "Tile", this.tilesGameObject);

		SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = this.tilesData [id].sprite;
		newTile.transform.Translate (x, y, 0);

		if (MapLoader.useTestBackground) {
			int textureWidth =10;
			int textureHeight = 10;
			int total = 100;
			SpriteRenderer newTileSprite = newTile.transform.GetChild (0).GetComponent<SpriteRenderer>();
			newTileSprite.sprite = this.banana[(int)((x % textureWidth + textureWidth * (textureHeight- y % textureHeight))) % total];		
		}

		bullyInstructionGenerator.addTile(x,y,id);
	}


	private class TileData{
		public Sprite sprite;

		public TileData(Sprite sprite){
			this.sprite = sprite;
		}
	}
}
