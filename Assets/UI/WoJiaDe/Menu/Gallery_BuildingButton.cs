using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_BuildingButton : MonoBehaviour
{
    public BuildingType type;
	public Gallery_BuildingPage buildingPage;
	public int id;
	void Start()
	{
		this.GetComponent<Button>().onClick.AddListener(OnBuildingBtn);
		id=this.transform.GetSiblingIndex();
	}
	
	public void OnBuildingBtn()
	{
		buildingPage.theBuildingPage.buildingType=type;
		buildingPage.currentid=id;
		buildingPage.OnBuildingBtn();
		
		buildingPage.theBuildingPage.UpdateBuilding();
	}
}
