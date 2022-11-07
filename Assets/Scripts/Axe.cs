using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Axe : MonoBehaviour
{
    public PlaceTiles tileScript;
    public Tilemap currentTilemap;
    void Start()
    {
        tileScript = gameObject.GetComponent<PlaceTiles>();
    }
    public void removeTree(Vector3Int tile)
    {
        tileScript.treeGrid[tile.x,tile.y].isTree = false;
        if (tileScript.grid1[tile.x,tile.y].isWater == true) { currentTilemap = tileScript.trees; }
        else { currentTilemap = tileScript.upperTrees; }
        
        tileScript.trees.SetTile(new Vector3Int(tile.x,tile.y,1), null);
        tileScript.upperTrees.SetTile(new Vector3Int(tile.x, tile.y, 1), null);
        currentTilemap.SetTile(new Vector3Int(tile.x, tile.y, 1), null);

        if (tileScript.treeGrid[tile.x,tile.y + 1].isTree == true)
        {
            currentTilemap.SetTile(new Vector3Int(tile.x, tile.y + 1, 1), tileScript.forestTiles[0]);
        }
        else
        {
            tileScript.trees.SetTile(new Vector3Int(tile.x, tile.y + 1, 1), null);
            tileScript.upperTrees.SetTile(new Vector3Int(tile.x, tile.y + 1, 1), null);
        }
        if (tileScript.treeGrid[tile.x, tile.y - 1].isTree == true)
        {
            currentTilemap.SetTile(new Vector3Int(tile.x, tile.y, 1), tileScript.forestTiles[1]);
        }
    }
}
