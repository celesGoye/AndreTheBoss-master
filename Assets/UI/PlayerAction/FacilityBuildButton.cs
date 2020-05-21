using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityBuildButton : MonoBehaviour
{
    private GameManager gameManager;
    private Button buildFacilityButton;


    public void BuildFacility(BuildingType type,ItemType itemTypeProduced,int initLevel)
    {
        HexCell cell = gameManager.hexMap.selectedCell;
        if (cell == null)
            return;
        else
        {
			if(gameManager.gameInteraction.facilityPalletePanel.currentType==BuildingType.Teleporter)
			{
				Teleporter origin=gameManager.buildingManager.CreateBuilding(type,itemTypeProduced, gameManager.gameInteraction.facilityPalletePanel.currentDestination, initLevel).GetComponent<Teleporter>();
				Teleporter destination=gameManager.buildingManager.CreateBuilding(type,itemTypeProduced, cell, initLevel).GetComponent<Teleporter>();
				origin.another=destination;
				destination.another=origin;
				gameManager.gameInteraction.facilityPalletePanel.isSelecting=false;
			}
			else
				gameManager.buildingManager.CreateBuilding(type,itemTypeProduced, cell, initLevel);
        }
		Debug.Log("clearing");
        gameManager.gameInteraction.Clear();
    }
	public void OnBuildFacilityButton()
	{
		gameManager.gameInteraction.facilityPalletePanel.facilityBuildPanel.ConsumeItem();
		BuildFacility(gameManager.gameInteraction.facilityPalletePanel.currentType,
						gameManager.gameInteraction.facilityPalletePanel.currentItem,
						gameManager.gameInteraction.facilityPalletePanel.currentLevel);
	}
	

    public void OnEnable()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        buildFacilityButton = GetComponent<Button>();
        buildFacilityButton.onClick.AddListener(() => OnBuildFacilityButton());
    }

    public void OnDisable()
    {
        buildFacilityButton.onClick.RemoveAllListeners();
    }
}
