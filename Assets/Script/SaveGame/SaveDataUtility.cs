using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<SerializableHexCellData> hexcellData;		// [x]
    public List<SerializableEnemyData> enemyData;			// [ ]
    public List<SerializableMonsterData> monsterData;		// [x]
    public List<SerializableEnemyData> revivedEnemyData;	// [ ]
    public List<SerializablePlayerData> playerData;			// [ ]
    public List<SerializableBuildingData> buildingData;		// [ ]
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
	public int itemType;
}