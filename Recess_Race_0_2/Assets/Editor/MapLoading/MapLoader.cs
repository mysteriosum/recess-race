using UnityEngine;
using System.Collections;
using System;

public class MapLoader {

    private static MapLoader instance = new MapLoader();
    private MapLoader(){
    }
    public static void loadFromFile(string file)
    {
        string text = System.IO.File.ReadAllText(file);
        instance.load(text);
    }


    private string[] lines;
	private Sprite[] sprites;
	private GameObject worldRootGameObject;
	private GameObject tilesGameObject;
	private GameObject bullyInstructionGameObject;

	private GameObject tilePrefab;

	private BullyInstructionGenerator bullyInstructionGenerator;


    private void load(string mapText)
	{	
		bullyInstructionGenerator = new BullyInstructionGenerator ();
		loadAssets ();
		createEmptyWorld ();

		lines = mapText.Split(new string[] { "\n\r", "\r\n", "\n", "\r" }, StringSplitOptions.None);
        if (lines[0].StartsWith("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")) {
            readLines();
        } else {
            Debug.LogError("Invalide File");
        }
		bullyInstructionGenerator.done ();
	}

	private void loadAssets(){
		sprites = Resources.LoadAll<Sprite> ("tileSets/testTileSet");
		tilePrefab = Resources.Load<GameObject> ("BasicTile");
	}

	private void createEmptyWorld(){
		worldRootGameObject = new GameObject();
		worldRootGameObject.name = "World";
		
		tilesGameObject = new GameObject();
		tilesGameObject.name = "Tiles";
		tilesGameObject.transform.parent = worldRootGameObject.transform;
		
		bullyInstructionGameObject = new GameObject();
		bullyInstructionGameObject.name = "Bully Instructions";
		bullyInstructionGameObject.transform.parent = worldRootGameObject.transform;
		bullyInstructionGenerator.setGameObjectParent (bullyInstructionGameObject.transform);
	}

    private void readLines() {
		int fileLayerLineIndex = getNextLayerLineIndex(0);
		if (fileLayerLineIndex!= -1) {
            Vector2 dimension = getLayerDimension(lines[fileLayerLineIndex]);
            fileLayerLineIndex += 2;
			for (int y = (int)(dimension.y -1); y >=0; y--) {
                loadLayerLine(y, lines[fileLayerLineIndex]);
                fileLayerLineIndex++;
            }
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

    private Vector2 getLayerDimension(string p)
    {
        int widthTextStart = p.IndexOf("width=");
        int widthTextEnd = widthTextStart + 7;
        int heightTextStart = p.IndexOf("height=");
        int heightTextEnd = heightTextStart + 8;
        int lineTextEnd = p.Length - 2;
        int width = Int32.Parse(p.Substring(widthTextEnd, heightTextStart - 2 - widthTextEnd));
        int height = Int32.Parse(p.Substring(heightTextEnd, lineTextEnd - heightTextEnd));
        return new Vector2(width, height);
    }

    private int getNextLayerLineIndex(int startingIndex)
    {
        int index = startingIndex;
        while (index < lines.Length) {
            if (lines[index].StartsWith(" <layer name=")){
                return index;
            } else {
                index++;
            }
            
        }
        return -1;
    }

}
