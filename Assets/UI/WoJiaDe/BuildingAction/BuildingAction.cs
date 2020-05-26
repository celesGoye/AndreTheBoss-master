using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingAction : MonoBehaviour
{
	public BuildingUpgradePanel buildingUpgradePanel;
	public BuildingDestroyPanel buildingDestroyPanel;
	public AltarPanel altarPanel;
	
	public Button buttonUpgrade;
	
	private HexCell currentHexCell;
    private GameManager gameManager;
	
	public void OnEnable()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		
		buildingUpgradePanel.gameObject.SetActive(false);
		buildingDestroyPanel.gameObject.SetActive(false);		
	}
	
	public void UpdateBuildingActionPanel(HexCell hexCell)
    {
		currentHexCell=hexCell;
		this.GetComponent<followGameObject>().follow=hexCell.GetComponent<Transform>();
		
		buttonUpgrade.interactable=(int)hexCell.building.GetCurrentLevel()<Building.GetMaxLevel(hexCell.building.GetBuildingType())?true:false;
    }
	
	public void OnUpgrade()
	{
		buildingUpgradePanel.gameObject.SetActive(true);
		buildingUpgradePanel.UpdateBuildingUpgradePanel(currentHexCell);
	}
	
	public void OnDestroyBuilding()
	{
		buildingDestroyPanel.gameObject.SetActive(true);
		buildingDestroyPanel.UpdateBuildingDestroyPanel(currentHexCell);
	}
	
	public void OnUse()
	{
		altarPanel.gameObject.SetActive(true);
		altarPanel.UpdateAltarPanel(currentHexCell);
	}
	
	public void OnDisable()
	{
		buildingUpgradePanel.gameObject.SetActive(false);
		buildingDestroyPanel.gameObject.SetActive(false);
		altarPanel.gameObject.SetActive(false);		
	}
}
