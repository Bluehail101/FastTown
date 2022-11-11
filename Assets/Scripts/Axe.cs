using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Axe : MonoBehaviour
{
    public PlaceTiles tileScript;
    public Tilemap currentTilemap;
    public Resource resourceScript;
    void Start()
    {
        tileScript = gameObject.GetComponent<PlaceTiles>();
        resourceScript = gameObject.GetComponent<Resource>();
    }

    public void doMethod()
    {
        Debug.Log("Pog");
    }
    public void removeTree(Vector3Int tile)
    {
        tileScript.treeGrid[tile.x,tile.y].isTree = false;
        resourceScript.wood = resourceScript.wood + 10;
        //Set the tree grid tile to not be a tree anymore.
        if (tileScript.grid1[tile.x,tile.y].isWater == true) { currentTilemap = tileScript.trees; }
        else { currentTilemap = tileScript.upperTrees; }
        //Determines whether or not the tree is on the lower or upper level.
        
        tileScript.trees.SetTile(new Vector3Int(tile.x,tile.y,1), null);
        tileScript.upperTrees.SetTile(new Vector3Int(tile.x, tile.y, 1), null);
        //Currently removes tree tile from both layers due to an error that can occur here.

        if (tileScript.treeGrid[tile.x,tile.y + 1].isTree == true)
        {
            currentTilemap.SetTile(new Vector3Int(tile.x, tile.y + 1, 1), tileScript.forestTiles[0]);
            //If tile above the selected is also tree, switch it's tile to a bottom tree tile.
        }
        else
        {
            tileScript.trees.SetTile(new Vector3Int(tile.x, tile.y + 1, 1), null);
            tileScript.upperTrees.SetTile(new Vector3Int(tile.x, tile.y + 1, 1), null);
            //If not, remove the above tile too, to remove the top part of the tree.
        }
        if (tileScript.treeGrid[tile.x, tile.y - 1].isTree == true)
        {
            currentTilemap.SetTile(new Vector3Int(tile.x, tile.y, 1), tileScript.forestTiles[1]);
            //If the tile below is a tree, switch the current tile with a top tree tile.
        }
    }
}
