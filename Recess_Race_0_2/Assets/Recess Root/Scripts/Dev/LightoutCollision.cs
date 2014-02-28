using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightoutCollision : MonoBehaviour {

    public bool reset;

    public int width;
    public int height;

    public void resize() {
        GameObject lightoutBoxPrefab = Resources.Load<GameObject>("LightoutBox");
        LightoutBox[] boxs = this.GetComponentsInChildren<LightoutBox>();
        foreach (var box in boxs) {
            GameObject.DestroyImmediate(box.gameObject);
        }
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject newBox = GameObjectFactory.createCopyGameObject(lightoutBoxPrefab,"LightBox",this.gameObject);
                newBox.transform.Translate(this.transform.position);
                newBox.transform.Translate(x,y,0);
                LightoutBox lightBox = (LightoutBox) newBox.GetComponent<LightoutBox>();
            }
        }

    }

    public void resetColors() {
        LightoutBox[] boxs = this.GetComponentsInChildren<LightoutBox>();
        foreach (var box in boxs) {
            box.resetColor();
        }
    }

}



