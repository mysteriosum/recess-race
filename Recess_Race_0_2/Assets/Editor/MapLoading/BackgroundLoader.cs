using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public  class BackgroundLoader {


    public static Sprite[] silouettes;
    public static Sprite[] fence; 
    public static Sprite[] backgroundElements;

    private Transform parent;

    private int fenceAmount = 85;
    private float bgDistance = 60;
    private float silhouetteDistance = 100;
    private float silhouetteYOffset = 1;
    private float fenceYOffset = -2;
    private float fenceDistance = 27;
    private int minimumAtStart = 70;
    private float minimumDistance = -10;

    private RecessCamera camera;
    private Map map;

    public void loadBackground(GameObject gameObjectMapParent, Map map, RecessCamera camera) {
        this.camera = camera;
        this.map = map;
        loadAssets();
        parent = GameObjectFactory.createGameObject("Backgrounds", gameObjectMapParent.transform).transform;
        camera.parallaxes = new List<Transform>();

        addLayerOfSprites("House", parent, backgroundElements, bgDistance, 0);
        addLayerOfSprites("Silouette", parent, silouettes, silhouetteDistance, silhouetteYOffset);
        //loadFences();
        makeParallax();
    }

    private void addLayerOfSprites(string name, Transform parent, Sprite[] sprites, float z, float yOffset) {
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

   /* private void loadFences() {
        Transform fencesParent = GameObjectFactory.createGameObject("Fences", parent.transform).transform;
        camera.parallaxes.Add(fencesParent);

        Vector3 fencePosition = new Vector3(minimumDistance, /*t.position.y +*//* fenceYOffset, fenceDistance);
        for (int i = 0; i < fenceAmount; i++) {
            GameObject newGuy = GameObjectFactory.createGameObject("Fence " + i.ToString(), fencesParent.transform);
            SpriteRenderer newSprite = newGuy.AddComponent<SpriteRenderer>();
            newSprite.sprite = fence;
            newGuy.transform.position = fencePosition + Vector3.right * newSprite.sprite.bounds.size.x;
            fencePosition = newGuy.transform.position;
            newSprite.sortingLayerName = "Background";
            camera.parallaxes.Add(newGuy.transform);
        }
    }*/

    private void makeParallax() {
        foreach (Transform tr in camera.parallaxes) {
            if (tr.position.z > camera.furthestParalaxZ) {
                camera.furthestParalaxZ = tr.position.z;
            }
        }
    }


    private static void loadAssets(){
        backgroundElements = Resources.LoadAll<Sprite>("Background/BuildingAndStuff/");
        silouettes = Resources.LoadAll<Sprite>("Background/Silouette/");
        //fence = Resources.LoadAll<Sprite>("Background/Silouette/");
    }
}
