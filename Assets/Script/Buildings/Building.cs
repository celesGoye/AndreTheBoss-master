using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building: MonoBehaviour
{
    public static List<Dictionary<int, int>> requireSouls =new List<Dictionary<int, int>>{
		new Dictionary<int, int> { { 1, 1 }, { 2, 3 }, { 3, 7 } },new Dictionary<int, int> { { 1, 1 }, { 2, 3 }, { 3, 7 } },
		new Dictionary<int, int> { { 1, 5 }, { 2, 10 } },new Dictionary<int, int> { { 1, 10 } }	} ;
	public static List<Dictionary<int, int>> produceItems =new List<Dictionary<int, int>>{
		new Dictionary<int, int> { { 1, 1 }, { 2, 3 }, { 3, 5 } },new Dictionary<int, int> { { 1, 1 }, { 2, 3 }, { 3, 5 } },
		new Dictionary<int, int> (),new Dictionary<int, int> ()};
		
	public HexCell currentCell;
	private BuildingType buildingType;
    private int currentLevel; // 1,2,3
    private ItemType itemTypeProduced;
	
    public void InitBuilding(BuildingType type, ItemType itemTypeProduced, int initLevel)
    {
		buildingType=type;
        currentLevel = Mathf.Clamp(initLevel, 1,GetMaxLevel(type));
		if(GetValidProduct(type).Contains(itemTypeProduced))
			this.itemTypeProduced = itemTypeProduced;
    }

    public int LevelUp(BuildingType type, int souls) // return souls used
    {
        if (currentLevel == GetMaxLevel(type))
            return 0;
		int before=0;
		int after=0;
		requireSouls[(int)type].TryGetValue(currentLevel + 1, out after);
		requireSouls[(int)type].TryGetValue(currentLevel,out before);
        int required = after-before;
        if (souls < required)
            return 0;

        currentLevel++;
        return required;
    }

    public int GetCurrentProduceNumber()
    {
		int result=0;
		produceItems[(int)buildingType].TryGetValue(currentLevel,out result);
		return result;
    }
	
	public static int GetMaxLevel(BuildingType type)
	{
		return requireSouls[(int)type].Count;
	}
	
	public static int GetRequireSouls(BuildingType type,int level)
	{
		int result=0;
		requireSouls[(int)type].TryGetValue(level,out result);
		return result;
	}
	
	public static List<ItemType> GetValidProduct(BuildingType type)
	{
		List<ItemType> list=new List<ItemType>();;
		switch((int)type)
		{
			case 0:
				list=new List<ItemType>(){ItemType.Spiderlily,ItemType.DemonFruit,ItemType.GoldenApple};
				break;
			case 1:
				list=new List<ItemType>(){ItemType.Sulphur,ItemType.Mercury,ItemType.Gold};
				break;
			default:
				break;
		}
		return list;
	}
	
	/*public Vector3 GetBuildingData()
	{
		Vector3 data=new Vector3();
		data.x=(int)buildingType;
		data.y=currentLevel;
		data.z=GetValidProduct(buildingType).IndexOf(itemTypeProduced);
		return data;
	}*/
	
	public BuildingType GetBuildingType()
	{
		return buildingType;
	}
	
	public ItemType GetItemType()
	{
		if(GetValidProduct(buildingType).Count>0)
			return itemTypeProduced;
		else
			return ItemType.NUM;
	}
}

