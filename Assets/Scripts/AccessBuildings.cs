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
        //Loads all the building scriptable object into a list.
        for (int i = 0; i < buttonList.Count; i++)
        {
            if(buildingList[i].accessible == false) { continue; }
            buttonList[i].image.overrideSprite = buildingList[i].mainTile.sprite;
        }
        //Sets each button to a building.
    }

    public void selectBuilding(int ID)
    {
        if(selectedBuilding == buildingList[ID])
        {
            selectedBuilding = null;
            return;
        }
        //If the building selected has already been selected, deselect it.
        selectedBuilding = buildingList[ID];
        //If building has not already been selected then select that building!
    }
}
