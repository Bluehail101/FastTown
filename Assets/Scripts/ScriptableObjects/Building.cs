using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Building")]
public class Building : ScriptableObject
{
    public Tile mainTile;
    public Tile redTile;
    public RuleTile ruleTile;
    public bool isRuleTile;
    public string buildingName;
    public int num;
    public bool accessible;
    public bool deselectOnBuild;
    public bool isMine;
    public bool isProducer;
    public bool isConsumer;
    [Header("Gold, Food, Wood")]
    public List<float> producingList = new List<float>();
    public List<float> cosumingList = new List<float>();
}
