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
	private float treesDistance = 52;
    private float silhouetteYOffset = 1;
    private float fenceYOffset = -4;
    private float fenceDistance = 27;
	private float grassLineYOffset = -10;

    private RecessCamera camera;
    private Map map;

    public void loadBackground(GameObject gameObjectMapParent, Map map, RecessCamera camera) {
        this.camera = camera;
        this.map = map;
        parent = GameObjectFactory.createGameObject("Backgrounds", gameObjectMapParent.transform).transform;
		if (MapLoader.loadGameElement) {
			camera.parallaxes = new List<Transform>();		
		}
        

		GameObject prefab = Resources.Load<GameObject>("SkyBackdrop");
		/*GameObject obj = */GameObjectFactory.createCopyGameObject(prefab, "SkyBackdrop", parent);

		addLayerOfSprites("Silouette"	, parent, "Background/Silouette/", silhouetteDistance, silhouetteYOffset, Vector2.one);
		addLayerOfSprites("House"		, parent, "Background/BuildingAndStuff/", bgDistance, 0, Vector2.one);
		addLayerOfSprites("GrassLine"	, parent, "Background/GrassLine/", bgDistance, grassLineYOffset, Vector2.one);
		addLayerOfSprites("Fence"		, parent, "Background/Fences/", fenceDistance, fenceYOffset, Vector2.one);
		addLayerOfSprites("Tree"		, parent, "Background/Trees/", treesDistance, 0f, new Vector2(0.4f,3f));
		if (MapLoader.loadGameElement) {
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
