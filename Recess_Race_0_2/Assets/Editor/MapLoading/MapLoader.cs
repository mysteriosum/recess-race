using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Threading;
using System.Diagnostics;


public class MapLoader {

	public static bool useTestBackground = false;
	public static bool verbose;
    public static bool inDebugMode = false;

    private static MapLoader instance = new MapLoader();
    private MapLoader(){
    }

    public static void loadFromFile(string file)
    {
        string text = System.IO.File.ReadAllText(file);
        instance.load(text);
    }
	
	private List<TileData> tilesData;

	private Map map;
	private GameObject worldRootGameObject;
	private GameObject aiGroupGameObject;

	private PlateformGenerator plateformGenerator;
    private TileCreator tileCreator;

    private XDocument document;


    private void load(string mapText){
        stopwatch = new Stopwatch();
		loadAssets ();
		createEmptyWorld ();

		document = XDocument.Parse (mapText);
		XElement mapElement = document.Elements ().First();
		XElement tilesLayer = document.Elements ().Descendants().First (e => e.Name == "layer");
        XElement waypoints = document.Elements().Descendants().First(e => e.Name == "objectgroup" && e.Attribute("name").Value == "Waypoints");
		XElement AIPlateformRemoves = document.Elements().Descendants().First(e => e.Name == "objectgroup" && e.Attribute("name").Value == "AIPlateformRemove");

        print("Set-up");

        loadTileset(mapElement.Descendants().Where(e => e.Name == "tileset"));
        print("Loaded tileSets");
        tileCreator = new TileCreator(worldRootGameObject.transform, tilesData);
		loadMapSettings (mapElement);
		plateformGenerator = new PlateformGenerator (this.map);
        plateformGenerator.setGameObjectParent(aiGroupGameObject.transform);
        print("Loaded MapSettings");

        loadTiles(tilesLayer);
        print("Loaded tiles");

		plateformGenerator.loadAIPlateformRemove(AIPlateformRemoves, this.map);
        plateformGenerator.loadWaypoints(waypoints, this.map);
        print("Loaded Waypoints");
        
		plateformGenerator.linkPlateforms ();
        print("Loaded Ai instructions");


        loadGarbage();
        loadTennisBalls();
        loadQuestionMark();
		loadSpeedBoosts();
        print("Loaded Objects");

        if(!MapLoader.inDebugMode){
            plateformGenerator.hideAllPlatefoms();
        }
		print ("Done");
	}

    private void loadAssets() {
    }

    private void loadTennisBalls() {
        GameObject tennisBallPrefab = Resources.Load<GameObject>("Objects/TennisBall");
        IEnumerable<XElement> tennisBalls = getAllObjectFromObjectGroup("TennisBalls");
        GameObject tennisBallParent = GameObjectFactory.createGameObject("Tennis Ball Group", worldRootGameObject.transform);

        foreach (var element in tennisBalls) {
            float x = (float) parse(element.Attribute("x").Value) / (float)map.tileDimension.width;
            float y = (float) map.mapDimension.height - parse(element.Attribute("y").Value) / (float)map.tileDimension.height;
            GameObject tennisBall = GameObjectFactory.createCopyGameObject(tennisBallPrefab, "Tennis Ball", tennisBallParent);
            tennisBall.transform.Translate(x,y,0);
            string value;
            try {
                value = element.Elements().Descendants().First(e => e.Name == "property" && e.Attribute("name").Value.Equals("direction")).Attribute("value").Value;
            }catch(InvalidOperationException){
                UnityEngine.Debug.LogError("MapLoader : Tennis ball without direction attribute! (" + x + element.Attribute("x").Value + "," + element.Attribute("y").Value + ")");
                continue;
            }
            
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
        IEnumerable<XElement> garbages = getAllObjectFromObjectGroup("Garbages");
        GameObject GarbageParent = GameObjectFactory.createGameObject("Garbage Group", worldRootGameObject.transform);
        foreach (var element in garbages) {
            float x = (float) parse(element.Attribute("x").Value) / (float)map.tileDimension.width;
            float y = (float) map.mapDimension.height - parse(element.Attribute("y").Value) / (float)map.tileDimension.height;
            GameObject garbage = GameObjectFactory.createCopyGameObject(garbagePrefab, "Garbage", GarbageParent);
            garbage.transform.Translate(x, y, 0);
        }
	}
	
	private void loadQuestionMark() {
        GameObject questionMarkPrefab = Resources.Load<GameObject>("Objects/QuestionMark");
        IEnumerable<XElement> questionMarks = getAllObjectFromObjectGroup("QuestionMarks");
		GameObject questionMarkParent = GameObjectFactory.createGameObject("Question Mark Group", worldRootGameObject.transform);
		foreach (var element in questionMarks) {
            float x = (float)parse(element.Attribute("x").Value) / (float)map.tileDimension.width;
            float y = (float)map.mapDimension.height - parse(element.Attribute("y").Value) / (float)map.tileDimension.height;
			GameObject garbage = GameObjectFactory.createCopyGameObject(questionMarkPrefab, "QuestionMark", questionMarkParent);
			garbage.transform.Translate(x, y, 0);
		}
	}
	
	private void loadSpeedBoosts() {
        GameObject questionMarkPrefab = Resources.Load<GameObject>("Objects/SpeedBoost");
        IEnumerable<XElement> speedBoosts = getAllObjectFromObjectGroup("SpeedBoosts");
		GameObject speedBoostParent = GameObjectFactory.createGameObject("Speed Boost Group", worldRootGameObject.transform);
		foreach (var element in speedBoosts) {
            float x = (float)parse(element.Attribute("x").Value) / (float)map.tileDimension.width;
            float y = (float)map.mapDimension.height - parse(element.Attribute("y").Value) / (float)map.tileDimension.height;
			GameObject garbage = GameObjectFactory.createCopyGameObject(questionMarkPrefab, "SpeedBoost", speedBoostParent);
			garbage.transform.Translate(x, y, 0);
		}
	}
	
    private IEnumerable<XElement> getAllObjectFromObjectGroup(string name) {
        try {
            return document.Elements().Descendants().First(e => e.Name == "objectgroup" && e.Attribute("name").Value == name).Descendants().Where(e => e.Name == "object");
        } catch (InvalidOperationException) {
            UnityEngine.Debug.LogError("MapLoader : Missing objectgroup " + name);
        }
        return new List<XElement>();
    }

	private void createEmptyWorld(){
		worldRootGameObject = GameObjectFactory.createGameObject ("World", null);
		worldRootGameObject.AddComponent<Map> ();
		this.map = worldRootGameObject.GetComponent<Map> ();

		aiGroupGameObject = GameObjectFactory.createGameObject ("Ai Group", worldRootGameObject.transform);
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
				UnityEngine.Debug.LogError("Map containts a error in tilesets : tileset "+  name + " (" + source + ") does'nt existe.");
			}else{
				foreach(Sprite sprite in sprites){
					TileData data = new TileData(sprite);
					this.tilesData.Add (data);
				}
			}

            IEnumerable<XElement> tileList = element.Descendants().Where(e => e.Name == "tile");
            foreach (var tileData in tileList) {
                int id = parse(tileData.Attribute("id").Value);
                try {
                    XElement noCollision = element.Descendants().First(e => e.Name == "property" && e.Attribute("name").Value == "noCollision");
                    if (noCollision != null) {
                        this.tilesData[firstGridId + id - 1].hasCollision = false;
                       // Debug.LogError("Tile " + id + " in " + name + " have noCollision");
                    }
                } catch (InvalidOperationException) {
                   // Debug.LogError("Tile " + id + " in " + name + "doesnt have noCollision");
                }
                
                
            }
		}
	}


	public void loadMapSettings(XElement mapElement){
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
		
		plateformGenerator.doneLoadingTiles ();
        tileCreator.doneLoadingTiles();
	}

    private void loadLayerLine(int y, string tileLine){
		string[] tiles = tileLine.Split(new char[] { ',' }, StringSplitOptions.None);
		int x = 0;
		foreach (string tileId in tiles) {
			if(!tileId.Equals("0") && !tileId.Equals("") && tileId != null){
				int id = parse(tileId) - 1;
                plateformGenerator.addTile(x, y, id);
				tileCreator.addTile(id, x, y);
			}
			x++;
		}

    }

	private int parse(string intStr){
		try{
			int id = Int32.Parse(intStr);
			return id;
		}catch (OverflowException){
			UnityEngine.Debug.LogError(intStr + " overflow the memory :(");
		}
		return -1;
	}

	

    private Stopwatch stopwatch;
	private void print(string str){
        if (MapLoader.verbose) {
            stopwatch.Stop();
            UnityEngine.Debug.Log("Maploader : " + str + " (" + stopwatch.ElapsedMilliseconds + " ms)");
        }
        stopwatch.Reset();
        stopwatch.Start();
	}

	public class TileData{
		public Sprite sprite;
        public bool hasCollision;

		public TileData(Sprite sprite){
			this.sprite = sprite;
            hasCollision = true;
		}
	}
}
