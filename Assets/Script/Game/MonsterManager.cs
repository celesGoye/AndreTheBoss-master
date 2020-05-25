using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private GameManager gameManager;

    public List<Monster> MonsterPawns;

    // the type of revived enemy should be set as PawnType.Monster
    public List<Enemy> RevivedEnemyPawns;

    public Monster MonsterPrefab_zombie;
    public Monster MonsterPrefab_sprite;
    public Monster MonsterPrefab_dwarf;
    public Monster MonsterPrefab_giant;
    public Boss Boss_Prefab;

    private Dictionary<MonsterType, Monster> prefabs;

    private GameObject MonsterRoot;

    public void OnEnable()
    {
		InitMonsterManager();
    }
	 public void InitMonsterManager()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        MonsterPawns = new List<Monster>();
        RevivedEnemyPawns = new List<Enemy>();

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
        Monster monster = GameObject.Instantiate<Monster>(prefabs[type]);
        monster.transform.SetParent(transform);
        gameManager.hexMap.SetCharacterCell(monster, cellToSpawn);

        gameManager.characterReader.InitMonsterData(ref monster, GetMonsterUnlockLevel(type), type, level);
        gameManager.hexMap.RevealCellsFrom(monster.currentCell);
		monster.healthbar=gameManager.healthbarManager.InitializeHealthBar(monster);
		MonsterPawns.Add(monster);
        return monster;
    }

    public int GetMonsterUnlockLevel(MonsterType type)
    {
        return Mathf.CeilToInt((float)type / 3);
    }

    public bool IsFriendlyUnit(Pawn pawn)
    {
        if (pawn == null)
            return false;

        if (RevivedEnemyPawns.Contains((Enemy)pawn) || MonsterPawns.Contains((Monster)pawn))
            return true;

        return false;
    }

}
