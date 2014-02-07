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



    private void load(string mapText)
    {
        GameObject.crea
        lines = mapText.Split(new string[] { "\n\r", "\n", "\r" }, StringSplitOptions.None);
        if (lines[0].StartsWith("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")) {
            readLines();
        } else {
            Debug.LogError("Invalide File");
        }
	}

    private void readLines() {
        int lineIndex = getNextLayerLineIndex(0);
        if (lineIndex != -1) {
            Vector2 dimension = getLayerDimension(lines[lineIndex]);
            lineIndex += 2;
            int layerLineIndex = 0;
            for (int fileLayerLineIndex = 0; fileLayerLineIndex < dimension.y; fileLayerLineIndex++) {
                loadLayerLine(layerLineIndex, lines[fileLayerLineIndex]);
                lineIndex++;
                layerLineIndex++;
            }

            Debug.Log(lineIndex);
        }
    }

    private void loadLayerLine(int layerLineIndex, string p)
    {
        throw new NotImplementedException();
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
