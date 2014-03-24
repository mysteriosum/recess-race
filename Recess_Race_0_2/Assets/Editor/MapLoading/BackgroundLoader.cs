using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public  class BackgroundLoader {


    public static Sprite[] silouettes;
    public static Sprite[] fence; 
    public static Sprite[] backgroundElements;

    private Transform parent;
	
    private float bgDistance = 60;
    private float silhouetteDistance = 100;
    private float silhouetteYOffset = 1;
    private float fenceYOffset = -2;
    private float fenceDistance = 27;

    private RecessCamera camera;
    private Map map;

    public void loadBackground(GameObject gameObjectMapParent, Map map, RecessCamera camera) {
        this.camera = camera;
        this.map = map;
        parent = GameObjectFactory.createGameObject("Backgrounds", gameObjectMapParent.transform).transform;
        camera.parallaxes = new List<Transform>();

		addLayerOfSprites("House"		, parent, "Background/BuildingAndStuff/", bgDistance, 0);
		addLayerOfSprites("Silouette"	, parent, "Background/Silouette/", silhouetteDistance, silhouetteYOffset);
		addLayerOfSprites("Fence"		, parent, "Background/fences/", fenceDistance, fenceYOffset);
        makeParallax();
    }

    private void addLayerOfSprites(string name, Transform parent, string assetPath, float z, float yOffset) {
		Sprite[] sprites = Resources.LoadAll<Sprite>(assetPath);
        Transform spritesParent = GameObjectFactory.createGameObject(name + "s", parent.transform).transform;
        int widthCovered = 0;
        int index = 0;
        bool isFirst = true;
        int totalToCover = map.mapDimension.width;

        while (widthCovered < totalToCover) {
            GameObject newGuy = GameObjectFactory.createGameObject(name + " " + index, spritesParent);
            SpriteRenderer newSprite = newGuy.AddComponent<SpriteRenderer>();
            int rando = UnityEngine.Random.Range(0, sprites.Length);
            newSprite.sprite = sprites[rando];

            int spriteWidth = (int)newSprite.sprite.bounds.size.x;

            newGuy.transform.position = new Vector3(widthCovered, yOffset, z);
            newSprite.sortingLayerName = "Background";
            camera.parallaxes.Add(newGuy.transform);

            widthCovered += spriteWidth;
            index++;
            if (isFirst) {
                totalToCover += spriteWidth;
                isFirst = false;
            }
        }
    }

    private void makeParallax() {
        foreach (Transform tr in camera.parallaxes) {
            if (tr.position.z > camera.furthestParalaxZ) {
                camera.furthestParalaxZ = tr.position.z;
            }
        }
    }
}
