﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;

public class EnemyManager : MonoBehaviour
{
    private GameManager gm;

    public Enemy EnemyPrefab_thief;
    public Enemy EnemyPrefab_sword;
    public Enemy EnemyPrefab_magic;

    public List<Enemy> EnemyPawns;
	private Enemy DeadEnemyPawn = null;
    private EnemyType LastDeadEnemyType = EnemyType.NUM;

    private GameObject EnemyRoot;

    public int MaxEnemyOnMap = 10;

    private static int[] heroAppearingTurn ={
        10, 20, 30, 40, 50
    };
	
	private Dictionary<EnemyType, Enemy> prefabs;

    public void InitEnemyManager()
    {
        EnemyPawns = new List<Enemy>();
        gm = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        EnemyRoot = new GameObject("EnemyRoot");
        EnemyRoot.transform.SetParent(transform);
        EnemyRoot.transform.position = Vector3.zero;
		
		prefabs = new Dictionary<EnemyType, Enemy>
        {
            {EnemyType.wanderingswordman, EnemyPrefab_sword },
            {EnemyType.magicapprentice, EnemyPrefab_magic},
            {EnemyType.thief, EnemyPrefab_thief },
        };
    }

    public void ClearEnemy()
    {
        for (int i = 0; i < EnemyPawns.Count; i++)
        {
            GameObject.Destroy(EnemyPawns[i]);
            EnemyPawns.RemoveAt(i);
        }
    }

    public void OnEnemyTurnBegin()
    {
        // Spawn new Enemy
        Enemy enemy = null;
        if (!heroAppearingTurn.Contains<int>(gm.gameTurnManager.GetCurrentGameTurn()))
            enemy = SpawnEnemy();
        else
            enemy = SpawnHero();

        // Camera focus
        if (enemy != null)
            gm.gameCamera.FocusOnPoint(enemy.transform.position);

        // Enemy movement
        OnEnemyTurn();
    }

    public void OnEnemyTurn()
    {
        currentEnemyIndex = 0;
        if (EnemyPawns.Count > 0)
            EnemyPawns[currentEnemyIndex].OnActionBegin();
        else
            OnEnemyTurnEnd();
    }

    private int currentEnemyIndex = 0;

    public void Update()
    {
        if(gm.gameTurnManager.IsEnemyTurn() && EnemyPawns.Count > 0)
        {
            if(currentEnemyIndex >= EnemyPawns.Count || !EnemyPawns[currentEnemyIndex].IsAction())
            {
                currentEnemyIndex++;
                if(currentEnemyIndex >= EnemyPawns.Count)
                {
                    OnEnemyTurnEnd();
                }
                else
                {
                    EnemyPawns[currentEnemyIndex].OnActionBegin();
                }
            }
        }

        foreach (Enemy enemy in EnemyPawns)
        {
            enemy.healthbar.UpdateLife();
        }
    }

    public void OnEnemyTurnEnd()
    {
        currentEnemyIndex = -1;
        gm.gameTurnManager.NextGameTurn();
    }

    private Enemy SpawnEnemy()
    {
        int turnNum = gm.gameTurnManager.GetCurrentGameTurn();

        if (EnemyPawns.Count >= MaxEnemyOnMap)
            return null;

        EnemyType enemyType = EnemyType.NUM;
        if (turnNum < heroAppearingTurn[0])          // level 1
        {
            enemyType = getRandomEnemyType(1);
        }
        else if(turnNum < heroAppearingTurn[1])     // level 2
        {
            enemyType = getRandomEnemyType(2);
        }
        else if(turnNum < heroAppearingTurn[2])     // level 3
        {
            enemyType = getRandomEnemyType(3);
        }
        else if(turnNum < heroAppearingTurn[3])     // level 4
        {
            enemyType = getRandomEnemyType(4);
        }
        else if(turnNum < heroAppearingTurn[4])     // level 5
        {
            enemyType = getRandomEnemyType(5);
        }

        // TODO: get portal code here
        if(enemyType != EnemyType.NUM)
            return SpawnEnemyAtCell(enemyType, gm.hexMap.GetRandomCellToSpawn());

        return null;
    }

    private Enemy SpawnHero()
    {
        EnemyType heroType = EnemyType.NUM;
        heroType = getHeroType(gm.gameTurnManager.GetCurrentGameTurn() / 10);
        if(heroType != EnemyType.NUM)
        {
            return SpawnEnemyAtCell(heroType, gm.hexMap.GetRandomCellToSpawn());
        }
        return null;
    }

    public Enemy SpawnEnemyAtCell(EnemyType type, HexCell cell)
    {
        Enemy newEnemy = null;
        if (cell.CanbeDestination())
        {
            switch (type)
            {
                case EnemyType.wanderingswordman:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_sword);
                    break;
                case EnemyType.magicapprentice:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_magic);
                    break;
                case EnemyType.thief:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_thief);
                    break;
                default:
                    break;
            }

            if(newEnemy != null)
            {
                gm.characterReader.InitEnemyData(ref newEnemy, getEnemyLevel(type), type);
                newEnemy.healthbar = gm.healthbarManager.InitializeHealthBar(newEnemy);
                EnemyPawns.Add(newEnemy);

                newEnemy.transform.SetParent(EnemyRoot.transform);
                gm.hexMap.SetCharacterCell(newEnemy, cell);
            }
        }
        return newEnemy;
    }

    // level : 1 - 5
    public EnemyType getRandomEnemyType(int level)
    {
        int offset = (level - 1) * 5;
        int ran = Random.Range(offset, offset + 4);
        return (EnemyType)ran;
    }

    public EnemyType getHeroType(int level)
    {
        int offset = (level-1) * 5 + 4;
        return (EnemyType)offset;
    }

    public int getEnemyLevel(EnemyType type)
    {
        return (int)type / 5 + 1;
    }

    public List<Enemy> getCurrentEnemies()
    {
        return EnemyPawns;
    }
	
	public Enemy getDeadEnemy()
    {
        return DeadEnemyPawn;
    }

    public void setDeadEnemy(Enemy enemy = null)
    {
        DeadEnemyPawn = enemy;
    }
	
	public EnemyType getLastDeadEnemyType()
	{
		return LastDeadEnemyType;
	}

	public void setDeadEnemyType(EnemyType enemyType)
	{
		LastDeadEnemyType = enemyType;
	}

    public void RemoveEnemyPawn(Enemy enemy)
    {
        if(EnemyPawns.Contains(enemy))
        {
            EnemyPawns.Remove(enemy);
        }
    }
	
	public void testAltar()
	{
		//int ran = Random.Range(0, (int)EnemyType.NUM);
        //if(LastDeadEnemyType != EnemyType.NUM)
		// {
            //Enemy newEnemy = Instantiate<Enemy>(EnemyPrefab_sword);
            //Enemy newEnemy = Instantiate<Enemy>(prefabs[DeadEnemyPawn]);
            //gm.characterReader.InitEnemyData(ref newEnemy, getEnemyLevel(DeadEnemyPawn), DeadEnemyPawn);
            //DeadEnemyPawn = newEnemy;
            //Debug.Log("testAltar:" + newEnemy.enemyType.ToString());
		// }
	}

}
