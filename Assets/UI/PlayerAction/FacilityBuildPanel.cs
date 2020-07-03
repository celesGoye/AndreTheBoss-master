using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityBuildPanel : MonoBehaviour
{
    public FacilityPallete facilityPallete;
    public Product_Item Prefab_product;
	public Upgrade_Item soulcontent;
	public Transform productcontent;
	
	public float width;
	public float height;
	public float size;
	
	public Button higher;
	public Button lower;
	public Button selectDestination;
	
	public Text txtname;
	public Text txtlevel;
	public Text txtsometitle;
	public Text txtselected;
	public Text txtdescription;
	
	private int requireSoul;
	private CharacterReader characterReader;
	private GameManager gameManager;

	public void OnEnable()
	{
		if (gameManager == null)
			gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		if(characterReader==null)
			characterReader = gameManager.characterReader;
		soulcontent.transform.gameObject.SetActive(false);
		productcontent.gameObject.SetActive(false);
		txtsometitle.text="";
	}
	
	public void SetIsSelecting(bool isselecting)
	{
		facilityPallete.isSelecting=isselecting;
		selectDestination.interactable=!isselecting;
	}
	
	public void OnSelectDestination()
	{
		SetIsSelecting(true);
		List<HexCell> cells=gameManager.hexMap.GetTeleporterBuildableCells(gameManager.hexMap.selectedCell, 
						Teleporter.GetMaxDistance(facilityPallete.currentLevel));
		foreach(HexCell cell in cells)
		{
			cell.indicator.gameObject.SetActive(true);
			cell.indicator.SetColor(Indicator.BuildColor);
		}
		gameManager.hexMap.selectedCell.indicator.SetColor(Indicator.StartColor);
	}
	
	public bool IsBuildOK()
	{
		if(facilityPallete.currentType==BuildingType.None||(facilityPallete.ValidProduct.Count>0&&facilityPallete.currentItem==ItemType.NUM))
			return false;
		if(gameManager.itemManager.ItemsOwn[ItemType.Soul]<requireSoul)
			return false;
		if(facilityPallete.currentType==BuildingType.Teleporter&&(facilityPallete.isSelecting==true||facilityPallete.currentDestination==null))
			return false;
		if(!gameManager.buildingManager.IsBuildingBuilt(facilityPallete.currentType,facilityPallete.currentItem))
			return false;
		return true;
	}
	
	public void ConsumeItem()
	{
		Debug.Log("Consume.");
		gameManager.itemManager.ConsumeItem(ItemType.Soul, requireSoul);
	}
	
	public void ClearProduct(int itemcount)
	{
		if(productcontent.childCount>itemcount)
			for(int i=itemcount;i<productcontent.childCount;i++)
			{
				GameObject.Destroy(productcontent.GetChild(i).gameObject);
			}
	}
	
	public void UpdateBuildPanel()
	{
		txtname.text=facilityPallete.currentType.ToString();
		txtlevel.text=facilityPallete.currentLevel+"";
		SetIsSelecting(false);
		if (gameManager == null)
			Debug.Log("GameManager is null");
		gameManager.hexMap.HideIndicator();
		gameManager.hexMap.selectedCell.indicator.gameObject.SetActive(true);
		gameManager.hexMap.selectedCell.indicator.SetColor(Indicator.StartColor);
		selectDestination.transform.gameObject.SetActive(false);
		soulcontent.transform.gameObject.SetActive(false);
		productcontent.gameObject.SetActive(false);
		txtselected.transform.gameObject.SetActive(false);
		switch((int)facilityPallete.currentType)
		{
			case 0:
			case 1:
				txtsometitle.text="Product";
				break;
			case 2:
				txtsometitle.text="Destination";
				selectDestination.transform.gameObject.SetActive(true);
				if(facilityPallete.currentDestination!=null)
				{
					txtselected.transform.gameObject.SetActive(true);
					facilityPallete.currentDestination.indicator.gameObject.SetActive(true);
					facilityPallete.currentDestination.indicator.SetColor(Indicator.EndColor);
				}
				break;
			default:
				txtsometitle.text="";
				break;
		}
		
		if(IsBuildOK())
		{
			txtdescription.gameObject.SetActive(true);
			facilityPallete.facilityBuildButton.GetComponent<Button>().interactable=true;
			
			txtdescription.text="<size=22>"+Building.GetDescription(facilityPallete.currentType,facilityPallete.currentItem,facilityPallete.currentLevel)+"</size>";
		}
		else
		{
			txtdescription.gameObject.SetActive(false);
			facilityPallete.facilityBuildButton.GetComponent<Button>().interactable=false;
		}
		ClearProduct(facilityPallete.ValidProduct.Count);

		if(facilityPallete.currentType!=BuildingType.None)
		{
			if(facilityPallete.ValidProduct.Count>0)
			{
				productcontent.gameObject.SetActive(true);
				for(int i=0;i<facilityPallete.ValidProduct.Count;i++)
				{
					Product_Item product=null;
					
					if(i<productcontent.childCount)
						product=productcontent.GetChild(i).GetComponent<Product_Item>();
					else
						product=GenerateProductItem(i);
					
					product.type=facilityPallete.ValidProduct[i];
					product.UpdateItemDisplay();
				}
				if(facilityPallete.currentItem!=ItemType.NUM)
				{
					soulcontent.transform.gameObject.SetActive(true);
				
					requireSoul= Building.GetRequireSouls(facilityPallete.currentType,facilityPallete.currentLevel);
					soulcontent.num=gameManager.itemManager.ItemsOwn[ItemType.Soul];
					soulcontent.numneed=requireSoul;
				}
			}
			else
			{
				soulcontent.transform.gameObject.SetActive(true);
				
				requireSoul= Building.GetRequireSouls(facilityPallete.currentType,facilityPallete.currentLevel);
				soulcontent.num=gameManager.itemManager.ItemsOwn[ItemType.Soul];
				soulcontent.numneed=requireSoul;
			}
			
			productcontent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, productcontent.childCount*width*UnityEngine.Screen.height); 
	
		}
			
		UpdateLevelButton();
		UpdateItem(soulcontent);
	}
	
	public Product_Item GenerateProductItem(int index)
	{
		Product_Item newproduct=Instantiate<Product_Item>(Prefab_product);
		newproduct.transform.SetParent(productcontent);
		newproduct.facilityPallete=this.facilityPallete;
		
		newproduct.index=index;
		newproduct.size=size;
		newproduct.width=width;
		newproduct.height=height;
		return newproduct;
	}
	
	public void UpdateItem(Upgrade_Item item)
	{
		item.index=0;
		item.size=size;
		item.width=width;
		item.UpdateItemDisplay();
	}
	
	public void UpdateLevelButton()
	{
		higher.interactable=facilityPallete.currentLevel<facilityPallete.currentMaxLevel?true:false; 
		lower.interactable=facilityPallete.currentLevel>1?true:false; 
	}
	
	public void OnHigherLevel()
	{
		facilityPallete.currentLevel++;
		facilityPallete.currentDestination=null;
		UpdateBuildPanel();
	}
	public void OnLowerLever()
	{
		facilityPallete.currentLevel--;
		facilityPallete.currentDestination=null;
		UpdateBuildPanel();
	}
	
	public void Update()
	{
		foreach(Transform child in productcontent)
		{
			child.GetComponent<Product_Item>().UpdatePosition();
		}
	}
}
