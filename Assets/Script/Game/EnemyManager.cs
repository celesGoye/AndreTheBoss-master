using System.Collections;
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

    private List<Enemy> EnemyPawns;
	private Enemy DeadEnemyPawn;

    private GameObject EnemyRoot;

    public int MaxEnemyOnMap = 10;

    private static int[] heroAppearingTurn ={
        10, 20, 30, 40, 50
    };

    public void InitEnemyManager()
    {
        EnemyPawns = new List<Enemy>();
        gm = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        EnemyRoot = new GameObject("EnemyRoot");
        EnemyRoot.transform.SetParent(transform);
        EnemyRoot.transform.position = Vector3.zero;
    }

    public void OnEnemyTurnBegin()
    {
        // Spawn new Enemy
        if (!heroAppearingTurn.Contains<int>(gm.gameTurnManager.GetCurrentGameTurn()))
            SpawnEnemy();
        else
            SpawnHero();

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

    private void SpawnEnemy()
    {
        int turnNum = gm.gameTurnManager.GetCurrentGameTurn();

        if (EnemyPawns.Count >= MaxEnemyOnMap)
            return;

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
            SpawnEnemyAtCell(enemyType, gm.hexMap.GetRandomCellToSpawn());
    }

    private void SpawnHero()
    {
        EnemyType heroType = EnemyType.NUM;
        heroType = getHeroType(gm.gameTurnManager.GetCurrentGameTurn() / 10);
        if(heroType != EnemyType.NUM)
        {
            SpawnEnemyAtCell(heroType, gm.hexMap.GetRandomCellToSpawn());
        }
    }

    private void SpawnEnemyAtCell(EnemyType type, HexCell cell)
    {
        if(cell.CanbeDestination())
        {
            Enemy newEnemy = null;

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

                gm.hexMap.RevealCell(cell);
                gm.gameCamera.FocusOnPoint(cell.transform.localPosition);
            }
        }
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
	
	public void setDeadEnemy(Enemy enemy=null)
	{
		DeadEnemyPawn=enemy;
	}
	
	public void testAltar()
	{
		/*
		int ran = Random.Range(0, (int)EnemyType.NUM);
        Enemy newEnemy = Instantiate<Enemy>(EnemyPrefab_sword);
		gm.characterReader.InitEnemyData(ref newEnemy, getEnemyLevel((EnemyType)ran), (EnemyType)ran);
		DeadEnemyPawn=newEnemy;
		Debug.Log("testAltar:"+newEnemy.enemyType.ToString());
        */
	}

}
