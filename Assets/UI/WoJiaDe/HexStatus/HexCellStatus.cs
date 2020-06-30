using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCellStatus : MonoBehaviour
{
	public Transform buildingStatus;
	public Transform terrainStatus;
	public Button buttonUse;
	
	public Image imgBuilding;
	public Image imgTerrain;
	
	public Text txtBuildingLevel;
    public Text txtBuildingDescription;
    public Text txtTerrainDescription;
    public Text txtName;
	
	public HexCell currentHex;
	
	private GameManager gameManager;
	private Sprite sprite;
	private OtherDescriptionReader otherDescriptionReader;
	private OtherDescriptionReader.OtherData otherData;
	
	public void OnEnable()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		otherDescriptionReader=new OtherDescriptionReader();
	}
	
    public void UpdateHexStatusPanel(HexCell hexCell)
    {
		currentHex=hexCell;
        UpdatePanel();
    }
    private void UpdatePanel()
    {
		if(currentHex.building==null)
		{
			buildingStatus.gameObject.SetActive(false);
			terrainStatus.gameObject.SetActive(true);
			
			if((sprite=Resources.Load("Image/galleryThings/terrain/"+currentHex.hexType.ToString(), typeof(Sprite)) as Sprite)!=null)
			{
				imgTerrain.sprite=sprite;
			}
			otherData=otherDescriptionReader.GetTerrainData(currentHex.hexType);
			txtTerrainDescription.text=otherData.effect;
			txtName.text=currentHex.hexType.ToString();
		}
		else
		{
			buildingStatus.gameObject.SetActive(true);
			terrainStatus.gameObject.SetActive(false);
			
			buttonUse.gameObject.SetActive(currentHex.building.GetBuildingType()==BuildingType.Altar?true:false);
			
			if((sprite=Resources.Load("Image/galleryThings/buildings/"+currentHex.building.GetBuildingType().ToString(), typeof(Sprite)) as Sprite)!=null)
			{
				imgBuilding.sprite=sprite;
			}
			txtBuildingLevel.text="."+currentHex.building.GetCurrentLevel();
			txtBuildingDescription.text="<size=22>"+currentHex.building.GetDescription()+"</size>";
			txtName.text=currentHex.building.GetBuildingType().ToString();
		}
    }
}
