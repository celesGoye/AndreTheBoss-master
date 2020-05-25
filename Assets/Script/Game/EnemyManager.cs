using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameManager gameManager;
    private CharacterReader characterReader;

    public Enemy EnemyPrefab_thief;
    public Enemy EnemyPrefab_sword;
    public Enemy EnemyPrefab_magic;

    private List<Enemy> EnemyPawns;

    private GameObject EnemyRoot;

    public static int[] heroAppearingTurn ={
        10, 20, 30,
    };


    public void OnEnable()
    {
        EnemyPawns = new List<Enemy>();
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        characterReader = gameManager.getCharacterReader();
        EnemyRoot = new GameObject();
        EnemyRoot.transform.SetParent(transform);
        EnemyRoot.transform.position = Vector3.zero;
    }

    public void SpawnEnemy()
    {
        int turnNum = gameManager.gameTurnManager.GetCurrentGameTurn();
        if(turnNum < heroAppearingTurn[0])          // level 1
        {
            
        }
        else if(turnNum < heroAppearingTurn[1])     // level 2
        {
            
        }
        else if(turnNum < heroAppearingTurn[2])     // level 3
        {

        }
        else if(turnNum < heroAppearingTurn[3])     // level 4
        {

        }
        else if(turnNum < heroAppearingTurn[4])     // level 5
        {

        }
    }

    public void SpawnEnemyAtCell(EnemyType type, HexCell cell)
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
            }		

            if(newEnemy != null)
            {
                gameManager.characterReader.InitEnemyData(ref newEnemy, getEnemyLevel(type), type);
                newEnemy.healthbar = gameManager.healthbarManager.InitializeHealthBar(newEnemy);
                EnemyPawns.Add(newEnemy);
            }
			
        }
    }

    public int getEnemyLevel(EnemyType type)
    {
        return (int)type / 5 + 1;
    }

    public List<Enemy> getCurrentEnemies()
    {
        return EnemyPawns;
    }

}
