using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class AccessBuildings : MonoBehaviour
{
    private List<Building> buildingList = new List<Building>();
    public List<Button> buttonList = new List<Button>();
    public static Building selectedBuilding;
    void Start()
    {
        buildingList.AddRange(Resources.LoadAll<Building>("Buildings"));
        for (int i = 0; i < buttonList.Count; i++)
        {
            if(buildingList[i].accessible == false) { continue; }
            buttonList[i].image.overrideSprite = buildingList[i].mainTile.sprite;
        }
    }

    public void selectBuilding(int ID)
    {
        if(selectedBuilding == buildingList[ID])
        {
            selectedBuilding = null;
            return;
        }
        selectedBuilding = buildingList[ID];
    }
}
