using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseChecks : MonoBehaviour
{
    public Tilemap map;
    public Tilemap buildingMap;
    public Tile cursorTile;
    private Vector3Int highlightedTile;
    private Vector3Int currentTile;
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        currentTile = map.WorldToCell(Camera.main.ScreenToWorldPoint(mouse));
        if (currentTile != highlightedTile)
        {
            map.SetTile(highlightedTile, null);
            highlightedTile = currentTile;
        }
        if(AccessBuildings.selectedBuilding != null)
        {
            map.SetTile(currentTile, AccessBuildings.selectedBuilding.buildingTile);
            return;
        }
        map.SetTile(currentTile, cursorTile);
    }

    public void OnMouseDown()
    {
        if(AccessBuildings.selectedBuilding == null) { return; }
        buildingMap.SetTile(currentTile, AccessBuildings.selectedBuilding.buildingTile);
        Debug.Log("click");
    }
}
