using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapLoaderEditor : EditorWindow {

	public string fileName = "";
	public bool useTestBackground = false;

	void OnGUI(){
		GUILayout.BeginHorizontal ();
		fileName = GUILayout.TextField (fileName);
		if (GUILayout.Button ("Find Map File")) {
			fileName = EditorUtility.OpenFilePanel("Open Map file","../maps","tmx");
		}
		GUILayout.EndHorizontal ();

		this.useTestBackground = GUILayout.Toggle (this.useTestBackground, "Use test Background");
		if (fileName.Length == 0) {
			GUI.enabled = false;
			GUILayout.Button("Load Map");
			GUI.enabled = true;
		}else{
			if(GUILayout.Button("Load Map")) {
				MapLoader.useTestBackground = this.useTestBackground;
                MapLoader.loadFromFile(fileName);
			}
		}
	}
	
	
	[MenuItem ("FruitsUtils/MapLoader")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(MapLoaderEditor), true, "MapLoader");
	}
}
