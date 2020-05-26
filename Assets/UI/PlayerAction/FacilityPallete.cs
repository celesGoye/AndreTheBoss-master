using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityPallete : MonoBehaviour
{
	public GameManager gameManager;
    public FacilityDisplay Prefab_facility;
	public Product_Item Prefab_item;
	public FacilityBuildPanel facilityBuildPanel;
	
	public Transform content;
	public Transform productcontent;
	
	public float width;
	public float height;
	public float size;
	
	public Color color;
	public BuildingType currentType;
	public ItemType currentItem;
	public int currentLevel;
	public int currentMaxLevel;
	public HexCell currentDestination;
	public List<ItemType> ValidProduct;
	
	public bool isSelecting;
	
	public FacilityBuildButton facilityBuildButton;
	
    //private BuildingReader buildingReader;
	private List<BuildingType> BuildableFacility;
	
	public void OnEnable()
	{
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		BuildableFacility=new List<BuildingType>();
		ValidProduct=new List<ItemType>();
		//BuildingReader =gameManager.buildingReader;
		currentType=BuildingType.None;
		currentItem=ItemType.NUM;
		currentLevel=1;
		facilityBuildPanel.UpdateBuildPanel();
		UpdateFacilityPallete();
		gameManager.hexMap.selectedCell.indicator.gameObject.SetActive(true);
		gameManager.hexMap.selectedCell.indicator.SetColor(Indicator.StartColor);
	}
	
	public void ClearContent(int count)
	{
		for(int i=count;i<content.childCount;i++)
		{
			GameObject.Destroy(content.GetChild(i).gameObject);
		}
	}
	
	public void UpdateFacilityPallete()
	{
		BuildableFacility.Clear();
		for(int i=0;i<(int)BuildingType.None;i++)
		{
			BuildableFacility.Add((BuildingType)i);
		}
		if(gameManager.buildingManager.IsAltarBuilt())
			BuildableFacility.Remove(BuildingType.Altar);
		FacilityDisplay facility;
		ClearContent(BuildableFacility.Count);
		for(int i=0;i<BuildableFacility.Count;i++)
		{
			if(content.childCount<=i)
			{
				facility=GenerateFacilityDisplay(i);
				facility.type=BuildableFacility[i];
				facility.InitFacilityDisplay();
			}
			else
			{
				facility=content.GetChild(i).GetComponent<FacilityDisplay>();
				facility.type=BuildableFacility[i];
			}
		}
		
       content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (((BuildableFacility.Count<=4?4:(BuildableFacility.Count))-1)*height+size)*UnityEngine.Screen.height); 
	}
	
	public FacilityDisplay GenerateFacilityDisplay(int index)
	{
		FacilityDisplay newfacility=Instantiate<FacilityDisplay>(Prefab_facility);
		newfacility.transform.SetParent(content);
		newfacility.facilityPallete=this;
		
		newfacility.index=index;
		newfacility.size=size;
		newfacility.width=width;
		newfacility.height=height;
		return newfacility;
	}
	
	public void OnFacilityButton()
	{
		for(int i=0;i<BuildableFacility.Count;i++)
		{
			FacilityDisplay facility=content.transform.GetChild(i).GetComponent<FacilityDisplay>();
			facility.gameObject.GetComponent<Image>().color=facility.type!=currentType?Color.white:color;
		}
		currentItem=ItemType.NUM;
		currentLevel=1;
		currentDestination=null;
		productcontent.gameObject.SetActive(true);
		facilityBuildPanel.UpdateBuildPanel();
		UnselectItem();
	}
	
	public void OnItemButton()
	{
		for(int i=0;i<ValidProduct.Count;i++)
		{
			Product_Item item=productcontent.transform.GetChild(i).GetComponent<Product_Item>();
			item.gameObject.GetComponent<Image>().color=item.type!=currentItem?Color.white:color;
		}
		facilityBuildPanel.UpdateBuildPanel();
	}
	
	public void UnselectItem()
	{
		if(currentType==BuildingType.None)
			return;
		for(int i=0;i<ValidProduct.Count;i++)
		{
			Product_Item item=productcontent.transform.GetChild(i).GetComponent<Product_Item>();
			item.gameObject.GetComponent<Image>().color=Color.white;
		}
	}
}
