using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    public static string GetDescription(ItemType type,int num)
	{
		string description="Produce: "+type+"*"+num;
		return description;
	}
	
	public override string GetUpgradeDescription()
	{
		string description = "Max.";
		if(currentLevel<Building.GetMaxLevel(buildingType))
			description="Produce: "+itemTypeProduced+"*"+GetCurrentProduceNumber()+"\n→ "+itemTypeProduced+"*"+GetNextLevelProduceNumber();
		return description;
	}
}
