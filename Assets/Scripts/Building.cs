using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    public Tile buildingTile;
    public Tile redTile;
    public string buildingName;
    public bool accessible;
    public bool deselectOnBuild;
    public bool isMine;
}
