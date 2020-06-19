using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDestroyPanel : MonoBehaviour
{
	public Text txtname;
	
	private GameManager gameManager;
	private Building building;
	
    public void UpdateBuildingDestroyPanel(HexCell hexCell)
	{
		building=hexCell.building;
		if(gameManager==null)
			gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		txtname.text="Destroy\n "+TextColor.SetTextColor(building.GetBuildingType().ToString(),TextColor.ItemColor)+"(lv."+building.GetCurrentLevel()+")"+"?";
	}
	
	public void OnDestroyYes()
	{
		gameManager.buildingManager.DestroyBuilding(building);
        gameManager.gameInteraction.Clear();
	}
	
	public void OnDestroyNo()
	{
        gameManager.gameInteraction.Clear();
	}
}
