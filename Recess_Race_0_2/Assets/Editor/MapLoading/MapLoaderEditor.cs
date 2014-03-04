using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapLoaderEditor : EditorWindow {

	public string fileName = "";
	public bool useTestBackground = false;
	public bool verbose = true;
    public Plateform plateform;
    public int choosenX;
	public bool flipXMapPathing = false;
	public bool flipYMapPathing = false;

	void OnGUI(){
		GUILayout.BeginHorizontal ();
		fileName = GUILayout.TextField (fileName);
		if (GUILayout.Button ("Find Map File")) {
			fileName = EditorUtility.OpenFilePanel("Open Map file","../maps","tmx");
		}
		GUILayout.EndHorizontal ();

		this.useTestBackground = GUILayout.Toggle (this.useTestBackground, "Use test Background");
		this.verbose = GUILayout.Toggle (this.verbose, "Verbose");
		if (fileName.Length == 0) {
			GUI.enabled = false;
			GUILayout.Button("Load Map");
			GUI.enabled = true;
		}else{
			if(GUILayout.Button("Load Map")) {
				MapLoader.useTestBackground = this.useTestBackground;
				MapLoader.verbose = this.verbose;
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
			flipXMapPathing = EditorGUILayout.Toggle("Flip map pathing", flipXMapPathing);
            if (map) {
				bool[,] split;
				SplitDirection splitDirection;
				if(flipXMapPathing){
					splitDirection = (flipYMapPathing)? SplitDirection.TopLeft : SplitDirection.TopLeft;
				}else{
					splitDirection = (flipYMapPathing)? SplitDirection.TopRight : SplitDirection.TopRight;
				}
				split = map.splitTo (splitDirection, new Vector3(choosenX, plateform.transform.position.y, 0), new Dimension(13,6));
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
