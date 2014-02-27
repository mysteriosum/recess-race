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
    }
}