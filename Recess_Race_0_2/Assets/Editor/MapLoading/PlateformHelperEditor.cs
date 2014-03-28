using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlateformHelperEditor  : EditorWindow {
	
	public Plateform plateform;
	public int choosenX;
	public bool flipXMapPathing = false;
	public bool flipYMapPathing = false;
	public int mapPathingWidth = 13;
	public int mapPathingHeight = 6;

	public int jumpWidth;
	public int jumpHeight;

	public int jumpIndex = 0;

	void OnGUI(){
		this.plateform = (Plateform)EditorGUILayout.ObjectField("Plateform", this.plateform, typeof(Plateform), true);
		if (this.plateform != null) {
			int left = (int)plateform.getLeftCornerPosition().x;
			int right = (int)plateform.getRightCornerPosition().x;
			choosenX = EditorGUILayout.IntSlider("working on x :", choosenX, left, right);

			flipXMapPathing = EditorGUILayout.Toggle("Flip X map pathing", flipXMapPathing);
			flipYMapPathing = EditorGUILayout.Toggle("Flip Y map pathing", flipYMapPathing);
			mapPathingWidth = EditorGUILayout.IntSlider("map Pathing Width", mapPathingWidth, 1, 30);
			mapPathingHeight = EditorGUILayout.IntSlider("map Pathing Height", mapPathingHeight, 1, 30);

			SplitDirection splitDirection;
			if(flipXMapPathing){
				splitDirection = (flipYMapPathing) ? SplitDirection.TopLeft : SplitDirection.BottomLeft;
			}else{
				splitDirection = (flipYMapPathing) ? SplitDirection.TopRight : SplitDirection.BottomRight;
			}

			Map map = (Map)GameObject.FindObjectOfType<Map>();
			if (map) {
				bool[,] split = map.splitTo(splitDirection, new Vector3(choosenX, plateform.transform.position.y, 0), new Dimension(mapPathingWidth, mapPathingHeight));
				PathingMap pathingMap = new PathingMap(split);
				EditorGUILayout.TextArea(pathingMap.ToStringWithNumbers());
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("try jumps");
			jumpWidth = EditorGUILayout.IntSlider("jump width :", jumpWidth, -13, 13);
			jumpHeight = EditorGUILayout.IntSlider("jupmp height :", jumpHeight, -PossibleJumpMaps.yDownHeight, PossibleJumpMaps.yUpHeightIncludingZero);

			Vector3 from = new Vector3(choosenX, plateform.transform.position.y,0);
			Vector3 to = new Vector3(from.x + jumpWidth, from.y + jumpHeight,0);
			plateform.showPoint(from, to,Color.red, !flipXMapPathing, !flipYMapPathing);

			var jumps = PossibleJumpMaps.getPossible(jumpWidth, jumpHeight);
			int nb = (jumps == null) ? 0 : jumps.Count;
			EditorGUILayout.LabelField("Known jumps : " + nb);
			
			if(nb != 0){
				jumpIndex = EditorGUILayout.IntSlider("jump to check out",jumpIndex,0, nb-1);
				EditorGUILayout.TextArea(jumps[jumpIndex].jumpingPath.ToStringWithNumbers());
			}
			/*if(GUILayout.Button("Remake plateform jumps")){
				plateform.linkedJumpPlateform.Clear();
				PlateformGenerator.
			}*/
		}
	}

	[MenuItem ("FruitsUtils/PlateformHelper")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(PlateformHelperEditor), true, "PlateformHelper");
	}
}
