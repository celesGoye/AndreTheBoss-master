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
    public Enemy EnemyPrefab_bandit;
    public Enemy EnemyPrefab_robinhood;
	
    public Enemy EnemyPrefab_banditcaptain;
    public Enemy EnemyPrefab_berserker;
    public Enemy EnemyPrefab_bard;
    public Enemy EnemyPrefab_magicmaster;
    public Enemy EnemyPrefab_tatenoyousta;
	
    public Enemy EnemyPrefab_magicmgraneaster;
    public Enemy EnemyPrefab_witch;
    public Enemy EnemyPrefab_assassin;
    //public Enemy EnemyPrefab_catapult;
    public Enemy EnemyPrefab_jinjyamiko;
	
    public Enemy EnemyPrefab_cultist;
    public Enemy EnemyPrefab_priest;
    public Enemy EnemyPrefab_bloodwitch;
    //public Enemy EnemyPrefab_darkknight;
    public Enemy EnemyPrefab_orchestraleader;
	
    public Enemy EnemyPrefab_magebelial;
    public Enemy EnemyPrefab_royalinquisitor;
    public Enemy EnemyPrefab_shadowfran;
    public Enemy EnemyPrefab_cardinaleriri;
    public Enemy EnemyPrefab_cinderlord;
    public Enemy EnemyPrefab_andrethehero;

    public List<Enemy> EnemyPawns;
	private Enemy DeadEnemyPawn = null;
    private EnemyType LastDeadEnemyType = EnemyType.NUM;

    private GameObject EnemyRoot;
	
	private EnemyLootReader reader;

    public int MaxEnemyOnMap = 10;

    private static int[] heroAppearingTurn ={
        5, 10, 15, 20, 25, 30
    };
	
	private Dictionary<EnemyType, Enemy> prefabs;

    public void InitEnemyManager()
    {
        EnemyPawns = new List<Enemy>();
        gm = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        EnemyRoot = new GameObject("EnemyRoot");
        EnemyRoot.transform.SetParent(transform);
        EnemyRoot.transform.position = Vector3.zero;
		
		reader=new EnemyLootReader();
		
		prefabs = new Dictionary<EnemyType, Enemy>
        {
            {EnemyType.wanderingswordman, EnemyPrefab_sword },
            {EnemyType.magicapprentice, EnemyPrefab_magic},
            {EnemyType.thief, EnemyPrefab_thief },
            {EnemyType.bandit, EnemyPrefab_bandit },
            {EnemyType.robinhood, EnemyPrefab_robinhood },
			
            {EnemyType.banditcaptain, EnemyPrefab_banditcaptain },
            {EnemyType.berserker, EnemyPrefab_berserker },
            {EnemyType.bard, EnemyPrefab_bard },
            {EnemyType.magicmaster, EnemyPrefab_magicmaster },
            {EnemyType.tatenoyousya, EnemyPrefab_tatenoyousta },
			
            {EnemyType.magicgrandmaster, EnemyPrefab_magicmgraneaster },
            {EnemyType.witch, EnemyPrefab_witch },
            {EnemyType.assassin, EnemyPrefab_assassin },
            //{EnemyType.catapult, EnemyPrefab_catapult },
            {EnemyType.jinjyamiko, EnemyPrefab_jinjyamiko },
			
            {EnemyType.cultist, EnemyPrefab_cultist },
            {EnemyType.priest, EnemyPrefab_priest },
            {EnemyType.bloodwitch, EnemyPrefab_bloodwitch },
            //{EnemyType.darkknight, EnemyPrefab_darkknight },
            {EnemyType.orchestraleader, EnemyPrefab_orchestraleader },
			
            {EnemyType.magebelial, EnemyPrefab_magebelial },
            {EnemyType.shadowfran, EnemyPrefab_shadowfran },
            {EnemyType.royalinquisitor, EnemyPrefab_royalinquisitor },
            {EnemyType.cardinaleriri, EnemyPrefab_cardinaleriri },
            {EnemyType.cinderlord, EnemyPrefab_cinderlord },

            //{EnemyType.andrethehero, EnemyPrefab_andrethehero },
        };
    }

    public void ClearEnemy()
    {
		Debug.Log("clearing "+EnemyPawns.Count);
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
        for (int i = 0; i < heroAppearingTurn.Length; i++)
        {
            if(heroAppearingTurn[i] == gm.gameTurnManager.GetCurrentGameTurn())
            {
                heroType = getHeroType(i + 1);
                break;
            }
        }
        if(heroType != EnemyType.NUM)
        {
            return SpawnEnemyAtCell(heroType, gm.hexMap.GetRandomCellToSpawn());
        }
        return null;
    }

    public Enemy SpawnEnemyAtCell(EnemyType type, HexCell cell)
    {
        Enemy newEnemy = null;
        if(cell.CanbeDestination())
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
				case EnemyType.bandit:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_bandit);
                    break;
                case EnemyType.robinhood:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_robinhood);
                    break;
				
				case EnemyType.banditcaptain:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_banditcaptain);
                    break;
                case EnemyType.berserker:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_berserker);
                    break;
                case EnemyType.bard:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_bard);
                    break;
				case EnemyType.magicmaster:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_magicmaster);
                    break;
                case EnemyType.tatenoyousya:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_tatenoyousta);
                    break;
					
				case EnemyType.magicgrandmaster:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_magicmgraneaster);
                    break;
                case EnemyType.witch:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_witch);
                    break;
                case EnemyType.assassin:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_assassin);
                    break;
				/*case EnemyType.catapult:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_catapult);
                    break;*/
                case EnemyType.jinjyamiko:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_jinjyamiko);
                    break;
					
				case EnemyType.cultist:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_cultist);
                    break;
                case EnemyType.priest:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_priest);
                    break;
                case EnemyType.bloodwitch:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_bloodwitch);
                    break;
				/*case EnemyType.darkknight:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_darkknight);
                    break;*/
                case EnemyType.orchestraleader:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_orchestraleader);
                    break;
					
				case EnemyType.magebelial:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_magebelial);
                    break;
                case EnemyType.royalinquisitor:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_royalinquisitor);
                    break;
                case EnemyType.shadowfran:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_shadowfran);
                    break;
				case EnemyType.cardinaleriri:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_cardinaleriri);
                    break;
                case EnemyType.cinderlord:
                    newEnemy = Instantiate<Enemy>(EnemyPrefab_cinderlord);
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

                //gm.hexMap.RevealCell(cell);
                gm.gameCamera.FocusOnPoint(cell.transform.localPosition);
				
				gm.animationManager.PlayCreateEff(newEnemy.transform.position);
            }
        }
        return newEnemy;
    }

    // level : 1 - 5
    public EnemyType getRandomEnemyType(int level)
    {
        int offset = (level - 1) * 5;
        int ran = Random.Range(offset, offset + 4);
		Debug.Log("enemytype "+ran);
        return (EnemyType)ran;
    }

    public EnemyType getHeroType(int level)
    {
        if (level == 6)
            return EnemyType.andrethehero;

        int offset = (level-1) * 5 + 4;
        if (offset >= (int)EnemyType.NUM)
            return EnemyType.NUM;
        else
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
	
	public void GetLoot(EnemyType type)
	{
		List<ItemEntry> list=new List<ItemEntry>();
		list=reader.GetItems(type,IsHero(type),((int)type)/5+1);
		if(list==null)
			return;
		Debug.Log("first,hello");
		List<ItemEntry> finalItems=new List<ItemEntry>();
		float posibility = (float)UnityEngine.Random.Range(1, 100);
		int sum=0;
		foreach(ItemEntry item in list)
		{
			if(item.posibility==100)
			{
				finalItems.Add(item);
				continue;
			}
			sum+=item.posibility;
			if(posibility<sum)
			{
				finalItems.Add(item);
				break;
			}
		}
		if(finalItems.Count>0)
			{
				foreach(ItemEntry item in finalItems)
				{
					gm.itemManager.GetItem(item.itemType, item.number);
				}
			}
	}
	
	public bool IsHero(EnemyType type)
	{
		return ((int)type+1)%5==0;
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
