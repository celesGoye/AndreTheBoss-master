using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<SerializableHexCellData> hexcellData;
    public List<SerializableEnemyData> enemyData;
    public List<SerializableMonsterData> monsterData;
    public List<SerializableEnemyData> revivedEnemyData;
    public SerializablePlayerData playerData;	
    public List<SerializableBuildingData> buildingData;
	//public List<SerializableGameEventData> gameeventData;
}

[Serializable]
public struct SerializableHexCellData
{
    public int hexType;
    public bool activeSelf;
}

[Serializable]
public struct SerializablePawnData
{
    public int hexcellIndex;

	public int level;
	public int skipCounter;
	public int currentHP;
	public bool isSkip;
	public bool isIgnoreDefense;
	public bool isIgnoreMagicDefense;
	public List<SerializableBuff> buffs;
}

[Serializable]
public struct SerializableBuff
{
	public int attributeType;
	public int modifiedValue;
	public int counter;
}

[Serializable]
public struct SerializableEnemyData
{
    public int enemyType;
    public SerializablePawnData pawnData;
}

[Serializable]
public struct SerializableMonsterData
{
	public int monsterType;
	public SerializablePawnData pawnData;
}

[Serializable]
public struct SerializablePlayerData
{
	public int turnNumber;
	public List<ItemType> ItemsGot;
	public Dictionary<ItemType, int> ItemsOwn;
}

[Serializable]
public struct SerializableBuildingData
{
	public int hexcellIndex;
	public int buildingType;
	public int itemType;
	public int level;
}

/*
public struct SerializableGameEventData
{
	public int eventType;
	public int whichEvent;

	public int hexcellIndex;

}
*/