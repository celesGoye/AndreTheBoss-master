using System;
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
	public Monster MonsterPrefab_druid;
	
    public Monster MonsterPrefab_dwarf;
    public Monster MonsterPrefab_giant;
	public Monster MonsterPrefab_ghoul;
	
	public Monster MonsterPrefab_stoneman;
	public Monster MonsterPrefab_goblin;
	public Monster MonsterPrefab_bloodseeker;
	
	public Monster MonsterPrefab_chimera;
	public Monster MonsterPrefab_bugbear;
	public Monster MonsterPrefab_drow;
	
    public Monster MonsterPrefab_centaur;
    public Monster MonsterPrefab_mindflayer;
    public Monster MonsterPrefab_boss;

    private Dictionary<MonsterType, Monster> prefabs;

    private GameObject MonsterRoot;

	 public void InitMonsterManager()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        MonsterPawns = new List<Monster>();
        RevivedEnemyPawns = new List<Enemy>();

        MonsterRoot = new GameObject("Monster Root");
        MonsterRoot.transform.SetParent(transform);
        MonsterRoot.transform.position = Vector3.zero;

        prefabs = new Dictionary<MonsterType, Monster>
        {
            {MonsterType.boss, MonsterPrefab_boss },
            {MonsterType.dwarf, MonsterPrefab_dwarf},
            {MonsterType.giant, MonsterPrefab_giant },
			{MonsterType.ghoul, MonsterPrefab_ghoul},
			{MonsterType.druid, MonsterPrefab_druid },
            {MonsterType.sprite, MonsterPrefab_sprite },
            {MonsterType.zombie, MonsterPrefab_zombie },
            {MonsterType.stoneman, MonsterPrefab_stoneman },
            {MonsterType.goblin, MonsterPrefab_goblin },
            {MonsterType.bloodseeker, MonsterPrefab_bloodseeker },
            {MonsterType.chimera, MonsterPrefab_chimera },
            {MonsterType.bugbear, MonsterPrefab_bugbear },
            {MonsterType.drow, MonsterPrefab_drow },
            {MonsterType.centaur, MonsterPrefab_centaur },
            {MonsterType.mindflayer, MonsterPrefab_mindflayer }
            //{MonsterType.dragon, MonsterPrefab_dragon }
        };
    }
	
    public void ClearMonster()
    {
        for (int i = 0; i < MonsterPawns.Count; i++)
        {
            if (MonsterPawns[i].monsterType == MonsterType.boss)
                continue;
            else
            {
                GameObject.Destroy(MonsterPawns[i]);
                MonsterPawns.RemoveAt(i);
            }
        }
    }
	
	public void ClearRevivedEnemy()
    {
        for (int i = 0; i < RevivedEnemyPawns.Count; i++)
        {
            GameObject.Destroy(RevivedEnemyPawns[i]);
        }
        RevivedEnemyPawns.Clear();
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
		gameManager.monsterActionManager.UpdateActionableMonsters();
        monster.transform.SetParent(MonsterRoot.transform);
		gameManager.animationManager.PlayCreateMonEff(monster.transform.position);
        return monster;
    }

    public Enemy CreateRevivedEnemy(EnemyType type, HexCell cellToSpawn)
    {
        Enemy enemy = gameManager.enemyManager.SpawnEnemyAtCell(type, cellToSpawn);
        if (enemy == null)
            return null;

        gameManager.enemyManager.EnemyPawns.Remove(enemy);
        RevivedEnemyPawns.Add(enemy);

        // The Enemy revived being a monster
        enemy.pawnType = PawnType.Monster;
        return enemy;
    }
	
    public int GetMonsterUnlockLevel(MonsterType type)
    {
        return Mathf.CeilToInt((float)type / 3);
    }

    public bool IsFriendlyUnit(Pawn pawn)
    {
        if (pawn == null)
            return false;
        if (RevivedEnemyPawns.Contains(pawn as Enemy) || MonsterPawns.Contains(pawn as Monster))
            return true;
        try
        {
            if (MonsterPawns.Contains((Monster)pawn))
                return true;
        }catch(InvalidCastException ex)
        {
            Debug.Log(ex.StackTrace);
            try
            {
                if (RevivedEnemyPawns.Contains((Enemy)pawn))
                    return true;
            }catch(InvalidCastException ex2)
            {
                Debug.Log(ex2.StackTrace);
            }
        }
        return false;
    }


    public void OnMonsterTurnBegin()
    {
		foreach(Monster monster in MonsterPawns)
        {
            monster.OnActionBegin();
        }
    }

    public void OnMonsterTurnEnd()
    {
		foreach(Monster monster in MonsterPawns)
        {
            monster.OnActionEnd();
        }
    }

    public void Update()
    {
        foreach(Monster monster in MonsterPawns)
        {
            monster.healthbar.UpdateLife();
        }
        foreach(Enemy enemy in RevivedEnemyPawns)
        {
            enemy.healthbar.UpdateLife();
        }
    }
	
	public void RemoveMonster(Monster monster)
    {
        if(MonsterPawns.Contains(monster))
        {
            MonsterPawns.Remove(monster);
        }
    }

    public void RemoveRevivedEnemy(Enemy enemy)
    {
        if(RevivedEnemyPawns.Contains(enemy))
        {
            RevivedEnemyPawns.Remove(enemy);
        }
    }
}
