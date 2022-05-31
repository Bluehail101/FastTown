using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseChecks : MonoBehaviour
{
    public Tilemap map;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Debug.Log(map.WorldToCell(Camera.main.ScreenToWorldPoint(mouse)));
    }
}
