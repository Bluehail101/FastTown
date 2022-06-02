using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseChecks : MonoBehaviour
{
    public Tilemap map;
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
        map.SetTile(currentTile, cursorTile);
    }
}
