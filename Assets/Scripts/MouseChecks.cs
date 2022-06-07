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
    void Start()
    {
        tileScript = gameObject.GetComponent<PlaceTiles>();
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
            map.SetTile(currentTile, currentBuilding.buildingTile);
            return;
        }
        map.SetTile(currentTile, cursorTile);
    }

    public void OnMouseDown()
    {
        if(currentBuilding == null) { return; }
        if(tileScript.checkValid(currentTile) == false) { return; }
        if(Input.mousePosition.y <  panBorder) { return; }
        buildingMap.SetTile(currentTile, currentBuilding.buildingTile);
        if(currentBuilding.deselectOnBuild == true)
        {
            AccessBuildings.selectedBuilding = null;
        }
    }
}
