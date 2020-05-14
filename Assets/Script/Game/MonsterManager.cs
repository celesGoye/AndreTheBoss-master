using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private GameManager gameManager;

    public List<Monster> MonsterPawns;
	

    public Monster MonsterPrefab_zombie;
    public Monster MonsterPrefab_sprite;
    public Monster MonsterPrefab_dwarf;
    public Monster MonsterPrefab_giant;
    public Boss Boss_Prefab;

    private Dictionary<MonsterType, Monster> prefabs;

    private GameObject MonsterRoot;

    public void OnEnable()
    {
        /*gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        MonsterPawns = new List<Monster>();
        MonsterRoot = new GameObject();
        MonsterRoot.transform.SetParent(transform);
        MonsterRoot.transform.position = Vector3.zero;

        prefabs = new Dictionary<MonsterType, Monster>
        {
            {MonsterType.boss, Boss_Prefab },
            {MonsterType.dwarf, MonsterPrefab_dwarf},
            {MonsterType.giant, MonsterPrefab_giant },
            {MonsterType.sprite, MonsterPrefab_sprite },
            {MonsterType.zombie, MonsterPrefab_zombie }
        };*/
		InitMonsterManager();
    }
	 public void InitMonsterManager()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        MonsterPawns = new List<Monster>();
        MonsterRoot = new GameObject();
        MonsterRoot.transform.SetParent(transform);
        MonsterRoot.transform.position = Vector3.zero;

        prefabs = new Dictionary<MonsterType, Monster>
        {
            {MonsterType.boss, Boss_Prefab },
            {MonsterType.dwarf, MonsterPrefab_dwarf},
            {MonsterType.giant, MonsterPrefab_giant },
            {MonsterType.sprite, MonsterPrefab_sprite },
            {MonsterType.zombie, MonsterPrefab_zombie }
        };
    }

    public Monster CreateMonster(MonsterType type, HexCell cellToSpawn, int level)
    {
		GameObject.Instantiate<Monster>(prefabs[type]);
        Monster monster = GameObject.Instantiate<Monster>(prefabs[type]);
        monster.transform.SetParent(transform);
        gameManager.hexMap.SetCharacterCell(monster, cellToSpawn);
        Pawn pawn = (Pawn)monster;
        gameManager.characterReader.InitPawnData(ref pawn, PawnType.Monster, (int)type, level);
        gameManager.hexMap.RevealCellsFrom(monster.currentCell);
		monster.healthbar=gameManager.healthbarManager.InitializeHealthBar(monster);
		MonsterPawns.Add(monster);
        return monster;
    }


}
