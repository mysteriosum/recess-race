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
	private GameObject tilePrefab;


    private void load(string mapText)
    {	
		loadAssets ();
		createEmptyWorld ();

		lines = mapText.Split(new string[] { "\n\r", "\r\n", "\n", "\r" }, StringSplitOptions.None);
        if (lines[0].StartsWith("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")) {
            readLines();
        } else {
            Debug.LogError("Invalide File");
        }
	}

	private void loadAssets(){
		sprites = Resources.LoadAll<Sprite> ("tileSets/testTileSet");
		tilePrefab = Resources.Load<GameObject> ("BasicTile");
	}
	private void createEmptyWorld(){
		worldRootGameObject = new GameObject();
		worldRootGameObject.name = "World";
	}

    private void readLines() {
		int fileLayerLineIndex = getNextLayerLineIndex(0);
		if (fileLayerLineIndex!= -1) {
            Vector2 dimension = getLayerDimension(lines[fileLayerLineIndex]);
            fileLayerLineIndex += 2;
			Debug.Log(dimension);
			for (int y = (int)(dimension.y -1); y >=0; y--) {
                loadLayerLine(y, lines[fileLayerLineIndex]);
                fileLayerLineIndex++;
            }
        }
    }

    private void loadLayerLine(int layerLineIndex, string tileLine)
    {
		string[] tiles = tileLine.Split(new char[] { ',' }, StringSplitOptions.None);
		int colIndex = 0;
		foreach (string tileId in tiles) {

			if(tileId.Equals("0") || tileId.Equals("") || tileId == null){

			}else{
				GameObject newTile = (GameObject)GameObject.Instantiate (this.tilePrefab);
				newTile.transform.parent = this.worldRootGameObject.transform;
				newTile.transform.Translate (colIndex,layerLineIndex,0);
			}
			colIndex++;
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
