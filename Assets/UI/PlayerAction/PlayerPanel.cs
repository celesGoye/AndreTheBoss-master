using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
	public BuildingManager buildingManager;
	public Text txtbuildmode;
	
	public void OnEnable()
	{
		txtbuildmode.text="buildmode ON";
	}
	
    public void OnOpenMenu()
	{
		
	}
	
	public void UpdateBuildMode()
	{
		buildingManager.UpdateBuildMode(!buildingManager.buildmode);
		Debug.Log("buildmode:---"+buildingManager.buildmode);
	}
	
	public void UpdatePlayerPanel()
	{
		txtbuildmode.text=buildingManager.buildmode?"buildmode OFF":"buildmode ON";
	}
	
	public void OnSkipTurn()
	{
		
	}

}
