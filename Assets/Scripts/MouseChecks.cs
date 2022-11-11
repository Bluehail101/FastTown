using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseChecks : MonoBehaviour
{
    public Tilemap map;
    public Tilemap buildingMap;
    public Tile cursorTile;
    public float panBorder;
    private PlaceTiles tileScript;
    private Vector3Int highlightedTile;
    private Vector3Int currentTile;
    private Building currentBuilding;
    private Resource resourceScript;
    private Axe axeScript;
    void Start()
    {
        tileScript = gameObject.GetComponent<PlaceTiles>();
        resourceScript = gameObject.GetComponent<Resource>();

        axeScript = gameObject.GetComponent<Axe>();
    }
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        currentBuilding = AccessBuildings.selectedBuilding;
        currentTile = map.WorldToCell(Camera.main.ScreenToWorldPoint(mouse));
        //Finds out which tile the mouse is currently hovering over.
        if (currentTile != highlightedTile)
        {
            map.SetTile(highlightedTile, null);
            highlightedTile = currentTile;
        }
        //If the mouse is hovering over a different tile than what is stored in currentTile, move the cursor to the new tile.
        if(currentBuilding != null)
        {
            if (tileScript.checkValid(currentTile, currentBuilding) == false || checkAffordable(currentBuilding) == false)
            {
                map.SetTile(currentTile, currentBuilding.redTile);
            }
            else
            {
                map.SetTile(currentTile, currentBuilding.mainTile);
            }
            return;
        }
        //If the player has selected a building, this checks if that current tile is a valid location for that building.
        //If it is a valid location, then building is shown on the tile as normal, if not the red variant is shown.
        map.SetTile(currentTile, cursorTile);
        //Only runs when the player hasn't selected a building, shows the cursor instead.
    }

    public void OnMouseDown()
    {
        if(currentBuilding == null) { return; }
        if(tileScript.checkValid(currentTile, currentBuilding) == false) { return; }
        if(Input.mousePosition.y <  panBorder) { return; }
        if(checkAffordable(currentBuilding) == false) { return; }
        if(currentBuilding.isSpecial == true)
        {
            if (currentBuilding.name == "0Axe")
            {
                axeScript.removeTree(currentTile);
            }
        }
        if(currentBuilding.isBuilding == false) { return; }
        //If no building is selected, the building is not in a valid location, player cannot afford the building,
        //or the mouse is within range of the pan border, then do not run the code below.
        if (currentBuilding.isRuleTile)
        {
            buildingMap.SetTile(currentTile, currentBuilding.ruleTile);
        }
        else
        {
            buildingMap.SetTile(currentTile, currentBuilding.mainTile);
        }
        //Checks if the tile is a rule tile or normal tile, then places the building down.
        takeResources(currentBuilding);
        //Removes the relevant resources.

        tileScript.buildingGrid[currentTile.x, currentTile.y].isBuilding = true;
        //Stores the location of the building in the grid, so the game knows it's there.

        if(currentBuilding.isProducer == true)
        {
            for (int i = 0; i < resourceScript.rateList.Count; i++)
            {
                resourceScript.rateList[i] += currentBuilding.producingList[i];
            }
        }
        if(currentBuilding.isConsumer == true)
        {
            for (int i = 0; i < resourceScript.rateList.Count; i++)
            {
                resourceScript.rateList[i] -= currentBuilding.cosumingList[i];
            }
        }
        //If building produces or cosumes any material, it is added to the producing / cosuming list.
        if(currentBuilding.deselectOnBuild == true)
        {
            AccessBuildings.selectedBuilding = null;
        }
        //Will deselect the building if that building would do so.
    }

    public bool checkAffordable(Building building)
    {
        if (building.costList[0] > resourceScript.gold && building.costList[0] != 0)
        {
            return false;
        }
        if (building.costList[1] > resourceScript.food && building.costList[1] != 0)
        {
            return false;
        }
        if (building.costList[2] > resourceScript.wood && building.costList[2] != 0)
        {
            return false;
        }
        return true;
        //If any of the resources are lower than the cost of the building return false, if not return true.
    }

    public void takeResources(Building building)
    {
        resourceScript.gold -= building.costList[0];
        resourceScript.food -= building.costList[1];
        resourceScript.wood -= building.costList[2];
    }
}
