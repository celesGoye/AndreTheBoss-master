using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;
using System.Data.Common;

public class SaveManager : MonoBehaviour
{
    public void OnEnable()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        //Save();
    }

    GameManager gm;

    public void Start()
    {
        if (PlayerPrefs.GetInt("IsNewGame") == 0)
        {
            Load();
        }
    }

    public void Save()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/atb.dat", FileMode.OpenOrCreate);
        //Debug.Log(Application.persistentDataPath + "/atb.dat");
        BinaryFormatter formatter = new BinaryFormatter();

        try
        {
            SaveData savedata = new SaveData();

            // HexMap
            savedata.hexcellData = new List<SerializableHexCellData>();
            for (int i = 0; i < gm.hexMap.cells.Length; i++)
            {
                SerializableHexCellData data = new SerializableHexCellData();
                data.hexType = (int)gm.hexMap.cells[i].hexType;
                data.activeSelf = gm.hexMap.cells[i].gameObject.activeSelf;
                savedata.hexcellData.Add(data);
            }

            // Monster
            savedata.monsterData = new List<SerializableMonsterData>();
            for (int i = 0; i  < gm.monsterManager.MonsterPawns.Count; i++)
            {
                Monster monster = gm.monsterManager.MonsterPawns[i];
                SerializableMonsterData data = new SerializableMonsterData();
                data.monsterType = (int)monster.monsterType;
                data.equippedSkill = (int)monster.equippedSkill;
                SerializablePawnData pawndata = new SerializablePawnData();

                pawndata.hexcellIndex = monster.currentCell.index;
                pawndata.level = monster.level;
                pawndata.skipCounter = monster.skipCounter;
                pawndata.isSkip = monster.isSkip;
                pawndata.isIgnoreDefense = monster.isIgnoreDefense;
                pawndata.isIgnoreMagicDefense = monster.isIgnoreMagicDefense;
                pawndata.currentHP = monster.currentHP;
                pawndata.buffs = new List<SerializableBuff>();
                for (int j = 0; j < monster.buffs.Count; j++)
                {
                    SerializableBuff buff = new SerializableBuff();
                    buff.attributeType = (int)monster.buffs[j].x;
                    buff.modifiedValue = (int)monster.buffs[j].y;
                    buff.counter = (int)monster.buffs[j].z;
                    pawndata.buffs.Add(buff);
                }

                data.pawnData = pawndata;
                savedata.monsterData.Add(data);
            }

            // Enemy
            savedata.enemyData = new List<SerializableEnemyData>();
            for (int i = 0; i < gm.enemyManager.EnemyPawns.Count; i++)
            {
                Enemy enemy = gm.enemyManager.EnemyPawns[i];
                SerializableEnemyData data = new SerializableEnemyData();
                data.enemyType = (int)enemy.enemyType;
                SerializablePawnData pawndata = new SerializablePawnData();

                pawndata.hexcellIndex = enemy.currentCell.index;
                pawndata.level = enemy.level;
                pawndata.skipCounter = enemy.skipCounter;
                pawndata.isSkip = enemy.isSkip;
                pawndata.isIgnoreDefense = enemy.isIgnoreDefense;
                pawndata.isIgnoreMagicDefense = enemy.isIgnoreMagicDefense;
                pawndata.currentHP = enemy.currentHP;
                pawndata.buffs = new List<SerializableBuff>();
                for (int j = 0; j < enemy.buffs.Count; j++)
                {
                    SerializableBuff buff = new SerializableBuff();
                    buff.attributeType = (int)enemy.buffs[j].x;
                    buff.modifiedValue = (int)enemy.buffs[j].y;
                    buff.counter = (int)enemy.buffs[j].z;
                    pawndata.buffs.Add(buff);
                }
                data.pawnData = pawndata;
                savedata.enemyData.Add(data);
            }

            // Revived Enemy
            savedata.revivedEnemyData = new List<SerializableEnemyData>();
            for (int i = 0; i < gm.monsterManager.RevivedEnemyPawns.Count; i++)
            {
                Enemy enemy = gm.monsterManager.RevivedEnemyPawns[i];
                SerializableEnemyData data = new SerializableEnemyData();
                data.enemyType = (int)enemy.enemyType;
                SerializablePawnData pawndata = new SerializablePawnData();

                pawndata.hexcellIndex = enemy.currentCell.index;
                pawndata.level = enemy.level;
                pawndata.skipCounter = enemy.skipCounter;
                pawndata.isSkip = enemy.isSkip;
                pawndata.isIgnoreDefense = enemy.isIgnoreDefense;
                pawndata.isIgnoreMagicDefense = enemy.isIgnoreMagicDefense;
                pawndata.currentHP = enemy.currentHP;
                pawndata.buffs = new List<SerializableBuff>();
                for (int j = 0; j < enemy.buffs.Count; j++)
                {
                    SerializableBuff buff = new SerializableBuff();
                    buff.attributeType = (int)enemy.buffs[j].x;
                    buff.modifiedValue = (int)enemy.buffs[j].y;
                    buff.counter = (int)enemy.buffs[j].z;
                    pawndata.buffs.Add(buff);
                }
                data.pawnData = pawndata;
                savedata.revivedEnemyData.Add(data);
            }

            // Player data (own items and turn number)
            savedata.playerData = new SerializablePlayerData();
            savedata.playerData.turnNumber = gm.gameTurnManager.GetCurrentGameTurn();
            savedata.playerData.ItemsGot = gm.itemManager.ItemsGot;
            savedata.playerData.ItemsOwn = gm.itemManager.ItemsOwn;

            // Buildings
            savedata.buildingData = new List<SerializableBuildingData>();
            for (int i = 0; i < gm.buildingManager.Buildings.Count; i++)
            {
                SerializableBuildingData data = new SerializableBuildingData();
                Building building = gm.buildingManager.Buildings[i];
                data.buildingType = (int)building.GetBuildingType();
                data.hexcellIndex = building.currentCell.index;
                data.itemType = (int)building.GetItemType();
                data.level = building.GetCurrentLevel();

                savedata.buildingData.Add(data);
            }

            /*
            // GameEvent
            savedata.gameeventData = new List<SerializableGameEventData>();
            for (int i = 0; i < gm.gameEventManager.gameEventDisplayers.Count; i++)
            {
                Debug.Log(gm.gameEventManager.gameEventDisplayers.Count);
                SerializableGameEventData data = new SerializableGameEventData();
                GameEventDisplayer gd = gm.gameEventManager.gameEventDisplayers[i];
                data.eventType = (int)gd.gameEvent.eventType;
                data.whichEvent = gd.gameEvent.whichEvent;
                data.hexcellIndex = gd.currentCell.index;

                savedata.gameeventData.Add(data);
            }
            */

            formatter.Serialize(fs, savedata);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        finally
        {
            fs.Close();
        }
    }

    public void Load()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/atb.dat", FileMode.Open);
        //Debug.Log(Application.persistentDataPath + "/atb.dat");
        BinaryFormatter formatter = new BinaryFormatter();

        try
        {
            SaveData savedata = new SaveData();

            savedata = (SaveData)formatter.Deserialize(fs);

            
            // Hexmap
            gm.hexMap.ClearHexType();
            for (int i = 0; i < gm.hexMap.cells.Length; i++)
            {
                HexCell cell = gm.hexMap.cells[i];
                cell.hexType = (HexType)savedata.hexcellData[i].hexType;
                gm.hexMap.GenerateHexType(cell, cell.hexType);
                cell.gameObject.SetActive(savedata.hexcellData[i].activeSelf);
            }

            
            // Monster
            gm.monsterManager.ClearMonster();
            for (int i = 0; i < savedata.monsterData.Count; i++)
            {
                SerializableMonsterData data = savedata.monsterData[i];
                SerializablePawnData pawndata = data.pawnData;
                Monster monster = null;
                if ((MonsterType)data.monsterType == MonsterType.boss)
                {
                    // boss loading
                    monster = gm.boss;
                    //monster = gm.monsterManager.CreateMonster((MonsterType)data.monsterType, gm.hexMap.cells[pawndata.hexcellIndex], pawndata.level);
                    //Monster oldBoss = gm.boss;
                    //HexCell oldCell = gm.boss.currentCell;
                    //gm.boss = (Boss)monster;
                    //oldBoss.currentCell.pawn = null;
                    //oldBoss.currentCell = null;
                    //oldBoss.GetComponentInChildren<Animator>().SetBool("Die", true);
                    //gm.hexMap.SetCharacterCell(monster, oldCell);

                    while(monster.GetLevel() < pawndata.level)
                    {
                        monster.Upgrade();
                    }

                    Sprite sprite = null;
                    if (pawndata.level == 3 || pawndata.level == 4)
                        sprite = (Sprite)Resources.Load("Image/Character/boss" + 3, typeof(Sprite));
                    else if (pawndata.level == 5)
                        sprite = (Sprite)Resources.Load("Image/Character/boss" + 5, typeof(Sprite));

                    if (sprite != null)
                        monster.GetComponentInChildren<SpriteRenderer>().sprite = sprite;

                    gm.hexMap.SetCharacterCell(monster, gm.hexMap.cells[pawndata.hexcellIndex]);
                    gm.animationManager.PlayCreateMonEff(monster.transform.position);
                }
                else
                {
                    // other monster loading
                    monster = gm.monsterManager.CreateMonster((MonsterType)data.monsterType, gm.hexMap.cells[pawndata.hexcellIndex], pawndata.level);
                }

                monster.equippedSkill = data.equippedSkill;
                monster.skipCounter = pawndata.skipCounter;
                monster.isSkip = pawndata.isSkip;
                monster.isDirty = true;
                monster.isIgnoreDefense = pawndata.isIgnoreDefense;
                monster.isIgnoreMagicDefense = pawndata.isIgnoreMagicDefense;
                monster.currentHP = pawndata.currentHP;

                for (int j = 0; j < pawndata.buffs.Count; j++)
                {
                    SerializableBuff buff = pawndata.buffs[j];
                    monster.buffs.Add(new Vector3(buff.attributeType, buff.modifiedValue, buff.counter));
                }

                gm.hexMap.SetCharacterCell(monster, gm.hexMap.cells[pawndata.hexcellIndex]);
            }

            // Enemy
            gm.enemyManager.ClearEnemy();
            for (int i = 0; i < savedata.enemyData.Count; i++)
            {
                SerializableEnemyData data = savedata.enemyData[i];
                SerializablePawnData pawndata = data.pawnData;
                Enemy enemy = gm.enemyManager.SpawnEnemyAtCell((EnemyType)data.enemyType, gm.hexMap.cells[pawndata.hexcellIndex]);

                if (enemy == null)
                    continue;

                enemy.skipCounter = pawndata.skipCounter;
                enemy.isSkip = pawndata.isSkip;
                enemy.isDirty = true;
                enemy.isIgnoreDefense = pawndata.isIgnoreDefense;
                enemy.isIgnoreMagicDefense = pawndata.isIgnoreMagicDefense;
                enemy.currentHP = pawndata.currentHP;

                for (int j = 0; j < pawndata.buffs.Count; j++)
                {
                    SerializableBuff buff = pawndata.buffs[j];
                    enemy.buffs.Add(new Vector3(buff.attributeType, buff.modifiedValue, buff.counter));
                }
            }

            
            // Revived Enemy
            gm.monsterManager.ClearRevivedEnemy();
            for (int i = 0; i < savedata.revivedEnemyData.Count; i++)
            {
                SerializableEnemyData data = savedata.enemyData[i];
                SerializablePawnData pawndata = data.pawnData;

                Enemy enemy = gm.monsterManager.CreateRevivedEnemy((EnemyType)data.enemyType, gm.hexMap.cells[pawndata.hexcellIndex]);

                if (enemy == null)
                    continue;

                enemy.skipCounter = pawndata.skipCounter;
                enemy.isSkip = pawndata.isSkip;
                enemy.isDirty = true;
                enemy.isIgnoreDefense = pawndata.isIgnoreDefense;
                enemy.isIgnoreMagicDefense = pawndata.isIgnoreMagicDefense;
                enemy.currentHP = pawndata.currentHP;

                for (int j = 0; j < pawndata.buffs.Count; j++)
                {
                    SerializableBuff buff = pawndata.buffs[j];
                    enemy.buffs.Add(new Vector3(buff.attributeType, buff.modifiedValue, buff.counter));
                }
            }

            // Player data (own items and turn number)
            gm.gameTurnManager.ResetCurrentTurn(savedata.playerData.turnNumber);
            gm.itemManager.ItemsGot = savedata.playerData.ItemsGot;
            gm.itemManager.ItemsOwn = savedata.playerData.ItemsOwn;

            
            // Buildings
            gm.buildingManager.ClearBuildings();
            for (int i = 0; i < savedata.buildingData.Count; i++)
            {
                SerializableBuildingData data = savedata.buildingData[i];
                gm.buildingManager.CreateBuilding((BuildingType)data.buildingType, (ItemType)data.itemType, gm.hexMap.cells[data.hexcellIndex], data.level);
            }


            
            /*
            // Game Event
            gm.gameEventManager.ClearGameEventDisplayers();
            for (int i = 0; i < savedata.gameeventData.Count; i++)
            {
                SerializableGameEventData data = savedata.gameeventData[i];
                GameEventDisplayer gd = gm.gameEventManager.CreateNewEventDisplayer(gm.gameEventManager.eventReader.getNewGameEvent((GameEventType)data.eventType, data.whichEvent));
                if (gd != null)
                    gm.hexMap.SetGameEventDisplayerCell(gd, gm.hexMap.cells[data.hexcellIndex]);
            }
            */
            
            

            // after loading
            gm.gameCamera.FocusOnPoint(gm.boss.transform.position);
            
        }
        catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        finally
        {
            fs.Close();
        }
    }
}
