using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Linq;

public class TileCreator {

    private Material tilesMaterial;
    private Sprite[] banana;

    private Transform tilesParent;
    private Transform collidersParent;
    private List<MapLoader.TileData> tilesData;

    private bool workingOnATile;
    private int tileMaxX;
    private int tileMinX;
    private int tileY;

    public TileCreator(Transform parent, List<MapLoader.TileData> tilesData) {
        this.tilesParent = parent;
        this.tilesData = tilesData;
        this.tilesParent = GameObjectFactory.createGameObject("Tiles", parent).transform;
        this.collidersParent = GameObjectFactory.createGameObject("Colliders", parent).transform;
        collidersParent.gameObject.layer = LayerMask.NameToLayer("normalCollisions");
        loadAssets();
    }

    private void loadAssets() {
        tilesMaterial = Resources.Load<Material>("Material/TilesMaterial");
        banana = Resources.LoadAll<Sprite>("background/testBanana");
    }

    public void addTile(int id, int x, int y) {
        GameObject newTile = GameObjectFactory.createGameObject("Tile", tilesParent);
        newTile.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = this.tilesData[id].sprite;
        spriteRenderer.material = tilesMaterial;
        spriteRenderer.sortingLayerID = 1;

        newTile.transform.Translate(x, y, 0);

        if (MapLoader.useTestBackground) {
            int textureWidth = 10;
            int textureHeight = 10;
            int total = 100;
            SpriteRenderer newTileSprite = newTile.transform.GetChild(0).GetComponent<SpriteRenderer>();
            newTileSprite.sprite = this.banana[(int)((x % textureWidth + textureWidth * (textureHeight - y % textureHeight))) % total];
        }
        if (this.tilesData[id].hasCollision) {
            addCollider(x,y);
        }

    }

    private void addCollider(int x, int y) {
        if (!workingOnATile) {
            tileMinX = x;
            tileMaxX = x;
            tileY = y;
            workingOnATile = true;
        } else {
            if (tileMaxX + 1 == x && y == tileY) {
                tileMaxX = x;
            } else {
                pushCollider();
                tileMinX = x;
                tileMaxX = x;
                tileY = y;
            }
        }
    }

    private void pushCollider() {
        GameObject newCollider = GameObjectFactory.createGameObject("Collider", collidersParent);
        newCollider.layer = LayerMask.NameToLayer("normalCollisions");
        newCollider.AddComponent<BoxCollider2D>();
        BoxCollider2D box = newCollider.GetComponent<BoxCollider2D>();
        float width = tileMaxX - tileMinX;
        float center = width / 2;
        newCollider.transform.Translate(tileMinX + center, tileY, 0);
        newCollider.transform.localScale = new Vector3(width + 1, 1, 0);
    }

    internal void doneLoadingTiles() {
        pushCollider();
    }
}
