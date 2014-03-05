using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapLoaderEditor : EditorWindow {

	public string fileName = "";
    public Plateform plateform;
    public int choosenX;
	public bool flipXMapPathing = false;
	public bool flipYMapPathing = false;
    public int mapPathingHeight = 6;

	void OnGUI(){
		GUILayout.BeginHorizontal ();
		fileName = GUILayout.TextField (fileName);
		if (GUILayout.Button ("Find Map File")) {
			fileName = EditorUtility.OpenFilePanel("Open Map file","../maps","tmx");
		}
		GUILayout.EndHorizontal ();

        MapLoader.inDebugMode = GUILayout.Toggle(MapLoader.inDebugMode, "Is in debug mod (show Plateform Gizmos)");
        MapLoader.verbose = GUILayout.Toggle(MapLoader.verbose, "Verbose");
		if (fileName.Length == 0) {
			GUI.enabled = false;
			GUILayout.Button("Load Map");
			GUI.enabled = true;
		}else{
			if(GUILayout.Button("Load Map")) {
                MapLoader.loadFromFile(fileName);
			}
		}

        GUILayout.Space(30);
        this.plateform = (Plateform)EditorGUILayout.ObjectField("Plateform", this.plateform, typeof(Plateform), true);
        if (this.plateform != null) {
            int left = (int)plateform.getLeftCornerPosition().x;
            int right = (int)plateform.getRightCornerPosition().x;
            choosenX = EditorGUILayout.IntSlider("working on x :", choosenX, left, right);
            Map map = (Map)GameObject.FindObjectOfType<Map>();
            flipXMapPathing = EditorGUILayout.Toggle("Flip X map pathing", flipXMapPathing);
            flipYMapPathing = EditorGUILayout.Toggle("Flip Y map pathing", flipYMapPathing);
            mapPathingHeight = EditorGUILayout.IntSlider("map Pathing Height", mapPathingHeight, 1, 30);
            if (map) {
				bool[,] split;
				SplitDirection splitDirection;
				if(flipXMapPathing){
                    splitDirection = (flipYMapPathing) ? SplitDirection.TopLeft : SplitDirection.BottomLeft;
				}else{
                    splitDirection = (flipYMapPathing) ? SplitDirection.TopRight : SplitDirection.BottomRight;
				}
                split = map.splitTo(splitDirection, new Vector3(choosenX, plateform.transform.position.y, 0), new Dimension(13, mapPathingHeight));
                PathingMap pathingMap = new PathingMap(split);
                EditorGUILayout.TextArea(pathingMap.ToStringWithNumbers());
            }
            
        }
    }
	
	
	[MenuItem ("FruitsUtils/MapLoader")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(MapLoaderEditor), true, "MapLoader");
	}
}
