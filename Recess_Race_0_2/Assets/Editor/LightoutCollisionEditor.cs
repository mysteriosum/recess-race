using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
[CustomEditor(typeof(LightoutCollision))]
public class LightoutCollisionEditor : Editor {

    public override void OnInspectorGUI() {
        LightoutCollision collision = (LightoutCollision)target;

        GUI.changed = false;
        collision.width = EditorGUILayout.IntSlider("Width",collision.width, 1, 50);
        collision.height = EditorGUILayout.IntSlider("Height", collision.height, 1, 50);
        if (GUI.changed) {
            collision.resize();
        }
        if (GUILayout.Button("Reset colors")) {
            collision.resetColors();
        }


		EditorGUILayout.LabelField ("Collision Map");
        EditorGUILayout.TextArea(generateLightoutCollision(collision));
    }

    private string generateLightoutCollision(LightoutCollision collision) {
        string text = "{";
        LightoutBox[] boxs = collision.gameObject.GetComponentsInChildren<LightoutBox>();
        int x = 0;
        foreach(LightoutBox box in boxs){
            if (x == collision.width) {
                text += "},\n{";
                x = 0;
            }
            if (x == 0) {
                text += box.triggered ? "true" : "false";
            } else {
                text += box.triggered ? ",true" : ",false";
            }
            x++;
        }
        text += "}";

        return text;
    }
}