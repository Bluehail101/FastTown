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
    void Start()
    {
        tileScript = gameObject.GetComponent<PlaceTiles>();
        resourceScript = gameObject.GetComponent<Resource>();
    }
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        currentBuilding = AccessBuildings.selectedBuilding;
        currentTile = map.WorldToCell(Camera.main.ScreenToWorldPoint(mouse));
        if (currentTile != highlightedTile)
        {
            map.SetTile(highlightedTile, null);
            highlightedTile = currentTile;
        }
        if(currentBuilding != null)
        {
            if (tileScript.checkValid(currentTile, currentBuilding.isMine) == false)
            {
                map.SetTile(currentTile, currentBuilding.redTile);
            }
            else
            {
                map.SetTile(currentTile, currentBuilding.mainTile);
            }
            return;
        }
        map.SetTile(currentTile, cursorTile);
    }

    public void OnMouseDown()
    {
        if(currentBuilding == null) { return; }
        if(tileScript.checkValid(currentTile, currentBuilding.isMine) == false) { return; }
        if(Input.mousePosition.y <  panBorder) { return; }
        if (currentBuilding.isRuleTile)
        {
            buildingMap.SetTile(currentTile, currentBuilding.ruleTile);
        }
        else
        {
            buildingMap.SetTile(currentTile, currentBuilding.mainTile);
        }
        tileScript.buildingGrid[currentTile.x, currentTile.y].isBuilding = true;
        if(currentBuilding.isProducer == true)
        {
            for (int i = 0; i < resourceScript.rateList.Count; i++)
            {
                resourceScript.rateList[i] += currentBuilding.producingList[i];
            }
        }
        if(currentBuilding.deselectOnBuild == true)
        {
            AccessBuildings.selectedBuilding = null;
        }
    }
}
