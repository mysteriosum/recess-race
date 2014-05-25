using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public  class BackgroundLoader {


    public static Sprite[] silouettes;
    public static Sprite[] fence; 
    public static Sprite[] backgroundElements;

    private Transform parent;
	
    private float silhouetteDistance = 100;
    private float bgDistance = 85;
	private float grassLineDistance = 80f;
	private float treesDistance = 77;
    private float fenceDistance = 75;
	
    private float silhouetteYOffset = 1;
    private float fenceYOffset = -4;
	private float grassLineYOffset = -30.5f;
	private float houseYOffset = 0;
	private float treesYOffset = 0;

    private CameraFollow camera;
    private Map map;

    public void loadBackground(GameObject gameObjectMapParent, Map map, CameraFollow camera, float yOffset) {
        this.camera = camera;
        this.map = map;
        parent = GameObjectFactory.createGameObject("Backgrounds", gameObjectMapParent.transform).transform;
		if (MapLoader.loadGameElement) {
			camera.parallaxes = new List<Transform>();		
		}
        
		addLayerOfSprites("Silouette"	, parent, "Background/Silouette/", silhouetteDistance, silhouetteYOffset + yOffset, Vector2.one);
		addLayerOfSprites("House"		, parent, "Background/BuildingAndStuff/", bgDistance, houseYOffset + yOffset, Vector2.one);
		addLayerOfSprites("GrassLine"	, parent, "Background/GrassLine/", grassLineDistance, grassLineYOffset + yOffset, Vector2.one);
		addLayerOfSprites("Fence"		, parent, "Background/Fences/", fenceDistance, fenceYOffset + yOffset, Vector2.one);
		addLayerOfSprites("Tree"		, parent, "Background/Trees/", treesDistance, treesYOffset + yOffset, new Vector2(0.4f,3f));
	
			if (MapLoader.loadGameElement) {
			GameObject prefab = Resources.Load<GameObject>("SkyBackdrop");
			GameObjectFactory.createCopyGameObject(prefab, "SkyBackdrop", parent);
			makeParallax ();
		}
    }

    private void addLayerOfSprites(string name, Transform parent, string assetPath, float z, float yOffset, Vector2 skipSpriteFactor) {
		Sprite[] sprites = Resources.LoadAll<Sprite>(assetPath);
        Transform spritesParent = GameObjectFactory.createGameObject(name + "s", parent.transform).transform;
        float widthCovered = 0;
        int index = 0;
        bool isFirst = true;
        int totalToCover = map.mapDimension.width;

        while (widthCovered < totalToCover) {
            GameObject newGuy = GameObjectFactory.createGameObject(name + " " + index, spritesParent);
            SpriteRenderer newSprite = newGuy.AddComponent<SpriteRenderer>();
            int rando = UnityEngine.Random.Range(0, sprites.Length);
            newSprite.sprite = sprites[rando];

            int spriteWidth = (int)newSprite.sprite.bounds.size.x;

            newGuy.transform.position = new Vector3(widthCovered, map.mapDimension.height/2 + yOffset, z);
            newSprite.sortingLayerName = "Background";
			if (MapLoader.loadGameElement) {
            	camera.parallaxes.Add(newGuy.transform);
			}

			float factor = UnityEngine.Random.Range(skipSpriteFactor.x, skipSpriteFactor.y);
            widthCovered += spriteWidth * factor;
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
