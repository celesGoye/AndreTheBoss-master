using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_TheBuildingPage : MonoBehaviour
{
	public BuildingType buildingType;
	
	public Text name;
	public Text story;
	public Text effect;
	public Image image;

	public void OnEnable()
	{
		
	}
	public void UpdateBuilding()
	{
		name.text=buildingType.ToString();
	}
}
