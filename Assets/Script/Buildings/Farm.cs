using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building
{
    public override string GetDescription()
	{
		string description="Produce: "+itemTypeProduced+"*"+GetCurrentProduceNumber();
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
