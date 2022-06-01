using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class testScript : MonoBehaviour
{
    public Tilemap map;
    public RuleTile tile;
    private void Start()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                map.SetTile(new Vector3Int(x, y, 1), tile);
            }
        }
    }
}
