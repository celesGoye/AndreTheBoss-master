using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    public void OnEnable()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        //Save();
    }

    GameManager gm;

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
                SerializablePawnData pawndata = new SerializablePawnData();

                pawndata.hexcellIndex = monster.currentCell.index;
                pawndata.level = monster.level;
                pawndata.skipCounter = monster.skipCounter;
                pawndata.isSkip = monster.isSkip;
                pawndata.isIgnoreDefense = monster.isIgnoreDefense;
                pawndata.isIgnoreMagicDefense = monster.isIgnoreMagicDefense;
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
                    monster = gm.boss;
                }
                else
                {
                    monster = gm.monsterManager.CreateMonster((MonsterType)data.monsterType, gm.hexMap.cells[pawndata.hexcellIndex], pawndata.level);
                }

                monster.skipCounter = pawndata.skipCounter;
                monster.isSkip = pawndata.isSkip;
                monster.isDirty = true;
                monster.isIgnoreDefense = pawndata.isIgnoreDefense;
                monster.isIgnoreMagicDefense = pawndata.isIgnoreMagicDefense;

                for (int j = 0; j < pawndata.buffs.Count; j++)
                {
                    SerializableBuff buff = pawndata.buffs[j];
                    monster.buffs.Add(new Vector3(buff.attributeType, buff.modifiedValue, buff.counter));
                }

                gm.hexMap.SetCharacterCell(monster, gm.hexMap.cells[pawndata.hexcellIndex]);
            }


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
