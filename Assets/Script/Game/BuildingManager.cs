using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	
	public int buildRange;	
	public bool buildmode;
	
	public List<Building> Buildings;
	public Dictionary<ItemType,int> itemProduced;
	
	public Building BuildingPrefab_Farm_Spiderlily;
	public Building BuildingPrefab_Farm_DemonFruit;
	public Building BuildingPrefab_Farm_GoldenApple;
	public Building BuildingPrefab_Mine_Sulphur;
	public Building BuildingPrefab_Mine_Mercury;
	public Building BuildingPrefab_Mine_Gold;
	public Building BuildingPrefab_Teleporter;
	public Building BuildingPrefab_Altar;
	
	//public HexCell currentHex;
	
    private Dictionary<Vector3, Building> prefabs;
	private GameManager gameManager;
    private GameObject BuildingRoot;
	
	public void OnEnable()
	{
		buildmode=false;
	}
	
	public void InitBuildingManager()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        Buildings = new List<Building>();

        BuildingRoot = new GameObject();
        BuildingRoot.transform.SetParent(transform);
        BuildingRoot.transform.position = Vector3.zero;

        prefabs = new Dictionary<Vector3, Building>
        {
            {new Vector2((int)BuildingType.Farm,0), BuildingPrefab_Farm_Spiderlily },
			{new Vector2((int)BuildingType.Farm,1), BuildingPrefab_Farm_DemonFruit },
			{new Vector2((int)BuildingType.Farm,2), BuildingPrefab_Farm_GoldenApple },
			
            {new Vector2((int)BuildingType.Mine,0), BuildingPrefab_Mine_Sulphur },
			{new Vector2((int)BuildingType.Mine,1), BuildingPrefab_Mine_Mercury },
			{new Vector2((int)BuildingType.Mine,2), BuildingPrefab_Mine_Gold },
			
			{new Vector2((int)BuildingType.Teleporter,-1), BuildingPrefab_Teleporter },
            {new Vector2((int)BuildingType.Altar,-1), BuildingPrefab_Altar },
        };
	}
	
	public void ClearBuildings()
    {
		for(int i = 0; i < Buildings.Count; i++)
        {
			GameObject.Destroy(Buildings[i]);
        }
		Buildings.Clear();
    }
	
	public Building CreateBuilding(BuildingType buildingType,ItemType itemType, HexCell cellToBuild, int level)
    {
		
        Building building = GameObject.Instantiate<Building>(prefabs[new Vector2((int)buildingType,Building.GetValidProduct(buildingType).IndexOf(itemType))]);
        building.transform.SetParent(transform);
        gameManager.hexMap.SetBuildingCell(building, cellToBuild);

		building.InitBuilding(buildingType,itemType,level);
		building.SetAppearance(level);
		Buildings.Add(building);
	   
		gameManager.animationManager.PlayCreateEff(building.transform.position);
       return building;
    }
	
    public void UpdateBuildMode(bool isbuildmode)
	{
		buildmode=isbuildmode;
		gameManager.gameInteraction.Clear();
		ShowBuildableHex();
		//if(buildmode)
		//	gameManager.gameCamera.FocusOnPoint(gameManager.monsterManager.MonsterPawns[0].transform.position);
	}
	
	public void ShowBuildableHex()
	{
		if(gameManager.monsterManager.MonsterPawns.Count!=0)
		gameManager.hexMap.UpdateBuildableCells(gameManager.monsterManager.MonsterPawns[0].currentCell, buildRange ,buildmode);
	}
	
	public bool IsAltarBuilt()
	{
		foreach(Building building in Buildings)
		{
			if(building.GetBuildingType()==BuildingType.Altar)
				return true;
		}
		return false;
	}
	
	public void OnMonsterTurnBegin()
	{
		itemProduced=new Dictionary<ItemType,int>();
		foreach(Building building in Buildings)
		{
			if(building.GetItemType()!=ItemType.NUM)
			{
				if(itemProduced.ContainsKey(building.GetItemType()))
					itemProduced[building.GetItemType()]+=building.GetCurrentProduceNumber();
				else
					itemProduced[building.GetItemType()]=building.GetCurrentProduceNumber();
			}
			building.OnPlayerTurnBegin();
		}
		
		string logstring="";
		if(itemProduced.Count==0)
		{
			logstring="The day is over,You have produced nothing;";
		}
		else
		{
			logstring="The day is over,You have produced the following items:";
			foreach(ItemType key in itemProduced.Keys)
			{
				gameManager.itemManager.GetItem(key,itemProduced[key]);
				logstring+=TextColor.SetTextColor(key.ToString(),TextColor.ItemColor)+"*"+itemProduced[key]+";";
			}
		}
		gameManager.gameInteraction.uilog.UpdateLog(logstring);
	}
	
	
	
	public void BuildingAccelerate(Building building)
	{
		if(building.GetItemType()!=ItemType.NUM)
			gameManager.itemManager.GetItem(building.GetItemType(),building.GetCurrentProduceNumber());
	}
	
	public void DestroyBuilding(Building building)
	{
		if(Buildings.Contains(building))
		{
			Buildings.Remove(building);
			building.currentCell.building=null;
			building.DestroyBuilding();
		}
		else
		{
			Debug.Log("destroy building not found");
		}
	}
}
