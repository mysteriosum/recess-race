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
    public static bool makeBackground = true;
	public static bool verbose;
    public static bool inDebugMode = false;
	public static bool loadGameElement = true;

    private static MapLoader instance = new MapLoader();
    private MapLoader(){
    }

    public static void loadFromFile(string file){
        string text = System.IO.File.ReadAllText(file);
        instance.load(text);
    }
	
	private List<TileData> tilesData;

	private Map map;
    private CameraFollow recessCamera;
	private GameObject worldRootGameObject;
	private GameObject aiGroupGameObject;

	public PlateformGenerator plateformGenerator;
    private TileCreator tileCreator;

    private XDocument document;


    private void load(string mapText) {
        stopwatch = new Stopwatch();
        loadAssets();
		emptyScene ();
        createEmptyWorld();
		addSystems ();

        document = XDocument.Parse(mapText);
        XElement mapElement = document.Elements().First();
        XElement tilesLayer = document.Elements().Descendants().First(e => e.Name == "layer");
        XElement waypoints = getObjectGroupElement("Waypoints");
        if (waypoints == null) {
            UnityEngine.Debug.Log("Missing important tag : Waypoints");
            return;
        }
        XElement AIPlateformRemoves = getObjectGroupElement("AIPlateformRemove");

        print("Set-up");

        loadTileset(mapElement.Descendants().Where(e => e.Name == "tileset"));
        print("Loaded tileSets");
        tileCreator = new TileCreator(worldRootGameObject.transform, tilesData);
        loadMapSettings(mapElement);
        plateformGenerator = new PlateformGenerator(this.map);
        plateformGenerator.setGameObjectParent(aiGroupGameObject.transform);
        print("Loaded MapSettings");

        loadBorders();
        print("Loaded borders");
        loadTiles(tilesLayer);
        print("Loaded tiles");

        plateformGenerator.doneLoadingTiles();
        tileCreator.doneLoadingTiles();
        print("Loaded tiles/CleanUp");

        plateformGenerator.loadAIPlateformRemove(AIPlateformRemoves, this.map);
        plateformGenerator.loadWaypoints(waypoints, this.map);
        print("Loaded Waypoints");

        plateformGenerator.linkPlateforms();
        print("Loaded Ai instructions");

		loadPositions ();
		print("Loaded Positions");

        loadGarbage();
        loadTennisBalls();
        loadItemPickup();
        loadSpeedBoosts();
		loadFillerTiles();
        print("Loaded Objects");

        if (makeBackground) {
            BackgroundLoader bgl = new BackgroundLoader();
            bgl.loadBackground(worldRootGameObject,map,recessCamera, map.backgroundYOffset);
            print("Loaded Background");
        }

        if (!MapLoader.inDebugMode) {
            plateformGenerator.hideAllPlatefoms();
        }
        print("Done");
    }

	private void emptyScene(){
		GameObject world = GameObject.Find ("World");
		if (world != null) {
			GameObject.DestroyImmediate(world);	
		}
		GameObject system = GameObject.Find ("Systems");
		if (system != null) {
			GameObject.DestroyImmediate(system);
		}
	}

    private void loadBorders() {
        Transform parent = GameObjectFactory.createGameObject("Borders", worldRootGameObject.transform).transform;
		Border border = parent.gameObject.AddComponent<Border>();
        float right = -1;
		float top = map.mapDimension.height + 1;
		createBorder("top", new Vector3(map.mapDimension.width / 2 + 1, top, 0), this.map.mapDimension.width+1, 1, parent);
        createBorder("bottom", new Vector3(map.mapDimension.width / 2+1, -1, 0), this.map.mapDimension.width+1, 1, parent);
        createBorder("left", new Vector3(map.mapDimension.width + 1, map.mapDimension.height / 2, 0), 1, this.map.mapDimension.height+1, parent);
        createBorder("right", new Vector3(right, map.mapDimension.height / 2, 0), 1, this.map.mapDimension.height+1, parent);
		if (loadGameElement) {
			border.border = new Rect (2, 0, map.mapDimension.width-4, map.mapDimension.height-1);		
		}
		
		
    }

    private void createBorder(string name, Vector3 position,int width, int height, Transform parent) {
        GameObject newBorder = GameObjectFactory.createGameObject("Top border", parent);
        newBorder.AddComponent<BoxCollider2D>();
        newBorder.transform.localScale = new Vector3(width, height);
        newBorder.transform.Translate(position);
        newBorder.AddComponent<GizmoDad>();
		newBorder.layer = LayerMask.NameToLayer("normalCollisions");
        GizmoDad dad = (GizmoDad)newBorder.GetComponent<GizmoDad>();
        dad.size = new Vector3(1, 1, 1);
        dad.myColour = Couleur.white;
        dad.myShape = GizmoDad.Forme.wireCube;
    }

    private void loadAssets() {
    }

	private void loadPositions(){
		Transform parent = GameObjectFactory.createGameObject("PositionsStuff", worldRootGameObject.transform).transform;
		IEnumerable<XElement> positions = getAllObjectFromObjectGroup("Positions");
		loadObject (positions, "End", "FinishLine","Finish Line", 2.8f, parent);
		if (loadGameElement) {
			loadObject (positions, "Player", "Fitzwilliam","Fitzwilliam", -0.5f, parent);
			loadObject (positions, "Billy", "Billy","Billy", -0.5f, parent);
			loadObject (positions, "StartObjects", "StartObjects","StartObjects", -0.5f, parent);
			loadObject (positions, "Liddy", "Liddy","Liddy", -0.5f, parent);
			loadObject (positions, "George", "George","George", -0.5f, parent);
			
			//Stuff to do with loading monitors; they're not in though at the moment so we'll keep this out
//			loadObject (positions, "Dialogue_1", "Dialogue_1","Dialogue_1", -0.4f, parent);
//			loadObject (positions, "Dialogue_1_2", "Dialogue_1_2","Dialogue_1", -0.4f, parent);
//			loadObject (positions, "Dialogue_2", "Dialogue_2","Dialogue_2", -0.4f, parent);
//			loadObject (positions, "Dialogue_3", "Dialogue_3","Dialogue_3", -0.4f, parent);
//			loadObject (positions, "Dialogue_4", "Dialogue_4","Dialogue_4", -0.4f, parent);
//			loadObject (positions, "Jane", "Jane","Jane", -0.4f, parent);
//			loadObject (positions, "Mr Bennet", "Mr Bennet","Mr Bennet", -0.4f, parent);
//			loadObject (positions, "Gardiner", "Gardiner","Gardiner", -0.4f, psarent);
//			loadObject (positions, "Hill", "Hill","Hill", -0.4f, parent);
		}				
	}


	private void loadObject(IEnumerable<XElement> xElement, string objectToTreatName, string prefabName, string unityObjectName, float yOffset, Transform parent){
		XElement element = null;
		try{
			element = xElement.First(e => e.Attribute("name").Value.Equals(objectToTreatName));
		}catch(InvalidOperationException){
			UnityEngine.Debug.LogError("MapLoader : Missing Element " + objectToTreatName);
			return;
		}

		GameObject prefab = Resources.Load<GameObject>(prefabName);
		GameObject obj = GameObjectFactory.createCopyGameObject(prefab, unityObjectName, parent);

		float x = (float) parse(element.Attribute("x").Value) / (float)map.tileDimension.width;
		float y = (float) map.mapDimension.height - parse(element.Attribute("y").Value) / (float)map.tileDimension.height;
		y = Mathf.Floor (y);
		obj.transform.Translate(x,y + yOffset,0);
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
	
	private void loadItemPickup() {
        GameObject itemPickupPrefab = Resources.Load<GameObject>("Objects/ItemPickup");
        IEnumerable<XElement> itemPickups = getAllObjectFromObjectGroup("ItemPickups");
		GameObject itemPickupParent = GameObjectFactory.createGameObject("Item Pickup Group", worldRootGameObject.transform);
		foreach (var element in itemPickups) {
            float x = (float)parse(element.Attribute("x").Value) / (float)map.tileDimension.width;
            float y = (float)map.mapDimension.height - parse(element.Attribute("y").Value) / (float)map.tileDimension.height;
			GameObject itemPickup = GameObjectFactory.createCopyGameObject(itemPickupPrefab, "ItemPickup", itemPickupParent);
			itemPickup.transform.Translate(x, y, 0);
		}
	}
	
	private void loadSpeedBoosts() {
        GameObject speedBoostPrefab = Resources.Load<GameObject>("Objects/SpeedBoost");
        IEnumerable<XElement> speedBoosts = getAllObjectFromObjectGroup("SpeedBoosts");
		GameObject speedBoostParent = GameObjectFactory.createGameObject("Speed Boost Group", worldRootGameObject.transform);
		foreach (var element in speedBoosts) {
            float x = (float)parse(element.Attribute("x").Value) / (float)map.tileDimension.width;
            float y = (float)map.mapDimension.height - parse(element.Attribute("y").Value) / (float)map.tileDimension.height;
			GameObject speedBoost = GameObjectFactory.createCopyGameObject(speedBoostPrefab, "SpeedBoost", speedBoostParent);
			speedBoost.transform.Translate(x, y, 0);
		}
	}	
	private void loadFillerTiles() {
        GameObject fillerPrefab = Resources.Load<GameObject>("Objects/FillerGrey");
        IEnumerable<XElement> fillers = getAllObjectFromObjectGroup("Fill");
		GameObject fillerParent = GameObjectFactory.createGameObject("Fill Group", worldRootGameObject.transform);
		foreach (var element in fillers) {
            float x = Mathf.Floor((float)parse(element.Attribute("x").Value) / (float)map.tileDimension.width);
            float y = Mathf.Ceil((float)map.mapDimension.height - parse(element.Attribute("y").Value) / (float)map.tileDimension.height);
			GameObject filler = GameObjectFactory.createCopyGameObject(fillerPrefab, "FillGuy", fillerParent);
			float scaleX = Mathf.Ceil((float)parse(element.Attribute("width").Value) / (float)map.tileDimension.width);
			float scaleY = Mathf.Ceil((float)parse(element.Attribute("height").Value) / (float)map.tileDimension.height);
			
			float offset = filler.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
			
			//filler.transform.Translate(x, y, 0);
			filler.transform.localScale = new Vector3(scaleX, scaleY, 1f);
			filler.transform.Translate(x + scaleX / 2 - offset, y - scaleY / 2 - offset, 0);
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
		if (loadGameElement) {
			this.recessCamera = GameObjectFactory.createCopyGameObject(Resources.Load<GameObject>("RecessCamera"), "RecessCamera", worldRootGameObject).GetComponent<CameraFollow>();		
		}

		aiGroupGameObject = GameObjectFactory.createGameObject ("Ai Group", worldRootGameObject.transform);
	}

	private void addSystems(){
		GameObject systems = GameObjectFactory.createGameObject ("Systems", null);
		systems.AddComponent<ScreenEffectSystem> ();
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
					XElement noCollision = tileData.Descendants().First(e => e.Name == "property" && e.Attribute("name").Value == "noCollision");
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
		var properties = mapElement.Descendants ().First (e => e.Name == "properties");
		UnityEngine.Debug.Log (properties.Descendants().Count());
		map.backgroundYOffset = parse( properties.Descendants ().First (e => e.Name == "property" && e.Attribute ("name").Value == "background-yOffset").Attribute("value").Value );
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

	private XElement getObjectGroupElement(string name){
		try{
			return document.Elements().Descendants().First(e => e.Name == "objectgroup" && e.Attribute("name").Value == name);
		}catch(InvalidOperationException){
			UnityEngine.Debug.LogError("The objectGroupe " + name + " is missing.");
		}
		return null;
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
