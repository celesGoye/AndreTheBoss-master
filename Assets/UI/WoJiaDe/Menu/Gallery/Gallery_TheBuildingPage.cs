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
	
	private List<ItemType> list;
	private Sprite sprite;

	public void OnEnable()
	{
		
	}
	public void UpdateBuilding()
	{
		name.text=buildingType.ToString();
		effect.text="";
		list=Building.GetValidProduct(buildingType);
		if(list.Count==0)
		{
			for(int j=1;j<=Building.GetMaxLevel(buildingType);j++)
					effect.text+="Lv."+j+"    "+Building.GetDescription(buildingType,ItemType.NUM,j)+"\n";
		}
		else
		{
			for(int i=0;i<list.Count;i++)
			{
				for(int j=1;j<=Building.GetMaxLevel(buildingType);j++)
					effect.text+="Lv."+j+"    "+Building.GetDescription(buildingType,list[i],j)+"\n";
			}
		}
		
		if((sprite=Resources.Load("Image/galleryThings/buildings/"+buildingType.ToString(), typeof(Sprite)) as Sprite)!=null)
			image.sprite=sprite;
	}
}
