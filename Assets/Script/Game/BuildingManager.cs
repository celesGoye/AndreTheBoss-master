using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	
	public int buildRange;	
	public bool buildmode;
	
	public List<Building> Buildings;
	public Dictionary<ItemType,int> itemProduced;
	
	public Building BuildingPrefab_Farm_1_Spiderlily;
	public Building BuildingPrefab_Farm_2_Spiderlily;
	public Building BuildingPrefab_Farm_3_Spiderlily;
	public Building BuildingPrefab_Farm_1_DemonFruit;
	public Building BuildingPrefab_Farm_2_DemonFruit;
	public Building BuildingPrefab_Farm_3_DemonFruit;
	public Building BuildingPrefab_Farm_1_GoldenApple;
	public Building BuildingPrefab_Farm_2_GoldenApple;
	public Building BuildingPrefab_Farm_3_GoldenApple;
	public Building BuildingPrefab_Mine_1_Sulphur;
	public Building BuildingPrefab_Mine_2_Sulphur;
	public Building BuildingPrefab_Mine_3_Sulphur;
	public Building BuildingPrefab_Mine_1_Mercury;
	public Building BuildingPrefab_Mine_2_Mercury;
	public Building BuildingPrefab_Mine_3_Mercury;
	public Building BuildingPrefab_Mine_1_Gold;
	public Building BuildingPrefab_Mine_2_Gold;
	public Building BuildingPrefab_Mine_3_Gold;
	public Building BuildingPrefab_Teleporter_1;
	public Building BuildingPrefab_Teleporter_2;
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
            {new Vector3((int)BuildingType.Farm,1,0), BuildingPrefab_Farm_1_Spiderlily },
            {new Vector3((int)BuildingType.Farm,2,0), BuildingPrefab_Farm_2_Spiderlily },
            {new Vector3((int)BuildingType.Farm,3,0), BuildingPrefab_Farm_3_Spiderlily },
			{new Vector3((int)BuildingType.Farm,1,1), BuildingPrefab_Farm_1_DemonFruit },
            {new Vector3((int)BuildingType.Farm,2,1), BuildingPrefab_Farm_2_DemonFruit },
            {new Vector3((int)BuildingType.Farm,3,1), BuildingPrefab_Farm_3_DemonFruit },
			{new Vector3((int)BuildingType.Farm,1,2), BuildingPrefab_Farm_1_GoldenApple },
            {new Vector3((int)BuildingType.Farm,2,2), BuildingPrefab_Farm_2_GoldenApple },
            {new Vector3((int)BuildingType.Farm,3,2), BuildingPrefab_Farm_3_GoldenApple },
			
            {new Vector3((int)BuildingType.Mine,1,0), BuildingPrefab_Mine_1_Sulphur },
            {new Vector3((int)BuildingType.Mine,2,0), BuildingPrefab_Mine_2_Sulphur },
            {new Vector3((int)BuildingType.Mine,3,0), BuildingPrefab_Mine_3_Sulphur },
			{new Vector3((int)BuildingType.Mine,1,1), BuildingPrefab_Mine_1_Mercury },
            {new Vector3((int)BuildingType.Mine,2,1), BuildingPrefab_Mine_2_Mercury },
            {new Vector3((int)BuildingType.Mine,3,1), BuildingPrefab_Mine_3_Mercury },
			{new Vector3((int)BuildingType.Mine,1,2), BuildingPrefab_Mine_1_Gold },
            {new Vector3((int)BuildingType.Mine,2,2), BuildingPrefab_Mine_2_Gold },
            {new Vector3((int)BuildingType.Mine,3,2), BuildingPrefab_Mine_3_Gold },
			
			{new Vector3((int)BuildingType.Teleporter,1,-1), BuildingPrefab_Teleporter_1 },
            {new Vector3((int)BuildingType.Teleporter,2,-1), BuildingPrefab_Teleporter_2 },
            {new Vector3((int)BuildingType.Altar,1,-1), BuildingPrefab_Altar },
        };
	}
	
	
	public Building CreateBuilding(BuildingType buildingType,ItemType itemType, HexCell cellToBuild, int level)
    {
		
        Building building = GameObject.Instantiate<Building>(prefabs[new Vector3((int)buildingType,level,Building.GetValidProduct(buildingType).IndexOf(itemType))]);
        building.transform.SetParent(transform);
        gameManager.hexMap.SetBuildingCell(building, cellToBuild);

       building.InitBuilding(buildingType,itemType,level);
	   Buildings.Add(building);
        return building;
    }
	
    public void UpdateBuildMode(bool isbuildmode)
	{
		buildmode=isbuildmode;
		gameManager.gameInteraction.Clear();
		ShowBuildableHex();
		if(buildmode)
			gameManager.gameCamera.FocusOnPoint(gameManager.monsterManager.MonsterPawns[0].transform.position);
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
	
	public void BuildingsOnPlayerTurnBegin()
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
				logstring+="<color=#6A5ACD>  "+key.ToString()+"</color>*"+itemProduced[key]+";";
			}
		}
		gameManager.gameInteraction.uilog.UpdateLog(logstring);
	}
	
	public void DestroyBuilding(Building building)
	{
		if(Buildings.Contains(building))
		{
			if((building as Teleporter)!=null)
			{
				((Teleporter)building).TeleporterDestroy();
			}
			building.currentCell.building=null;
			GameObject.Destroy(building.gameObject);
			
		}
		else
		{
			Debug.Log("destroy building not found");
		}
	}
}
