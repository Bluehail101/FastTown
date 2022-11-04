using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Axe : MonoBehaviour
{
    public PlaceTiles tileScript;
    public Tilemap currentTilemap;
    public Tile yay;
    void Start()
    {
        tileScript = gameObject.GetComponent<PlaceTiles>();
    }
    public void removeTree(Vector3Int tile)
    {
        tileScript.treeGrid[tile.x,tile.y].isTree = false;
        if (tileScript.grid1[tile.x,tile.y].isWater == true) { currentTilemap = tileScript.trees; }
        else { currentTilemap = tileScript.upperTrees; }
        currentTilemap.SetTile(new Vector3Int(tile.x,tile.y,1), null);
        //currentTilemap.SetTile()
    }
}
